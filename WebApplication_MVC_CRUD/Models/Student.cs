using System.ComponentModel.DataAnnotations;

namespace WebApplication_MVC_CRUD.Models
{
    public class Student
    {
        
            public int ID { get; set; }
        [Required]
            public string? studentName { get; set; }
        [Required]
        public string? studentGender { get; set; }
        [Required]
        public int age { get; set; }
        [Required]
        public string? standard { get; set; }
        [Required]
        public string? fatherName { get; set; }
        }

    
}
