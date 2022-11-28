using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class MyTask
    {
        [Key]
        public int TaskID { get; set; }
        [Required]
        [DisplayName("Task")]
        public string Task { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public string Due { get; set; }
        [Required]
        public string Completed { get; set; }   
    }
}