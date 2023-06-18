namespace API.Models.DTOs
{
    /// <summary>
    /// Data transfer object for API context. Reprecents <see cref="Recipe"/> Object
    /// </summary>
    public class RecepieDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecepieDTO"/> class.
        /// </summary>
        public RecepieDTO()
        {
            this.Ingredients = new List<IngredientDTO>();
        }

        /// <summary>
        /// Id of the recipe
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the recipe
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Instructions of the recipe
        /// </summary>
        public string Instructions { get; set; } = null!;

        /// <summary>
        /// List of ingredients in the recipe
        /// </summary>
        public List<IngredientDTO> Ingredients { get; set; }
    }
}
