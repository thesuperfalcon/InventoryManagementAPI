using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
    public class User : IdentityUser
    {
		[JsonPropertyName("id")]
		public override string Id { get; set; }

		[JsonPropertyName("firstName")]
        [PersonalData]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        [PersonalData]
        public string LastName { get; set; }

        [JsonPropertyName("employeeNumber")]
        [PersonalData]
        public string EmployeeNumber { get; set; }

        public string Name => $"{FirstName} {LastName}";

        [JsonPropertyName("roleId")]
        [PersonalData]
        public string? RoleId { get; set; }

        [JsonPropertyName("created")]
        [PersonalData]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        [PersonalData]
        public DateTime Updated { get; set; }

        [JsonPropertyName("accessFailedCount")]
		[JsonIgnore]
		[PersonalData]
        public int AccessFailedCount { get; set; }

        [JsonPropertyName("emailConfirmed")]
		[JsonIgnore]
		[PersonalData]
        public bool EmailConfirmed { get; set; }


        [JsonPropertyName("phoneNumberConfirmed")]
		[JsonIgnore]
		[PersonalData]
        public bool PhoneNumberConfirmed { get; set; }


        [JsonPropertyName("twoFactorEnabled")]
		[JsonIgnore]
		[PersonalData]
        public bool TwoFactorEnabled { get; set; }


        [JsonPropertyName("lockoutEnabled")]
		[JsonIgnore]
		[PersonalData]
        public bool LockoutEnabled { get; set; }
        [JsonPropertyName("profilePic")]

        [PersonalData]
        public string? ProfilePic { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool? IsDeleted { get; set; } = false;
    }
}
