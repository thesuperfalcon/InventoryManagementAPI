using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
	public class Statistic
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("reporterId")]
		public string? ReporterId { get; set; }

		[JsonPropertyName("executerId")]
		public string? ExecuterId { get; set; }

		[JsonPropertyName("initialStorageId")]
		public int? InitialStorageId { get; set; }

		[JsonPropertyName("destinationStorageId")]
		public int? DestinationStorageId { get; set; }

		[JsonPropertyName("productId")]
		public int? ProductId { get; set; }

		[JsonPropertyName("productQuantity")]
		public int ProductQuantity { get; set; }

		[JsonPropertyName("orderTime")]
		public DateTime OrderTime { get; set; } = DateTime.Today;

	}
}
