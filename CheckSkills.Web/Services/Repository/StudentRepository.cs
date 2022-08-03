using CheckSkills.Web.Models;
using CheckSkills.Web.Services.Interfaces;

namespace CheckSkills.Web.Services.Repository
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(DataContext context) : base(context) { }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await FindAll()
               .OrderBy(st => st.LastName)
               .ToListAsync();
        }
        public async Task<Student> GetStudentByIdAsync(int Id)
        {
            return await FindByCondition(student => student.Id.Equals(Id))
                .FirstOrDefaultAsync();
        }

        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await FindByCondition(student => student.Email.Equals(email))
                    .FirstOrDefaultAsync();
        }

        public async Task<Student> GetStudentByVerificationTokenAsync(string token)
        {
            return await FindByCondition(student => student.VerificationToken.Equals(token))
                    .FirstOrDefaultAsync();
        }

        public async Task<Student> GetStudentByPasswordResetTokenAsync(string token)
        {
            return await FindByCondition(student => student.PasswordResetToken.Equals(token))
                   .FirstOrDefaultAsync();
        }

        public void CreateStudent(Student student)
        {
            Create(student);
        }
        public void UpdateStudent(Student student)
        {
            Update(student);
        }
        public void DeleteStudent(Student student)
        {
            Delete(student);
        }
    }
}
