namespace Web.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Common.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Web.Logic.Hubs;
    using Web.Logic.Services;

    [Route("api/stats")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ILog Log;
        private readonly IHubContext<UIHub> hubContext;
        private readonly IStatisticsService statisticsService;

        public StatisticsController(ILog log, IHubContext<UIHub> hubContext, IStatisticsService statisticsService)
        {
            this.Log = log;
            this.hubContext = hubContext;
            this.statisticsService = statisticsService;
        }

        [HttpPut]
        [Route("vehiclePopulationPositions")]
        public async Task<IActionResult> UpdateVehiclePopulationPositions([FromBody] VehiclePopulation population)
        {
            try
            {
                if (TryValidateModel(population))
                {
                    await this.statisticsService.UpdateVehiclePopulation(population);

                    return Ok();
                }
                else
                {
                    return UnprocessableEntity();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("summary")]
        public IActionResult SummaryReport()
        {
            try
            {
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
    }
}