using Microsoft.AspNetCore.Mvc;
using Supabase;
using System.Text.Json;
using TripPlanningAssistant.API.Models;
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
        public async Task<IEnumerable<string>> SematicSearch(string input, Single matchThreshold = 0.5f, int count = 10)
        {
            var inputEmbedding = await _awsBedrockService.GenerateEmbeddingsResponseAsync(input);

            var result = await _client.Rpc("match_documents", new 
            {
                query_embedding = inputEmbedding, // pass the query embedding
                match_threshold = matchThreshold, // choose an appropriate threshold for your data
                match_count = count, // choose the number of matches
            });

            var convertedResult = JsonSerializer.Deserialize<IEnumerable<BaseModel>>(result.Content ?? "");
            return convertedResult?.Select(x => x.sentences) ?? new List<string>();
        }
    }
}
