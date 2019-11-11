using Common.Models;
using Common.Models.Models;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class DataAggregationModule : MonoBehaviour
{
    private CommunicationModule communicationModule;
    private CancellationTokenSource communicationCancellationTokenSource = new CancellationTokenSource();
    private VehicleManager vehicleManager;
    private readonly float updateIntervalInSeconds = 1f;

    public void Init(VehicleManager vehicleManager)
    {
        this.vehicleManager = vehicleManager;
        if (Application.isPlaying)
        {
            communicationModule = new CommunicationModule(communicationCancellationTokenSource.Token);
            StartCoroutine(GatherVehiclePopulationData(communicationCancellationTokenSource.Token));
        }
    }

    public void StopAndWaitForShutdown()
    {
        communicationCancellationTokenSource.Cancel();
        StopAllCoroutines();
        communicationModule.WaitForShutdown();
    }

    public void CreateDriverReport(Vehicle vehicle)
    {
        communicationModule.AddMessageToQueue(new DriverReport()
        {
            TravelTime = TimeSpan.FromSeconds(new System.Random().Next(10, 543)),
            AvgSpeed = new System.Random().Next(10, 543),
            RouteTarget = "Kaczka",
            Driver = new SlowDriver()
        });
    }

    private IEnumerator GatherVehiclePopulationData(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var vehiclePopulation = new VehiclePopulation();
            foreach (Transform vehicleTransform in vehicleManager.transform)
            {
                vehiclePopulation.VehiclePositions.Add(
                    new VehicleStatus
                    {
                        Id = vehicleTransform.GetComponent<Vehicle>().id,
                        Latitude = vehicleTransform.position.y,
                        Longitude = vehicleTransform.position.x,
                        CurrentSpeed = vehicleTransform.GetComponent<Vehicle>().velocity
                    });

                if (cancellationToken.IsCancellationRequested)
                    break;
            }

            if (vehiclePopulation.VehicleCount != 0)
            {
                communicationModule.AddMessageToQueue(vehiclePopulation);
            }

            yield return new WaitForSeconds(this.updateIntervalInSeconds);
        }
    }
}
