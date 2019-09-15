using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using ToDo.Commands;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Handlers
{
    internal class MarkItemCompleteCommandHandler : ICommandHandler<MarkItemCompleteCommand>
    {
        private readonly IToDoItemRepository _repository;

        public MarkItemCompleteCommandHandler(IToDoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(MarkItemCompleteCommand command)
        {
            ToDoItem item = await _repository.GetSingleItem(command.ItemId);
            if (item.CreatedByUserId != command.UserId)
            {
                throw new UnauthorizedItemAccessException();
            }
            item.IsComplete = true;
            await _repository.Upsert(item);
        }
    }
}