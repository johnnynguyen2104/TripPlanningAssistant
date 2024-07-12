using Microsoft.AspNetCore.Mvc;
using Supabase;
using TripPlanningAssistant.API.Services;

namespace TripPlanningAssistant.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ILogger<TripController> _logger;
        private readonly Client _client;
        private readonly AWSBedrockService _awsBedrockService;

        public TripController(ILogger<TripController> logger, 
            Client client,
            AWSBedrockService awsBedrockService)
        {
            _logger = logger;
            _client = client;
            _awsBedrockService = awsBedrockService;
        }

        [HttpGet]
        public async Task Get(string input)
        {
            var inputEmbedding = await _awsBedrockService.GenerateEmbeddingsResponseAsync(input);

            var result = _client.Rpc("match_attractions", new 
            {
                query_embedding = inputEmbedding, // pass the query embedding
                match_threshold = 0.78, // choose an appropriate threshold for your data
                match_count = 10, // choose the number of matches
            });
        }
    }
}
