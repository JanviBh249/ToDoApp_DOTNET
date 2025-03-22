using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
   
        public class ToDoItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public bool IsCompleted { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Priority { get; set; }
            public DateTime DueDate { get; set; } // Add this property
            public string Email { get; set; }
    }


    }


