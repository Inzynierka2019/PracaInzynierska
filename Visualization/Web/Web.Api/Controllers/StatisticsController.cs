using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Communication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Logic.Hubs;
using Web.Logic.Services;

namespace Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ILog Log;
        private readonly IUnityAppManager appManager;
        private readonly IHubContext<UIHub> hubContext;
        public StatisticsController(ILog log, IUnityAppManager appManager, IHubContext<UIHub> hubContext)
        {
            this.Log = log;
            this.appManager = appManager;
            this.hubContext = hubContext;
        }

        [HttpGet]
        [Route("connect")]
        public IActionResult ConnectWithWebClient()
        {
            try
            {
                this.hubContext.Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus.Method, true);

                return this.Ok();
            }
            catch(Exception ex)
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
                this.hubContext.Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus.Method, false);

                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("summary")]
        public IActionResult SummaryReport()
        {
            try
            {
                this.hubContext.Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus.Method, false);

                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest();
            }
        }
    }
}