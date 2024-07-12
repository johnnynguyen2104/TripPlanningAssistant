﻿namespace TripPlanningAssistant.Lambda.Options
{
    public class AWSBedrockConfigOptions
    {
        public string AccessKeyId { get; set; } = string.Empty;
        public string SecretAccessKey { get; set; } = string.Empty;
        public string Token { get;set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string ChatModelId { get; set; } = string.Empty;
        public string EmbeddingModelId { get; set; } = string.Empty;
    }
}