using AutoMapper;
using CheckSkills.Web.Dtos.Student;
using CheckSkills.Web.Models;
using CheckSkills.Web.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace CheckSkills.Web.Services.StudentServices
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentService(IStudentRepository studentRepository, IRefreshTokenRepository refreshTokenRepository, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _studentRepository = studentRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<string>> DeleteStudent(int id)
        {
            ServiceResponse<string> response = new();

            var student = await _studentRepository.GetStudentByIdAsync(id);
            if(student == null)
            {
                response.Message = $"Student with id: {id} not found";
                response.Success = false;
                return response;
            }

            _studentRepository.DeleteStudent(student);
            await _studentRepository.SaveAsync();

            response.Message = $"Student with id: {id} has been deleted";

            return response;
        }

        public async Task<ServiceResponse<GetRequestStudentDto>> GetStudentById(int id)
        {
            ServiceResponse<GetRequestStudentDto> response = new();

            var student = await _studentRepository.GetStudentByIdAsync(id);

            if (student == null)
            {
                response.Message = $"Student with id: {id} not found";
                response.Success = false;
                return response;
            }

            response.Data = _mapper.Map<GetRequestStudentDto>(student);

            return response;
        }

        public async Task<ServiceResponse<List<GetRequestStudentDto>>> GetStudents()
        {
            var students = await _studentRepository.GetAllStudentsAsync();

            return new ServiceResponse<List<GetRequestStudentDto>>
            {
                Data = students.Select(s => _mapper.Map<GetRequestStudentDto>(s)).ToList()
            };

        }

        public async Task<ServiceResponse<GetRequestStudentDto>> Register(PostRequestStudentDto request)
        {
            ServiceResponse<GetRequestStudentDto> response = new();

            if (await _studentRepository.GetStudentByEmailAsync(request.Email) != null)
            {
                response.Success = false;
                response.Message = $"Student with email: {request.Email} already exist";
                return response;
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var student = _mapper.Map<Student>(request);
            student.PasswordHash = passwordHash;
            student.PasswordSalt = passwordSalt;

            var isFirstAccount = await _studentRepository.GetAllStudentsAsync();
            student.Role = (isFirstAccount.Count() == 0) ? "Admin" : "Student";

            var token = CreateToken(student);
            student.VerificationToken = token;

            _studentRepository.CreateStudent(student);

            await _studentRepository.SaveAsync();

            await SendEmailAsync(student.Email, token, "Verify Account");

            response.Data = _mapper.Map<GetRequestStudentDto>(student);

            return response;
        }

        public async Task<ServiceResponse<VerifyAccountRequestDto>> VerifyAccount(string token)
        {
            ServiceResponse<VerifyAccountRequestDto> response = new();

            var student = await _studentRepository.GetStudentByVerificationTokenAsync(token);

            if (student == null)
            {
                response.Success = false;
                return response;
            }

            student.VerifiedAt = DateTime.Now;

            _studentRepository.UpdateStudent(student);
            await _studentRepository.SaveAsync();

            response.Data = _mapper.Map<VerifyAccountRequestDto>(student);

            return response;
        }

        public async Task<ServiceResponse<AuthRequestStudentDto>> Login(AuthRequestStudentDto request)
        {
            ServiceResponse<AuthRequestStudentDto> response = new();

            var student = await _studentRepository.GetStudentByEmailAsync(request.Email);

            if (student == null || !VerifyPasswordHash(request.Password, student.PasswordHash, student.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Passswor or Login";
                return response;
            }

            if (student.VerifiedAt == null)
            {
                response.Success = false;
                response.Message = "Student is not viryfied";
                return response;
            }

            var refreshToken = await GenerateRefreshTokenAsync();
            SetRefreshTokenAsync(refreshToken, student);

            _studentRepository.UpdateStudent(student);
            await _studentRepository.SaveAsync();

            response.Data = _mapper.Map<AuthRequestStudentDto>(student);

            return response;
        }

        public async Task<ServiceResponse<StudentForgotPasswordRequestDto>> ForgotPassword(StudentForgotPasswordRequestDto request)
        {
            ServiceResponse<StudentForgotPasswordRequestDto> response = new();

            var student = await _studentRepository.GetStudentByEmailAsync(request.Email);

            if (student == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            student.PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            student.ResetTokenExpires = DateTime.Now.AddDays(1);

            await SendEmailAsync(request.Email, $"You can reset password <p>{student.PasswordResetToken}<p>", "Forgot Password");

            _studentRepository.UpdateStudent(student);
            await _studentRepository.SaveAsync();

            return response;
        }

        public async Task<ServiceResponse<StudentResetPasswordRequestDto>> ResetPassword(StudentResetPasswordRequestDto request)
        {
            ServiceResponse<StudentResetPasswordRequestDto> response = new();

            var student = await _studentRepository.GetStudentByPasswordResetTokenAsync(request.Token);

            if (student == null || student.ResetTokenExpires < DateTime.Now)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            student.PasswordHash = passwordHash;
            student.PasswordSalt = passwordSalt;
            student.PasswordResetToken = null;
            student.ResetTokenExpires = null;

            _studentRepository.UpdateStudent(student);
            await _studentRepository.SaveAsync();

            return response;
        }

        public async Task<ServiceResponse<string>> RefreshToken(AuthRequestStudentDto request)
        {
            ServiceResponse<string> response = new();

            if (_httpContextAccessor.HttpContext == null)
            {
                response.Success = false;
                response.Message = "http context is null";
                return response;
            }

            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            var student = await _studentRepository.GetStudentByEmailAsync(request.Email);

            if (student == null || !student.RefreshToken.Equals(refreshToken) || student.TokenExpires < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Ivalid Token";
                return response;
            }

            var token = CreateToken(student);

            var newRefreshToken = await GenerateRefreshTokenAsync();
            SetRefreshTokenAsync(newRefreshToken, student);

            response.Data = token;

            return response;
        }

        public async Task<ServiceResponse<UpdateStudentRequestDto>> UpdateStudent(UpdateStudentRequestDto request)
        {
            ServiceResponse<UpdateStudentRequestDto> response = new();

            var student = await _studentRepository.GetStudentByIdAsync(request.Id);

            if (student == null)
            {
                response.Success = false;
                response.Message = $"Student with id: {request.Id} not found";
                return response;
            }

            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.Age = request.Age;

            if(!student.Email.Equals(request.Email))
            {
                student.Email = request.Email;
                student.VerificationToken = CreateToken(student);
                student.VerifiedAt = null;
                await SendEmailAsync(student.Email, student.VerificationToken, "VerifyAccount");
            }

            _studentRepository.UpdateStudent(student);
            await _studentRepository.SaveAsync();

            response.Data = _mapper.Map<UpdateStudentRequestDto>(student);

            return response;
        }

        public async Task SendEmailAsync(string userEmail, string message, string subject)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }

        private async Task<RefreshToken> GenerateRefreshTokenAsync()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            _refreshTokenRepository.CreateRefreshToken(refreshToken);
            await _refreshTokenRepository.SaveAsync();

            return refreshToken;
        }

        private void SetRefreshTokenAsync(RefreshToken newrefreshToken, Student student)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newrefreshToken.Expires
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newrefreshToken.Token, cookieOptions);

            student.RefreshToken = newrefreshToken.Token;
            student.VerifiedAt = newrefreshToken.Created;
            student.TokenExpires = newrefreshToken.Expires;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password,  byte[] passwordHash,  byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(Student student)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, student.FirstName),
                new Claim(ClaimTypes.Role, student.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes
                (_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
