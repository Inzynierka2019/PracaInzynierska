namespace Web.Api.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using log4net;

    using Web.Logic.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class UnityController : ControllerBase
    {
        private readonly IProcessService processService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(UnityController));

        public UnityController(IProcessService processService)
        {
            this.processService = processService;
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
                Log.Error("An error occured in building simulation: ", e);
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
                Log.Error("An error occured in running simulation: ", e);
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
