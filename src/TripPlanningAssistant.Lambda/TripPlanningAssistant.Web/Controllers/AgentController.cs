using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using TripPlanningAssistant.API.Services;
using TripPlanningAssistant.Web.Models;

namespace TripPlanningAssistant.Web.Controllers
{
    public class AgentController : Controller
    {
        private readonly ILogger<AgentController> _logger;
        private readonly AWSBedrockService _awsBedrockService;

        public AgentController(ILogger<AgentController> logger,
            AWSBedrockService awsBedrockService)
        {
            _logger = logger;
            _awsBedrockService = awsBedrockService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("api/chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            return Ok(await _awsBedrockService.TalkToAgent(request.Input));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ChatRequest
    {
        public string Input { get; set; } = string.Empty;
    }
}
