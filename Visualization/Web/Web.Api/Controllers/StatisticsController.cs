namespace Web.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;
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
                    this.Log.Info($"Receiving vehicle data from simulation: {population}");

                    await this.statisticsService.UpdatePersonalityStats(population);
                    await this.statisticsService.UpdateVehiclePopulation(population);
                    await this.statisticsService.UpdateMomentarySpeeds(population);

                    return Ok();
                }
                else
                {
                    return ValidationProblem();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("driverReport")]
        public IActionResult UpdateDriverReport([FromBody] DriverReport report)
        {
            try
            {
                if (TryValidateModel(report))
                {
                    this.Log.Info($"Receiving driver report: {report}");

                    this.statisticsService.UpdateDriverReports(report);

                    return Ok();
                }
                else
                {
                    return ValidationProblem();
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
                this.Log.Info($"Requesting drivers statistics from simulation");
                var reports = this.statisticsService.GetDriverReports();

                return this.Ok(reports);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
    }
}