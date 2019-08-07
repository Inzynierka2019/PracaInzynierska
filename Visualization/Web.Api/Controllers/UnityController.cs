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
        [Route("build")]
        [EnableCors("CorsPolicy")]
        public IActionResult BuildSimulation()
        {
            try
            {
                Log.Info("Simulation build has started.", LogType.Warning);
                if(this.processService.ExecuteBuildSimulation())
                    Log.Info("Build has successfully finished!", LogType.Success);
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in building simulation: {e.Message}", LogType.Error);

                return this.BadRequest();
            }

            return this.Ok();
        }

        [HttpGet]
        [Route("run")]
        [EnableCors("CorsPolicy")]
        public IActionResult RunSimulation()
        {
            try
            {
                Log.Info("Simulation.exe has started.", LogType.Warning);
                this.processService.ExecuteRunSimulation();
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in running simulation: {e.Message}", LogType.Error);

                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
