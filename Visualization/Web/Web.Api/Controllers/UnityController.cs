﻿namespace Web.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Common.Models;
    using Common.Models.Enums;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Web.Logic.Hubs;
    using Web.Logic.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class UnityController : ControllerBase
    {
        private readonly IProcessService processService;
        private readonly ILog Log;
        private readonly IUnityAppManager appManager;
        private readonly IHubContext<UIHub> hubContext;

        public UnityController(IProcessService processService, ILog log, IUnityAppManager appManager, IHubContext<UIHub> hubContext)
        {
            this.processService = processService;
            this.Log = log;
            this.appManager = appManager;
            this.hubContext = hubContext;
        }

        [HttpGet]
        [Route("run")]
        [EnableCors("CorsPolicy")]
        public IActionResult RunSimulation()
        {
            try
            {
                Log.Info("Simulation.exe has started.");
                this.processService.ExecuteRunSimulationWindows();
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in running simulation: {e.Message}");

                return this.BadRequest();
            }

            return this.Ok();
        }

        [HttpGet]
        [Route("timespan")]
        [EnableCors("CorsPolicy")]
        public IActionResult GetUnityConnectionTimeSpan()
        {
            try
            {
                var timespan = appManager.GetTimeSpan();

                return this.Ok(new DateTime(timespan.Ticks));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("connect")]
        public IActionResult ConnectWithWebClient()
        {
            try
            {
                appManager.UpdateState(UnityAppState.CONNECTED);

                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("disconnect")]
        public IActionResult DisconnectWithWebClient()
        {
            try
            {
                appManager.UpdateState(UnityAppState.DISCONNECTED);

                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("preferences")]
        public IActionResult LoadSimulationPreferences()
        {
            try
            {
                var simulationPreferences = this.processService.GetJsonSimulationPreferences();

                return this.Ok(simulationPreferences);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("preferences")]
        public IActionResult SaveSimulationPreferences([FromBody] SimulationPreferences preferences)
        {
            try
            {
                if (TryValidateModel(preferences))
                {
                    this.processService.SaveJsonSimulationPreferences(preferences);
                    return this.Ok();
                }

                return this.ValidationProblem();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("geoposition")]
        public IActionResult GetGeoPositionReference()
        {
            try
            {
                var simPreferences = this.processService.GetJsonSimulationPreferences();
                var sceneData = simPreferences.availableScenes.Find(x => x.name == simPreferences.currentSceneName);
                return this.Ok(new { sceneData.latitude, sceneData.longitude });
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
    }
}
