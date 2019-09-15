using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    internal interface IToDoItemRepository
    {
        Task Upsert(ToDoItem toDoItem);

        Task<IReadOnlyCollection<ToDoItem>> Get(string userId);

        Task<ToDoItem> GetSingleItem(string itemId);
    }
}
