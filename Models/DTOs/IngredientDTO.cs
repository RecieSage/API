namespace API.Models.DTOs
{
    public class IngredientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Amount { get; set; }
        public string Unit { get; set; } = null!;
    }
}
