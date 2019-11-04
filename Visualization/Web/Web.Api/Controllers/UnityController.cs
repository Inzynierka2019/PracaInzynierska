namespace Web.Api.Controllers
{
    using System;
    using Common.Models.Enums;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    using Web.Logic.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class UnityController : ControllerBase
    {
        private readonly IProcessService processService;

        private readonly ILog Log;

        public UnityController(IProcessService processService, ILog log)
        {
            this.processService = processService;
            this.Log = log;
        }

        [HttpGet]
        [Route("run")]
        [EnableCors("CorsPolicy")]
        public IActionResult RunSimulation()
        {
            try
            {
                Log.Warn("Simulation.exe has started.");
                this.processService.ExecuteRunSimulationWindows();
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in running simulation: {e.Message}");

                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
