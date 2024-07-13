using Supabase;

namespace TripPlanningAssistant.Common.Options
{
    public class SupabaseDbOptions
    {
        public string SupabaseUrl { get; set; } = string.Empty;

        public string SupabaseKey { get; set; } = string.Empty;

        public SupabaseOptions? SupabaseOptions { get; set; } 
    }
}
