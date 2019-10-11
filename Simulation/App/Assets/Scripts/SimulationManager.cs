using Common.Communication;
using Common.Models;
using Libraries.Web;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class SimulationManager : MonoBehaviour
{
    static SimulationManager instance = null;

    [SerializeField] JunctionManager junctionManager;
    [SerializeField] RoadManager roadManager;
    [SerializeField] VehicleManager vehicleManager;

    private readonly AppConnector appConnector = new AppConnector(new UnityDebugLogger(), "https://localhost:5001/UIHub");
    private readonly AppUpdater appUpdater = new AppUpdater(new UnityDebugLogger(), "https://localhost:5001/UIHub");
    private static ConcurrentQueue<Action> MainThreadTaskQueue = new ConcurrentQueue<Action>();

    private static Task communicationThread;
    private static BlockingCollection<IMessage> messageQueue = new BlockingCollection<IMessage>();
    public static CancellationTokenSource AppCancellationTokenSource { private set; get; } = new CancellationTokenSource();

    public static JunctionManager JunctionManager
    {
        get => instance?.junctionManager;
        set { if (instance != null) instance.junctionManager = value; }
    }

    public static RoadManager RoadManager
    {
        get => instance?.roadManager;
        set { if (instance != null) instance.roadManager = value; }
    }

    public static VehicleManager VehicleManager
    {
        get => instance?.vehicleManager;
        set { if (instance != null) instance.vehicleManager = value; }
    }

    public static void ScheduleTaskOnMainThread(Action action)
    {
        MainThreadTaskQueue.Enqueue(action);
    }

    public static void Rebuild()
    {
        Debug.Log("Rebuilding. Please hold");
        JunctionManager.RebuildRoads();
        Debug.Log("Rebuilding. Done.");
    }

    void Start()
    {
        if (instance != null)
            Debug.LogError("Too many simulation managers! Leave just one in hierarchy.");
        instance = this;

        if(junctionManager == null)
        {
            GameObject go = new GameObject("JunctionManager");
            go.transform.SetParent(transform);
            junctionManager = go.AddComponent<JunctionManager>();
        }
        if (roadManager == null)
        {
            GameObject go = new GameObject("RoadManager");
            go.transform.SetParent(transform);
            roadManager = go.AddComponent<RoadManager>();
        }
        if (vehicleManager == null)
        {
            GameObject go = new GameObject("VehicleManager");
            go.transform.SetParent(transform);
            vehicleManager = go.AddComponent<VehicleManager>();
        }

        Rebuild();
        this.appConnector.KeepAlive();

        communicationThread = Task.Factory.StartNew(() =>
        {
            foreach(var message in messageQueue.GetConsumingEnumerable(AppCancellationTokenSource.Token))
            {
                if (message is VehiclePopulation population)
                    appUpdater.UpdateVehiclePopulationPositions(population);
            }
        }, TaskCreationOptions.LongRunning);

        StartCoroutine(onCoroutine(AppCancellationTokenSource.Token));
    }
    
    void Update()
    {
        while (Application.isPlaying && !MainThreadTaskQueue.IsEmpty)
        {
            if (MainThreadTaskQueue.TryDequeue(out Action action))
            {
                action();
            }
        }
    }

    public SimulationManager()
    {
        // This line is necessary to actively ignore security concerns involving Mono certificate trust issues.
        ServicePointManager.ServerCertificateValidationCallback += (p1, p2, p3, p4) => true;
    }

    void OnApplicationQuit()
    {
        // disconnects from web server and informs that app has closed.
        this.appConnector.Dispose();
        AppCancellationTokenSource.Cancel();
        try
        {
            communicationThread.Wait();
        }
        catch (OperationCanceledException)
        {
            //Do nothing.It's an expected behaviour
        }
    }

    private IEnumerator onCoroutine(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var vehiclePopulation = new VehiclePopulation();
            foreach (Transform vehicleTransform in VehicleManager.transform)
            {
                vehiclePopulation.vehiclePositions.Add(
                    Tuple.Create(
                        vehicleTransform.position.x,
                        vehicleTransform.position.y,
                        vehicleTransform.GetComponent<Vehicle>().id));


                messageQueue.Add(vehiclePopulation);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
