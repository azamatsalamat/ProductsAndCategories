using System.Text.Json.Serialization;

namespace ProductsAndCategoriesLibrary
{
    public class ProductFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StorageName { get; set; }
        public string Url { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
