using System;

namespace ToDo.Models
{
    public class ToDoItem
    {
        public string Id { get; set; }
        
        public string CreatedByUserId { get; set; }
        
        public string Title { get; set; }
        
        public DateTime CreatedAtUtc { get; set; }
        
        public bool IsComplete { get; set; }
    }
}