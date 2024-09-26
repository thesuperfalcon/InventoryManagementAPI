using InventoryManagementAPI.Models;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.DTO
{
    public class StatisticDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("initialStorageId")]
        public int? InitialStorageId { get; set; }

        [JsonPropertyName("destinationStorageId")]
        public int? DestinationStorageId { get; set; }

        [JsonPropertyName("productId")]
        public int? ProductId { get; set; }

        [JsonPropertyName("productQuantity")]
        public int ProductQuantity { get; set; }

        [JsonPropertyName("orderTime")]
        public DateTime? OrderTime { get; set; } = DateTime.Today;
    }
}
