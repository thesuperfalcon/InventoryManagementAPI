using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
	public class ActivityLog
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		//Krockar med Primary Keys, kolla om vi behöver lägga till productId och storageId i databasen för ActivityLog

		[JsonPropertyName("userId")]
		public string? UserId { get; set; }

		[JsonPropertyName("action")]
		public ActionType? Action { get; set; }

		[JsonPropertyName("itemType")]
		public ItemType? ItemType { get; set; }

		[JsonPropertyName("typeId")]
		public int? TypeId { get; set; }

		[JsonPropertyName("timeStamp")]
		public DateTime? TimeStamp { get; set; }

		[JsonPropertyName("notes")]
		public string? Notes { get; set; }

        
        [ForeignKey(nameof(TypeId))]
        [JsonPropertyName("product")]
        public virtual Product? Product { get; set; }


		[ForeignKey(nameof(TypeId))]
        [JsonPropertyName("storage")]
        public virtual Storage? Storage { get; set; }

        [JsonPropertyName("user")]
        public virtual User? User { get; set; }
	}

	public enum ActionType
	{
		Created,
		Updated,
		Deleted,
		Moved
	}

	public enum ItemType
	{
		Product,
		Storage
	}
}
