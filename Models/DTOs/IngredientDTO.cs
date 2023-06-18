namespace API.Models.DTOs
{
    /// <summary>
    /// Data transfer object for API context. Reprecents <see cref="Ingredient"/> Object
    /// </summary>
    public class IngredientDTO
    {
        /// <summary>
        /// Id of the ingredient
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the ingredient
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Amount of the ingredient
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Unit of the ingredient
        /// </summary>
        public string Unit { get; set; } = null!;
    }
}
