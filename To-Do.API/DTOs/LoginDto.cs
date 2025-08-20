using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace To_Do.API.DTOs
{
    public class LoginDto
    {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    }
}