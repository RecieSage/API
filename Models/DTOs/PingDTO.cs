namespace API.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for the Ping Endpoint Response
    /// </summary>
    public class PingDTO
    {
        /// <summary>
        /// The version of the API
        /// </summary>
        public required string Version { get; set; }
    }
}
