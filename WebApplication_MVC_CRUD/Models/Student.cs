using System.ComponentModel.DataAnnotations;

namespace WebApplication_MVC_CRUD.Models
{
    public class Student
    {
        
            public int ID { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        [StringLength(50, MinimumLength = 3)]
        public string? studentName { get; set; }
        [Required]
        [StringLength(10)]
        [RegularExpression(@"^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string? studentGender { get; set; }
        [Required]
        [Range(18,60)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Age must be a valid number.")]
        public int age { get; set; }
        [Required]
        [StringLength(2)]
        [RegularExpression(@"^([1-9]|1[0-9]|2[0-5])$", ErrorMessage = "Standard must be a number between 1 and 25.")]
        public string? standard { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
         [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string? fatherName { get; set; }
        }

    
}
