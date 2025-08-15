using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using To_Do.API.DTOs;
using To_Do.API.Models;

namespace To_Do.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoItem, ToDoItemDto>();       // Entity → DTO
            CreateMap<ToDoItemCreateDto, ToDoItem>(); // CreateDto → Entity
            CreateMap<ToDoItemUpdateDto, ToDoItem>(); // UpdateDto → Entity
        }
    }
}