using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using To_Do.API.Models;
using To_Do.API.Utilities;

namespace To_Do.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {

            if (request.Username == "admin" && request.Password == "password")
            {

                var token = JwtUtils.GenerateJwt(request.Username, "Admin", _configuration);
                return Ok(new { Token = token });
            }

            return Unauthorized();

        }
    }
}