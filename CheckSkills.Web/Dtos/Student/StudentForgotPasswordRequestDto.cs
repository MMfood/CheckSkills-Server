using System.ComponentModel.DataAnnotations;

namespace CheckSkills.Web.Dtos.Student
{
    public class StudentForgotPasswordRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
