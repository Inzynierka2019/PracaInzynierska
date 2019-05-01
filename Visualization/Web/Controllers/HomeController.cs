namespace Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Web.Models;

    public class HomeController : Controller
    {
        private readonly ILogger Log;

        public HomeController(ILogger Log)
        {
            this.Log = Log;
        }

        [Route("")]
        [Route("Home")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Console")]
        public IActionResult Console()
        {
            /* code example */
            var host = HttpContext.Request.Host;
            var ip = HttpContext.Connection.LocalIpAddress;
            Log.LogError($"System console started: {host}");
            Log.LogWarning($"Your IP address is: {ip}");
            Log.LogInformation("Simulation console started: Unity app not connected");
            Log.LogDebug("Exit: 0");

            return View();
        }

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("About")]
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
