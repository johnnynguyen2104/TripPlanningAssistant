using Amazon.Lambda.Core;
using System.Text.Json;
using TripPlanningAssistant.API.Services;
using TripPlanningAssistant.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TripPlanningAssistant;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and returns both the upper and lower case version of the string.
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public Casing FunctionHandler(string input, ILambdaContext context)
    {
        Console.WriteLine(input);
        return new Casing(input.ToLower(), input.ToUpper());
    }

    public async Task<IEnumerable<string>> SematicSearch(string input, string targetFunction, Single matchThreshold = 0.5f, int count = 10)
    {
        var awsBedrockService = new AWSBedrockService(new Options.AWSBedrockConfigOptions()
        {
            AccessKeyId = "ASIAZ5JPWZFCFRNTPJPB",
            SecretAccessKey = "ToyiunqMXEj4Z26xdVG9OpyzRHS8qUYK/K0GrIpp",
            Token = "IQoJb3JpZ2luX2VjEKX//////////wEaCXVzLWVhc3QtMSJHMEUCIQCMLOPL0DO5hvFoYZG0hcFIKONjCj6pT2Q1IZox+4sjkwIgaIhxnAg84rbi2MMXxORTqjPUn4+2nC4s7NkqteJ+yJcqmQIIbhABGgw2ODEzODkyNDY3ODgiDKR4w1VxQgldJeLFYCr2AeKY+PceqZk7vBU98WkZ70TyynXs+Tdo/WD9qKg5Za0rUDnymvQbnpPUydOzjBk35FsplsRiRd910W4Ze3rvL6uB2Y6qdfVjuhsdJoXUIpDCXpd60I7q2ZH1x9TnnCRVQ2wi/l48xV/tzFjhsiAYUP7zZxUGimzHtlBsjBMrqrYyKruoyF8ohtbSSBfc2cWEX1oXloLc38SYK8LuvUU6R0V6zwq2ksZZrmBcnTKpv5b+w1BAZGIOt2zblCfIZadOOrYaiHCMV8k9UnLZZHuJ2eGmG6pm/3X+a0vYqsLAoK567hHbRJfA61F9QJPTqaZYd3MgMFaR9jC/7cK0BjqdAalpVGlwgNfdF8Wqu90c0raRi8aHcCnUG24OgSoQKA2fAnBUfQPrZ1azDITbn3xrdD/f/v7UxUpsygxy6zvTCqandkF14LKGVquqSaab1mq2OyBSiOwnDPBvflFsw5wwV9Au+UwIvsOljHMgKQBfOi2izlUpAHNAeTEEHjqgBWp/o+8nMQPMRRxcfZR8msaPoHtnNU3qlBKSkApBSTM=",
            EmbeddingModelId = "amazon.titan-embed-text-v1",
            Region = "us-west-2"
        });
        var client = new Supabase.Client("https://mqqegwkbtnmqdudrlpdm.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1xcWVnd2tidG5tcWR1ZHJscGRtIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTcyMDYyNDIyNCwiZXhwIjoyMDM2MjAwMjI0fQ.uHP9JR6CcuV1fEzK5jEFjSIONyO1d7emekHlQn7krwI");

        var inputEmbedding = await awsBedrockService.GenerateEmbeddingsResponseAsync(input);

        var result = await client.Rpc(targetFunction, new
        {
            query_embedding = inputEmbedding, // pass the query embedding
            match_threshold = matchThreshold, // choose an appropriate threshold for your data
            match_count = count, // choose the number of matches
        });

        var convertedResult = JsonSerializer.Deserialize<IEnumerable<BaseModel>>(result.Content ?? "");
        return convertedResult?.Select(x => x.sentences) ?? new List<string>();
    }
}

public record Casing(string Lower, string Upper);