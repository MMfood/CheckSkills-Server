using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckSkills.Web.Data;
using CheckSkills.Web.Models;
using CheckSkills.Web.Services.Interfaces;
using CheckSkills.Web.Dtos.Student;
using Microsoft.AspNetCore.Authorization;

namespace CheckSkills.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(AuthRequestStudentDto request)
        {
            var response = await _studentService.Login(request);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ServiceResponse<string>>> RefrefhToken(AuthRequestStudentDto request)
        {
            var response = await _studentService.RefreshToken(request);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetRequestStudentDto>>>> GetStudentsList()
        {
            return Ok(await _studentService.GetStudents());
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetRequestStudentDto>>> GetStudent(int id)
        {
            var response = await _studentService.GetStudentById(id);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<UpdateStudentRequestDto>>> UpdateStudent(UpdateStudentRequestDto request)
        {
            var response = await _studentService.UpdateStudent(request);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetRequestStudentDto>>> Register(PostRequestStudentDto request)
        {
            var response = await _studentService.Register(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<StudentForgotPasswordRequestDto>>> ForgotPassword(StudentForgotPasswordRequestDto request)
        {
            var response = await _studentService.ForgotPassword(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<StudentResetPasswordRequestDto>>> ResetPassword(StudentResetPasswordRequestDto request)
        {
            var response = await _studentService.ResetPassword(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> VerifyAccount(string token)
        {
            var response = await _studentService.VerifyAccount(token);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetRequestStudentDto>>> DeleteStudent(int id)
        {
            var response = await _studentService.DeleteStudent(id);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

    }
}
