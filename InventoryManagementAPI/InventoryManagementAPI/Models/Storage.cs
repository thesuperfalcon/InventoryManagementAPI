﻿using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
    public class Storage
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("maxCapacity")]
        public int? MaxCapacity { get; set; }

        [JsonPropertyName("currentStock")]
        public int? CurrentStock { get; set; }

        [JsonPropertyName("created")]
        public DateTime? Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime? Updated { get; set; }
        [JsonPropertyName("isDeleted")]
        public bool? IsDeleted { get; set; } = false;

        [JsonPropertyName("activityLogs")]
        public virtual ICollection<ActivityLog> ActivityLog { get; set; } = new List<ActivityLog>();

        [JsonPropertyName("inventoryTrackers")]
        public virtual ICollection<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();

        [JsonPropertyName("statisticDestinationStorages")]
        public virtual ICollection<Statistic> StatisticDestinationStorages { get; set; } = new List<Statistic>();

        [JsonPropertyName("statisticInitialStorages")]
        public virtual ICollection<Statistic> StatisticInitialStorages { get; set; } = new List<Statistic>();
    }
}
