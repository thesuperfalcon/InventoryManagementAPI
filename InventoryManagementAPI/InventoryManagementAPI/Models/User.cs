using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
    [Table("AspNetUsers")]
    public class User : IdentityUser
    {
        [JsonPropertyName("id")]
        [PersonalData]
        public string Id { get; set; }

        [JsonPropertyName("firstName")]
        [PersonalData]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        [PersonalData]
        public string LastName { get; set; }


        [JsonPropertyName("userName")]
        [PersonalData]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        [PersonalData]
        public string PasswordHash { get; set; }

        [JsonPropertyName("employeeNumber")]
        [PersonalData]
        public string EmployeeNumber { get; set; }

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
        [PersonalData]
        public int AccessFailedCount { get; set; }

        [JsonPropertyName("emailConfirmed")]
        [PersonalData]
        public bool EmailConfirmed { get; set; }


        [JsonPropertyName("phoneNumberConfirmed")]
        [PersonalData]
        public bool PhoneNumberConfirmed { get; set; }


        [JsonPropertyName("twoFactorEnabled")]
        [PersonalData]
        public bool TwoFactorEnabled { get; set; }


        [JsonPropertyName("lockoutEnabled")]
        [PersonalData]
        public bool LockoutEnabled { get; set; }
    }
}
