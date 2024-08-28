using DoctorPortalAPI.Models;
using DoctorPortalAPI.Models.Dto;

namespace DoctorPortalAPI.Repository.IRepostiory
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<APIResponse> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
