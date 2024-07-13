using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace TripPlanningAssistant.Common.Models
{
    [Table("knowledges_base")]
    public class KnowledgeBase : BaseModel
    {
        [PrimaryKey("id", false)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [Column("embedding")]
        [JsonPropertyName("embedding")]
        public string Embedding { get; set; } = string.Empty;
    }
}
