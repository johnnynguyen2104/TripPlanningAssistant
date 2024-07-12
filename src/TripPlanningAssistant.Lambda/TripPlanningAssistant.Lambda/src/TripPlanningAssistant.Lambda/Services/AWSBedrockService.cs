using System.Text.Json.Nodes;
using System.Text.Json;
using Amazon.BedrockRuntime;
using TripPlanningAssistant.Lambda.Options;
using Amazon.Runtime;
using Amazon;
using Amazon.BedrockRuntime.Model;
using Microsoft.Extensions.Options;

namespace TripPlanningAssistant.API.Services
{
    public class AWSBedrockService
    {
        private readonly AmazonBedrockRuntimeClient _bedrockClient;
        private readonly AWSBedrockConfigOptions _config;

        public AWSBedrockService(IOptions<AWSBedrockConfigOptions> config)
        {
            _config = config.Value;
            var credentials = new SessionAWSCredentials(_config.AccessKeyId, _config.SecretAccessKey, _config.Token);
            var region = RegionEndpoint.GetBySystemName(_config.Region);
            _bedrockClient = new AmazonBedrockRuntimeClient(credentials, region);
        }

        public async Task<string> GenerateEmbeddingsResponseAsync(string input, InferenceConfiguration? inferenceConfiguration = null)
        {
            var nativeRequest = JsonSerializer.Serialize(new
            {
                inputText = input
            });

            var request = new InvokeModelRequest()
            {
                ModelId = _config.EmbeddingModelId,
                Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(nativeRequest)),
                ContentType = "application/json"
            };

            // Send the request to the Bedrock Runtime and wait for the response.
            var response = await _bedrockClient.InvokeModelAsync(request);

            // Decode the response body.
            var modelResponse = await JsonNode.ParseAsync(response.Body);

            // Extract and print the response text.
            var responseText = modelResponse?["embedding"] ?? "";
            return responseText.ToString();
        }

        public async Task<string> GenerateResponseAsync(string input, InferenceConfiguration? inferenceConfiguration = null)
        {
            var request = new ConverseRequest
            {
                ModelId = _config.ChatModelId,
                Messages = new List<Message>
                {
                    new Message
                    {
                        Role = ConversationRole.User,
                        Content = new List<ContentBlock> { new ContentBlock { Text = input } }
                    }
                },
                InferenceConfig = new InferenceConfiguration()
                {
                    MaxTokens = inferenceConfiguration is null ? 512 : inferenceConfiguration.MaxTokens,
                    Temperature = inferenceConfiguration is null ? 0.5F : inferenceConfiguration.Temperature,
                    TopP = inferenceConfiguration is null ? 0.9F : inferenceConfiguration.TopP
                }
            };

            var response = await _bedrockClient.ConverseAsync(request);

            // Extract and print the response text.
            string responseText = response?.Output?.Message?.Content?[0]?.Text ?? "";
            return responseText;
        }
    }
}
