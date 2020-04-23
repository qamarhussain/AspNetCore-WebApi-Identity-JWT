using System.Collections.Generic;
using AspNetCoreApiJwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = MicrosoftVSConstants.AuthenticationSceme)]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public List<string> GetValues()
        {
            return new List<string>() { "value 1", "value 2", "value 3", "value 4" };
        }
    }
}