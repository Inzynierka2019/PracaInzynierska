using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inzynierka.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Inzynierka.Controllers
{
    [Route("api/console")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        public ConsoleController()
        {
        }
    }
}