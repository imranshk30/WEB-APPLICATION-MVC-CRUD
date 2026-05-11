using System.ComponentModel.DataAnnotations;

namespace WebApplication_MVC_CRUD.Models
{
    public class Student
    {
        
            public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? studentName { get; set; }
        [Required]
        [StringLength(10)]
        public string? studentGender { get; set; }
        [Required]
        [Range(18,60)]
        public int age { get; set; }
        [Required]
        [StringLength(2)]
        public string? standard { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? fatherName { get; set; }
        }

    
}
