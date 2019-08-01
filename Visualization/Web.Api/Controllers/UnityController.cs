namespace Web.Api.Controllers
{
    using System;
    using Common.Models.Enums;
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
        public IActionResult BuildSimulation()
        {
            try
            {
                this.processService.ExecuteBuildSimulation();
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
        public IActionResult RunSimulation()
        {
            try
            {
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
