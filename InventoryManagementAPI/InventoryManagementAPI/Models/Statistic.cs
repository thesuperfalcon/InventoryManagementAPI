using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
	public class Statistic
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("userId")]
		public string? UserId { get; set; }
		[JsonPropertyName("user")]
		public virtual User? User { get; set; }
		[JsonPropertyName("initialStorageId")]
		public int? InitialStorageId { get; set; }
		[JsonPropertyName("initialStorage")]
		public virtual Storage? InitialStorage { get; set; }

		[JsonPropertyName("destinationStorageId")]
		public int? DestinationStorageId { get; set; }
        [JsonPropertyName("destinationStorage")]
        public virtual Storage? DestinationStorage { get; set; }

        [JsonPropertyName("productId")]
		public int? ProductId { get; set; }

        [JsonPropertyName("product")]
        public virtual Product? Product { get; set; }
        [JsonPropertyName("productQuantity")]
		public int ProductQuantity { get; set; }

		[JsonPropertyName("orderTime")]
		public DateTime? OrderTime { get; set; } = DateTime.Today;

	}
}
