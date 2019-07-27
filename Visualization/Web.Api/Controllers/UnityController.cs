using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Logic.Services;

namespace Web.Api.Controllers
{
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
