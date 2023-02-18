using System.Text.Json.Serialization;

namespace ProductsAndCategoriesLibrary
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public List<ProductFile> Files { get; set; }
    }
}
