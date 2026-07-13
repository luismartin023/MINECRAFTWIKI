using System.Text.Json.Serialization;

//XD

namespace MINECRAFTWIKI.MODELS
{
    public class MinecraftEntity
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("health")]
        public string Health { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
