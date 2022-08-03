using System.ComponentModel.DataAnnotations;

namespace CheckSkills.Web.Dtos.Student
{
    public class UpdateStudentRequestDto
    {
        public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public int Age { get; set; }
    }
}
