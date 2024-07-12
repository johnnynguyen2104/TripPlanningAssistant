using Supabase.Postgrest.Attributes;

namespace TripPlanningAssistant.API.Models
{
    [Table("attractions")]
    public class Attraction: Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("sentences")]
        public string Sentences { get; set; }

        [Column("embedding")]
        public string Embedding { get; set; }
    }
}
