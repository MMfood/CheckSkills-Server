using CheckSkills.Web.Models;

namespace CheckSkills.Web.Services.Interfaces
{
    public interface IStudentRepository : IRepositoryBase<Student>
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> GetStudentByEmailAsync(string email);
        Task<Student> GetStudentByVerificationTokenAsync(string token);
        Task<Student> GetStudentByPasswordResetTokenAsync(string token);
        void CreateStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
    }
}
