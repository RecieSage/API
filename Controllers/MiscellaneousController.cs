using API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("")]
    public class MiscellaneousController : ControllerBase
    {
        /// <summary>
        /// Ping Endpoint to check if the API is online and get the version
        /// </summary>
        /// <returns>Returns an Instance of <see cref="PingDTO"/></returns>
        /// <response code="200">Returns an Instance of <see cref="PingDTO"/></response>
        [HttpGet]
        public ActionResult<PingDTO> Ping()
        {
            return this.Ok(new PingDTO()
            {
                version = this.GetType().Assembly.GetName().Version?.ToString() ?? "unset",
            });
        }
    }
}
