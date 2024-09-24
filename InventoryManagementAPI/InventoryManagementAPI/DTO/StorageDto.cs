namespace InventoryManagementAPI.DTO
{
    public class StorageDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }   
        public int? MaxCapacity { get; set; }
        public int? CurrentStock { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
