using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace To_Do.API.DTOs
{
    public class ToDoItemCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}