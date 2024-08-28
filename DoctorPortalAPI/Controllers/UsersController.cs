using DoctorPortalAPI.Models;
using DoctorPortalAPI.Models.Dto;
using DoctorPortalAPI.Repository.IRepostiory;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DoctorPortalAPI.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        protected APIResponse _response;
        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _response = new();
        }

        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepo.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterationRequestDTO model)
        {
            var registerResult = await _userRepo.Register(model);

            if (registerResult.IsSuccess)
            {
                return Ok(registerResult);
            }
            else 
            {
                return BadRequest(registerResult);
            }
        }
    }
}
