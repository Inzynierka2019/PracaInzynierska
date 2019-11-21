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
            TravelTime = vehicle.createTime - DateTime.Now,
            AvgSpeed = vehicle.sumVelocity / vehicle.framesCount,
            RouteTarget = vehicle.roadTypeName,
            Driver = vehicle.driver
        });
    }

    private IEnumerator GatherVehiclePopulationData(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var vehiclePopulation = new VehiclePopulation();
            foreach (Vehicle vehicle in vehicleManager.vehicles)
            {
                vehiclePopulation.VehicleStatuses.Add(
                    new VehicleStatus
                    {
                        Id = vehicle.id,
                        Latitude = vehicle.transform.position.y,
                        Longitude = vehicle.transform.position.x,
                        CurrentSpeed = vehicle.velocity
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
