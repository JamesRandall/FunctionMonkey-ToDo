using AzureFromTheTrenches.Commanding.Abstractions;

namespace ToDo.Commands
{
    public class MarkItemCompleteCommand : ICommand
    {
        public string UserId { get; set; }
        
        public string ItemId { get; set; }
    }
}