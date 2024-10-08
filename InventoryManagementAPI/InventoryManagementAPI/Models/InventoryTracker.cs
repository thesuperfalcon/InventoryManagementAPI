﻿using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
	public class InventoryTracker
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
		public DateTime? Modified { get; set; }
		[JsonPropertyName("product")]
		public virtual Product? Product { get; set; }

		[JsonPropertyName("storage")]
		public virtual Storage? Storage { get; set; }
	}
}
