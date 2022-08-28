using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Обязательное поле")]
        public string? Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public int? TypeId { get; set; }
        public Type? Type { get; set; }
    }
}
