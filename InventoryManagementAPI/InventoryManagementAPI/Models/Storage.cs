using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
    public class Storage
    {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("maxcapacity")]
            public int? MaxCapacity { get; set; }
            [JsonPropertyName("currentstock")]
            public int? CurrentStock { get; set; }
            [JsonPropertyName("created")]
            public DateTime Created { get; set; }
            [JsonPropertyName("updated")]
            public DateTime? Updated { get; set; }

            //public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
    }
}
