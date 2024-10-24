﻿using System.Text.Json.Serialization;

namespace InventoryManagementAPI.DTO
{
    public class RoleDTO
    {
        [JsonPropertyName("user")]
        public Models.User? User { get; set; }

        [JsonPropertyName("currentRoles")]
        public List<string?>? CurrentRoles { get; set; }

        [JsonPropertyName("addRole")]
        public string? AddRole { get; set; }

        [JsonPropertyName("resetPassword")]
        public bool ResetPassword { get; set; }
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
