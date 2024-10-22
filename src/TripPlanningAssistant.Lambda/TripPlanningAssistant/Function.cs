using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using Supabase;
using System.Text.Json;
using TripPlanningAssistant.API.Services;
using TripPlanningAssistant.Common.Models;
using TripPlanningAssistant.Common.Options;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TripPlanningAssistant;

public class Function
{
    // TODO: Remove the hardcoded credential here and put it to somewhre safer.
    private readonly AWSBedrockService _awsBedrockService = new AWSBedrockService(new AWSBedrockConfigOptions()
    {
        AccessKeyId = "AKIAZ5JPWZFCM2QOMPP7",
        SecretAccessKey = "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",
        EmbeddingModelId = "amazon.titan-embed-text-v1",
        Region = "us-west-2"
    });
    private readonly Client _supabaseclient = new Client("https://mqqegwkbtnmqdudrlpdm.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1xcWVnd2tidG5tcWR1ZHJscGRtIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTcyMDYyNDIyNCwiZXhwIjoyMDM2MjAwMjI0fQ.uHP9JR6CcuV1fEzK5jEFjSIONyO1d7emekHlQn7krwI");

    public object FunctionHandler(JObject input, ILambdaContext context)
    {
        context.Logger.LogInformation(input.ToString());
        var inputObject = input.ToObject<AgentObject>();

        object responseBody = new { };
        List<string> results = new List<string>();
        string? userInput = inputObject.parameters.FirstOrDefault(x => x.name == "input")?.value
                ?? inputObject.inputText;

        if (inputObject is not null && inputObject.function == "sematic_search")
        {
            results.AddRange(SematicSearch(userInput, "match_knowledges").Result);

            responseBody = new { TEXT = new { body = JsonSerializer.Serialize(results) }};

            if (results.Count == 0)
                responseBody = new { TEXT = new { body = "Sorry! I will need to ask my creator for more knowledge base because I do have needed information to process your requests." } };
        }
        else if (inputObject is not null && inputObject.function == "obtain_knowledge")
        {
            var embedding = _awsBedrockService.GenerateEmbeddingsResponseAsync(userInput).Result;
            var data = new KnowledgeBase()
            {
                Content = userInput,
                Embedding = embedding
            };
            var insertResult = _supabaseclient.From<KnowledgeBase>().Insert(data).Result;

            if (insertResult.ResponseMessage?.StatusCode == System.Net.HttpStatusCode.Created)
                responseBody = new { TEXT = new { body = JsonSerializer.Serialize("Knowledge obtained and save.") } };
            else
                responseBody = new { TEXT = new { body = JsonSerializer.Serialize("Something is wrong with the action and the knowledge couldn't be obtained.") } };
        }
        
        var response = new
        {
            response = new
            {
                actionGroup = inputObject.actionGroup,
                function = inputObject.function,
                functionResponse = new
                {
                    responseBody = responseBody
                }
            },
            messageVersion = inputObject.messageVersion
        };

        context.Logger.LogInformation(response.ToString());

        return response;
    }

    public async Task<IEnumerable<string>> SematicSearch(string input, string targetFunction, Single matchThreshold = 0.6f, int count = 3)
    {
        var inputEmbedding = await _awsBedrockService.GenerateEmbeddingsResponseAsync(input);

        var result = await _supabaseclient.Rpc(targetFunction, new
        {
            query_embedding = inputEmbedding, // pass the query embedding
            match_threshold = matchThreshold, // choose an appropriate threshold for your data
            match_count = count, // choose the number of matches
        });

        var convertedResult = JsonSerializer.Deserialize<IEnumerable<KnowledgeBase>>(result.Content ?? "");
        return convertedResult?.Select(x => x.Content) ?? new List<string>();
    }
}