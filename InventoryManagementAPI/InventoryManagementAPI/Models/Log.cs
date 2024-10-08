﻿using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
    public class Log
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }
        [JsonPropertyName("action")]
        public string? Action { get; set; }
        [JsonPropertyName("entityType")]
        public string? EntityType { get; set; }
        [JsonPropertyName("entityName")]
        public string? EntityName { get; set; }
        [JsonPropertyName("entityDetails")]
        public string? EntityDetails { get; set; }
        [JsonPropertyName("timeStamp")]
        public DateTime TimeStamp { get; set; }
    }
}
