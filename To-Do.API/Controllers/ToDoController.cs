using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using To_Do.API.Data;
using To_Do.API.DTOs;
using To_Do.API.Models;
using To_Do.API.Repositories.Implementations;
using To_Do.API.Repositories.Interfaces;

namespace To_Do.API.Controllers
{
    // https://localhost:7126/api/todo
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly IToDoRepositories _repository;
        private readonly IMapper _mapper;

        public ToDoController(ILogger<ToDoController> logger, IToDoRepositories repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }



        // Get All Items
        // GET: https://localhost:7126/api/todo
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var Items = await _repository.GetAllAsync();

            var ItemsDto = _mapper.Map<List<ToDoItemDto>>(Items);

            return Ok(ItemsDto);
        }



        // Get a single Item by id
        // GET: https://localhost:7126/api/todo{id}
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {

            var Item = await _repository.GetByIdAsync(Id);

            if (Item == null)
            {
                _logger.LogWarning($"ToDo Item  with ID {Id} was not found.");
                return NotFound($"Item with ID {Id} does not exist.");

            }

            var ItemDto = _mapper.Map<ToDoItemDto>(Item);

            _logger.LogInformation($"ToDo Item with ID {Id} was retrieved successfully", Id);

            return Ok(ItemDto);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateItem([FromBody] ToDoItemCreateDto createDto)
        {

            var newItem = _mapper.Map<ToDoItem>(createDto);
            newItem.IsCompleted = false;
            newItem.CreatedAt = DateTime.UtcNow;

            var itemDM = await _repository.CreateItemAsync(newItem);

            var ItemDto = _mapper.Map<ToDoItemDto>(newItem);
            _logger.LogInformation("Created new ToDo Item with ID {Id}", itemDM.Id);

            return CreatedAtAction(nameof(GetById), new { id = itemDM.Id }, ItemDto);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult?> UpdateItem(int id, [FromBody] ToDoItemUpdateDto updateDto)
        {


            var existingItem = await _repository.GetByIdAsync(id);

            if (existingItem == null)
            {
                _logger.LogWarning("ToDo Item with ID {Id} not found for update.", id);
                return NotFound($"Item with ID {id} does not exist.");
            }

            _mapper.Map(updateDto, existingItem);

            var updatedItem = await _repository.UpdateItemAsync(existingItem);

            _logger.LogInformation("Updated ToDo Item with ID {Id}.", id);

            var ItemDto = _mapper.Map<ToDoItemDto>(updatedItem);

            return Ok(ItemDto);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {

            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("ToDo Item with ID {Id} not found for deletion.", id);
                return NotFound($"Item with ID {id} does not exist.");
            }

            _logger.LogInformation("Deleted ToDo Item with ID {Id}.", id);
            return NoContent();
        }
    }
}