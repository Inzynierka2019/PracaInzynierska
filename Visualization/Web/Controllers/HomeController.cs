namespace Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using Web.Models;
    using Web.Services;
    using Web.Services.Hubs;

    public class HomeController : Controller
    {
        private readonly ISignalLogger<SimHub> Log;

        public HomeController(ISignalLogger<SimHub> Log)
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
            /* example code */
            var host = HttpContext.Request.Host;
            var ip = HttpContext.Connection.RemoteIpAddress;
            Log.Error($"System console started: {host}");
            Log.Warning($"Your IP address is: {ip}");
            Log.Info("Simulation console started: Unity app not connected");
            Log.Debug("Exit: 0");

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
