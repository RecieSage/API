namespace API.Models.DTOs
{
    /// <summary>
    /// Data transfer object for API context. Reprecents <see cref="Recipe"/> Object in a minimal way
    /// </summary>
    public class RecepieMinDTO
    {
        /// <summary>
        /// Id of the recipe
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the recipe
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
