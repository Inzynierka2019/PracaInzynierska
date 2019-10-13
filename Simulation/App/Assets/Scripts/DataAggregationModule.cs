using Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
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
        communicationModule = new CommunicationModule(communicationCancellationTokenSource.Token);
        StartCoroutine(GatherVehiclePopulationData(communicationCancellationTokenSource.Token));
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
                vehiclePopulation.vehiclePositions.Add(
                    Tuple.Create(
                        vehicleTransform.position.x,
                        vehicleTransform.position.y,
                        vehicleTransform.GetComponent<Vehicle>().id));

                if (cancellationToken.IsCancellationRequested)
                    break;


            }
            communicationModule.AddMessageToQueue(vehiclePopulation);
            yield return new WaitForSeconds(1f);
        }
    }
}
