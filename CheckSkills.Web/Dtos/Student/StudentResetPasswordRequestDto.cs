using System.ComponentModel.DataAnnotations;

namespace CheckSkills.Web.Dtos.Student
{
    public class StudentResetPasswordRequestDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required, MaxLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
