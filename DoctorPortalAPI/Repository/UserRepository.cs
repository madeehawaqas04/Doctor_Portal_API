using AutoMapper;
using Azure;
using DoctorPortalAPI.Data;
using DoctorPortalAPI.Models;
using DoctorPortalAPI.Models.Dto;
using DoctorPortalAPI.Repository.IRepostiory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace DoctorPortalAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);


            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            //if user was found generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
                
            };
            return loginResponseDTO;
        }

        #region RegisterAsync
        public async Task<APIResponse> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                bool ifUserNameUnique = IsUniqueUser(registerationRequestDTO.UserName);

                if (!ifUserNameUnique)
                  return new APIResponse()
                  {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message="Username already exists",
                  };

            ApplicationUser user = new()
            {
                UserName = registerationRequestDTO.UserName,
                Email=registerationRequestDTO.UserName,
                NormalizedEmail=registerationRequestDTO.UserName.ToUpper(),
                PhoneNumber = registerationRequestDTO.PhoneNumber,
                CreatedAt = DateTime.Now
            };

           
                var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if (result.Succeeded)
                {
                   
                    await _userManager.AddToRoleAsync(user, "assistant");
                    var userToReturn = _db.ApplicationUsers
                        .FirstOrDefault(u => u.UserName == registerationRequestDTO.UserName);

                    return new APIResponse()
                    {
                        Result = userToReturn,
                        StatusCode = HttpStatusCode.OK,
                        IsSuccess = true,
                    };


                }
                else
                {
                    var errorString = "User Creation failed because: ";
                    foreach (var error in result.Errors)
                    {
                        errorString += " # " + error.Description;
                    }

                    return new APIResponse()
                    {
                        Message= errorString,
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                    };
                }
            }
            catch(Exception ex)
            {
                return new APIResponse()
                {
                    Message=ex.Message,
                    ErrorMessages = new List<string>() { ex.ToString() },
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                };
            }

        }
        #endregion
    }
}
