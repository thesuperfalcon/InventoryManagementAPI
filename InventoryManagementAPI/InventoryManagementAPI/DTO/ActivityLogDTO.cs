using InventoryManagementAPI.Models;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.DTO
{
    public class ActivityLogDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        //Krockar med Primary Keys, kolla om vi behöver lägga till productId och storageId i databasen för ActivityLog

        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("action")]
        public ActionTypeDTO? Action { get; set; }

        [JsonPropertyName("itemType")]
        public ItemTypeDTO? ItemType { get; set; }

        [JsonPropertyName("typeId")]
        public int? TypeId { get; set; }

        [JsonPropertyName("timeStamp")]
        public DateTime? TimeStamp { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }
    public enum ActionTypeDTO
    {
        Created,
        Updated,
        Deleted,
        Moved
    }

    public enum ItemTypeDTO
    {
        Product,
        Storage
    }
}
