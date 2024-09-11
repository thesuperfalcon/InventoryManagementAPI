using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("articleNumber")]
        public string ArticleNumber { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [JsonPropertyName("stock")]
        public int Stock { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
        [JsonPropertyName("updated")]
        public DateTime? Updated { get; set; }
    }
}
