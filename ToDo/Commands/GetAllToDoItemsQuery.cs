using System.Collections.Generic;
using AzureFromTheTrenches.Commanding.Abstractions;
using ToDo.Models;

namespace ToDo.Commands
{
    public class GetAllToDoItemsQuery : ICommand<IReadOnlyCollection<ToDoItem>>
    {
        [SecurityProperty]
        public string UserId { get; set; }
    }
}