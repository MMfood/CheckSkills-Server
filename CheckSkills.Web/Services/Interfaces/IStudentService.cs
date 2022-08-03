namespace CheckSkills.Web.Services.Interfaces;

using CheckSkills.Web.Models;
using CheckSkills.Web.Dtos.Student;

public interface IStudentService
{
    Task<ServiceResponse<List<GetRequestStudentDto>>> GetStudents();
    Task<ServiceResponse<GetRequestStudentDto>> GetStudentById(int id);
    Task<ServiceResponse<UpdateStudentRequestDto>> UpdateStudent(UpdateStudentRequestDto request);
    Task<ServiceResponse<GetRequestStudentDto>> Register(PostRequestStudentDto request);
    Task<ServiceResponse<string>> DeleteStudent(int id);
    Task<ServiceResponse<AuthRequestStudentDto>> Login(AuthRequestStudentDto request);
    Task<ServiceResponse<string>> RefreshToken(AuthRequestStudentDto request);
    Task<ServiceResponse<VerifyAccountRequestDto>> VerifyAccount(string token);
    Task<ServiceResponse<StudentResetPasswordRequestDto>> ResetPassword(StudentResetPasswordRequestDto request);
    Task<ServiceResponse<StudentForgotPasswordRequestDto>> ForgotPassword(StudentForgotPasswordRequestDto request);
}
