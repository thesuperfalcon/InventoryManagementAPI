using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
	public class Role : IdentityRole
	{
        [JsonPropertyName("id")]
        public override string Id { get; set; }

		[JsonPropertyName("roleName")]
		public string RoleName { get; set; }

		[JsonPropertyName("fullAccess")]
		public bool FullAccess { get; set; }
	}
}
