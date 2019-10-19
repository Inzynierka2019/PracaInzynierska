using Common.Models;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class DataAggregationModule : MonoBehaviour
{
    private CommunicationModule communicationModule;
    private CancellationTokenSource communicationCancellationTokenSource = new CancellationTokenSource();
    private VehicleManager vehicleManager;

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

    private IEnumerator GatherVehiclePopulationData(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var vehiclePopulation = new VehiclePopulation();
            foreach (Transform vehicleTransform in vehicleManager.transform)
            {
                vehiclePopulation.VehiclePositions.Add(
                    new GeoPosition
                    {
                        Id = vehicleTransform.GetComponent<Vehicle>().id,
                        Latitude = vehicleTransform.position.x,
                        Longitude = vehicleTransform.position.y,
                    });

                if (cancellationToken.IsCancellationRequested)
                    break;
            }

            if (vehiclePopulation.VehicleCount != 0)
            {
                communicationModule.AddMessageToQueue(vehiclePopulation);
            }

            yield return new WaitForSeconds(5f);
        }
    }
}
