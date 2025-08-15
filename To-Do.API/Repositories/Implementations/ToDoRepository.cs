using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using To_Do.API.Data;
using To_Do.API.DTOs;
using To_Do.API.Models;
using To_Do.API.Repositories.Interfaces;

namespace To_Do.API.Repositories.Implementations
{
    public class ToDoRepository : IToDoRepositories
    {
        private readonly AppDbContext _context;

        // Dependency Injection
        public ToDoRepository(AppDbContext context)
        {
            _context = context;
        }



        public async Task<ToDoItem> CreateItemAsync(ToDoItem toDoItem)
        {
            await _context.ToDoItems.AddAsync(toDoItem);
            await _context.SaveChangesAsync();
            return toDoItem;
        }




        public async Task<IEnumerable<ToDoItem>> GetAllAsync()
        {
            // Executes the query and materializes it into a List, but returns as IEnumerable.
            return await _context.ToDoItems.ToListAsync();
        }



        public async Task<ToDoItem?> GetByIdAsync(int Id)
        {
            return await _context.ToDoItems.FirstOrDefaultAsync(i => i.Id == Id);
        }



        public async Task<ToDoItem?> UpdateItemAsync(ToDoItem toDoItem)
        {

            var oldItem = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == toDoItem.Id);

            if (oldItem == null) return null;

            oldItem.Title = toDoItem.Title;
            oldItem.Description = toDoItem.Description;
            oldItem.IsCompleted = toDoItem.IsCompleted;

            await _context.SaveChangesAsync();
            return oldItem;
        }


        public async Task<bool> DeleteAsync(int Id)
        {
            var item = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == Id);

            if (item == null) return false;

            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}