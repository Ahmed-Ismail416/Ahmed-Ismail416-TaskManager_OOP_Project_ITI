using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Enums;

namespace TaskManager.Models
{
   
    public class UserTask
    {
      
        public int Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; } 
        public PriorityLevel Priority { get; set; } // High, Medium, Low
        public bool IsCompleted { get; set; }
        public CategoryType Category { get; set; }





        private int Id1;

        









    }
}
