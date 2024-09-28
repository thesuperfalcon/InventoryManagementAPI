using System.Text.Json.Serialization;

namespace InventoryManagementAPI.DTO
{
    public class RemoveRolesDTO
    {
        [JsonPropertyName("user")]
        public Models.User? User { get; set; }
        [JsonPropertyName("currentRoles")]
        public List<string?>? CurrentRoles { get; set; }
        [JsonPropertyName("removeRole")]
        public bool RemoveRole { get; set; }
    }
}
