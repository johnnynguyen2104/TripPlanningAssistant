using Supabase.Postgrest.Attributes;

namespace TripPlanningAssistant.API.Models
{
    [Table("knowledges_base")]
    public class KnowledgeBase : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("embedding")]
        public string Embedding { get; set; }
    }
}
