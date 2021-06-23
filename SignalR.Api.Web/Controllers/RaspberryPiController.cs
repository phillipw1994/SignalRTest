using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.Api.Model;
using SignalR.Api.Web.Hubs;

namespace SignalR.Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaspberryPiController : ControllerBase
    {
        public IHubContext<DeviceHub> HubContext { get; }

        private ILogger<RaspberryPiController> Logger { get; }

        public RaspberryPiController(ILogger<RaspberryPiController> logger, IHubContext<DeviceHub> hubContext)
        {
            HubContext = hubContext;
            Logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await HubContext.Clients.Group("Devices").SendAsync("ReceiveMessage", "Test User", "Test Message");
            return Ok();
        }

        [HttpPost("led")]
        public async Task<IActionResult> PostLed(bool status)
        {
            await HubContext.Clients.Group("Devices").SendAsync("LEDToggle", status);
            return Ok();
        }

        [HttpPost("lcd")]
        public async Task<IActionResult> PostLcdMessage(LcdMessage lcdMessage)
        {
            await HubContext.Clients.Group("Devices").SendAsync("LcdMessage", lcdMessage.Message);
            return Ok(lcdMessage);
        }

        [HttpPost("consolemessage")]
        public async Task<IActionResult> PostConsoleMessage(ConsoleMessage consoleMessage)
        {
            await HubContext.Clients.Group("Devices").SendAsync("consoleMessage", consoleMessage.Message);
            return Ok(consoleMessage);
        }

        [HttpPost("displaycurrenttime")]
        public async Task<IActionResult> PostCurrentTimeMessage()
        {
            await HubContext.Clients.Group("Devices").SendAsync("DisplayCurrentTime");
            return Ok();
        }
    }
}
