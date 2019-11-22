using Common.Models;
using Common.Models.Enums;
using System.Threading.Tasks;

class AppUpdater
{
    private readonly ApiClient ApiClient = new ApiClient();
    public UnityAppState AppState = UnityAppState.NOT_CONNECTED;

    public AppUpdater()
    {
    }

    public async Task Update(VehiclePopulation population)
    {
        population.VehicleStatuses.ForEach(x => x.CurrentSpeed *= 3.6f); // change to km/h
        await this.ApiClient.SendAsync(population, Endpoints.VehiclePopulation);
    }

    public async Task Update(DriverReport report)
    {
        await this.ApiClient.SendAsync(report, Endpoints.DriverReport);
    }

    public async Task Start()
    {
        await ApiClient.GetAsync(Endpoints.Connect);
        this.AppState = UnityAppState.CONNECTED;
    }

    public async Task Stop()
    {
        await ApiClient.GetAsync(Endpoints.Disconnect);
        this.AppState = UnityAppState.DISCONNECTED;
    }
}