using System.Text.Json.Serialization;
#nullable enable

namespace InventoryManagementAPI.Models
{
	public record InventoryTracker
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("storageId")]
		public int? StorageId { get; set; }

		[JsonPropertyName("productId")]
		public int? ProductId { get; set; }

		[JsonPropertyName("quantity")]
		public int? Quantity { get; set; }

		[JsonPropertyName("modified")]
		public DateTime? Modified { get; set; } = DateTime.Now;

		[JsonPropertyName("product")]
		public virtual Product? Product { get; set; }

		[JsonPropertyName("storage")]
		public virtual Storage? Storage { get; set; }
	}
}
