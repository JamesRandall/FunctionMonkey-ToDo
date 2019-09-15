using System;
using AzureFromTheTrenches.Commanding.Abstractions;
using ToDo.Models;

namespace ToDo.Commands
{
    public class AddToDoItemCommand : ICommand<ToDoItem>
    {
        [SecurityProperty]
        public string UserId { get; set; }
        
        public string Title { get; set; }
    }
}