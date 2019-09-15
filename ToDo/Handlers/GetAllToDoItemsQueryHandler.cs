using System.Collections.Generic;
using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using ToDo.Commands;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Handlers
{
    internal class GetAllToDoItemsQueryHandler : ICommandHandler<GetAllToDoItemsQuery, IReadOnlyCollection<ToDoItem>>
    {
        private readonly IToDoItemRepository _repository;

        public GetAllToDoItemsQueryHandler(IToDoItemRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IReadOnlyCollection<ToDoItem>> ExecuteAsync(
            GetAllToDoItemsQuery command,
            IReadOnlyCollection<ToDoItem> previousResult)
        {
            return await _repository.Get(command.UserId);
        }
    }
}