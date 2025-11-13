namespace SmartLibraryAPI.Models
{
    /// <summary>
    /// Catalog entity - represents different categories/collections of books
    /// Demonstrates additional class for organization
    /// </summary>
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        
        // Statistics
        public int TotalBooks { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }

        public Catalog()
        {
            CreatedDate = DateTime.UtcNow;
        }
    }
}
