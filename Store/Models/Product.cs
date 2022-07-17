using Microsoft.AspNetCore.Mvc.Rendering;

namespace Store.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public int? TypeId { get; set; }
        public Type? Type { get; set; }
    }
}
