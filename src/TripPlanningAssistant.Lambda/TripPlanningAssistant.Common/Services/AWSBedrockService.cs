using System.Text.Json.Nodes;
using System.Text.Json;
using Amazon.BedrockRuntime;
using Amazon.Runtime;
using Amazon;
using Amazon.BedrockRuntime.Model;
using TripPlanningAssistant.Common.Options;
using Amazon.BedrockAgentRuntime;
using Amazon.BedrockAgentRuntime.Model;
using InferenceConfiguration = Amazon.BedrockRuntime.Model.InferenceConfiguration;
using System.Text;
using Microsoft.Extensions.Options;

namespace TripPlanningAssistant.API.Services
{
    public class AWSBedrockService
    {
        private readonly AmazonBedrockRuntimeClient _bedrockClient;
        private readonly AWSBedrockConfigOptions _config;
        private readonly AmazonBedrockAgentRuntimeClient _bedrockAgentClient;

        public AWSBedrockService(IOptions<AWSBedrockConfigOptions> config)
        {
            _config = config.Value;
            var credentials = new BasicAWSCredentials(_config.AccessKeyId, _config.SecretAccessKey);
            var region = RegionEndpoint.GetBySystemName(_config.Region);
            _bedrockClient = new AmazonBedrockRuntimeClient(credentials, region);
            _bedrockAgentClient = new AmazonBedrockAgentRuntimeClient(credentials, region);
        }

        public AWSBedrockService(AWSBedrockConfigOptions config)
        {
            _config = config;
            var credentials = new BasicAWSCredentials(_config.AccessKeyId, _config.SecretAccessKey);
            var region = RegionEndpoint.GetBySystemName(_config.Region);
            _bedrockClient = new AmazonBedrockRuntimeClient(credentials, region);
            _bedrockAgentClient = new AmazonBedrockAgentRuntimeClient(credentials, region);
        }

        public async Task<string> TalkToAgent(string input)
        {
            var response = await _bedrockAgentClient.InvokeAgentAsync(new InvokeAgentRequest()
            {
                AgentId = "N4KCBYD26S",
                AgentAliasId = "3GQ86ILTDD",
                SessionId = Guid.NewGuid().ToString(),
                InputText = input
            });

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                MemoryStream output = new MemoryStream();
                foreach (Amazon.BedrockAgentRuntime.Model.PayloadPart item in response.Completion)
                {
                    item.Bytes.CopyTo(output);
                }
                return Encoding.UTF8.GetString(output.ToArray());
            }

            return "Couldn't connect to the Agent at the moment. Please try again later.";
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
