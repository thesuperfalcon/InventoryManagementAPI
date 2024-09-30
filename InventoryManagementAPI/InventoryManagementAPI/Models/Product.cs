﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
    public class Product
    {
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("articleNumber")]
		public string? ArticleNumber { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("price")]
		[DataType(DataType.Currency)]
		public decimal? Price { get; set; }

		[JsonPropertyName("totalStock")]
		public int? TotalStock { get; set; }

		[JsonPropertyName("currentStock")]
		public int? CurrentStock { get; set; }

		[JsonPropertyName("created")]
		public DateTime? Created { get; set; }

		[JsonPropertyName("updated")]
		public DateTime? Updated { get; set; }
        [JsonPropertyName("isDeleted")]
        public bool? IsDeleted { get; set; } = false;
        [JsonPropertyName("inventoryTrackers")]
        public virtual ICollection<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();

        [JsonPropertyName("activityLog")]
        public virtual ICollection<ActivityLog> ActivityLog { get; set; } = new List<ActivityLog>();
        [JsonPropertyName("statistics")]
        public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
	}
}
