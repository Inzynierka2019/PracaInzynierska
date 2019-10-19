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
        await this.ApiClient.SendAsync(population, Endpoints.VehiclePopulation);
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