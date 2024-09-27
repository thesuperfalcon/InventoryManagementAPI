using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
	public class Role : IdentityRole
	{
		[JsonPropertyName("roleName")]
		public string RoleName { get; set; }

		[JsonPropertyName("fullAccess")]
		public bool FullAccess { get; set; }
	}
}
