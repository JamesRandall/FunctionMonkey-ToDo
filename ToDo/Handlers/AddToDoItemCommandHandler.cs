using System;
using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using ToDo.Commands;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Handlers
{
    internal class AddToDoItemCommandHandler : ICommandHandler<AddToDoItemCommand, ToDoItem>
    {
        private readonly IToDoItemRepository _repository;

        public AddToDoItemCommandHandler(IToDoItemRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<ToDoItem> ExecuteAsync(AddToDoItemCommand command, ToDoItem previousResult)
        {
            ToDoItem newItem = new ToDoItem
            {
                CreatedAtUtc = DateTime.UtcNow,
                CreatedByUserId = command.UserId,
                Id = Guid.NewGuid().ToString(),
                IsComplete = false,
                Title = command.Title
            };
            await _repository.Upsert(newItem);
            return newItem;
        }
    }
}