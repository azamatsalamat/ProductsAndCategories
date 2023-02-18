using System.Text.Json.Serialization;

namespace ProductsAndCategoriesLibrary
{
    public class ProductProperty
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        [JsonIgnore]
        public Property Property { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public string Value { get; set; }
    }
}
