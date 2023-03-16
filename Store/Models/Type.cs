namespace TodoList.Models
{
    public class Type
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
