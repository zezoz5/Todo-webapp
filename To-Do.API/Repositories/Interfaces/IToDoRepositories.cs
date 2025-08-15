using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using To_Do.API.Data;
using To_Do.API.DTOs;
using To_Do.API.Models;

namespace To_Do.API.Repositories.Interfaces
{
    public interface IToDoRepositories
    {
        Task<IEnumerable<ToDoItem>> GetAllAsync();
        Task<ToDoItem?> GetByIdAsync(int Id);
        Task<ToDoItem> CreateItemAsync(ToDoItem toDoItem);
        Task<ToDoItem?> UpdateItemAsync(ToDoItem toDoItem);
        Task<bool> DeleteAsync(int Id);
    }
}