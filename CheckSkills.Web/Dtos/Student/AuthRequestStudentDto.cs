using System.ComponentModel.DataAnnotations;

namespace CheckSkills.Web.Dtos.Student
{
    public class AuthRequestStudentDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
