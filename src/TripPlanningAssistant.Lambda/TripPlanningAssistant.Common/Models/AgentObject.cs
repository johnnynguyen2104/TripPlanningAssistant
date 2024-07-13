namespace TripPlanningAssistant.Common.Models
{
    public class AgentObject
    {
        public string function { get; set; }
        public List<Parameter> parameters { get; set; }
        public string sessionId { get; set; }
        public string inputText { get; set; }
        public string actionGroup { get; set; }
        public string messageVersion { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }
}
