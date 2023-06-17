namespace API.Models.DTOs
{
    public class RecepieDTO
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Instructions { get; set; } = null!;

        public List<IngredientDTO> ingredients { get; set; }
    }
}
