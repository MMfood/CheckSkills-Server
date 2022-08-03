namespace CheckSkills.Web.Dtos.Student
{
    public class GetRequestStudentDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
