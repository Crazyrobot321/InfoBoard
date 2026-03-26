using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid(); //Unique identifier instead of an integer
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
