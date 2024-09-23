using System.Text.Json.Serialization;

namespace InventoryManagementAPI.DTO
{
    public class InventoryTrackerDto
    {
        public int Id { get; set; }

        public int? StorageId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }
    }
}
