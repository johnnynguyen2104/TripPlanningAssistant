namespace TripPlanningAssistant.Common.Options
{
    public class AWSBedrockConfigOptions
    {
        public string AccessKeyId { get; set; } = string.Empty;
        public string SecretAccessKey { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string? ChatModelId { get; set; }
        public string? EmbeddingModelId { get; set; }
    }
}
