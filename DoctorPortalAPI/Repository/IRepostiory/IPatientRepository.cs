using DoctorPortalAPI.Models;
using System.Linq.Expressions;

namespace DoctorPortalAPI.Repository.IRepostiory
{
    public interface IPatientRepository 
    {

        Task<APIResponse> UpdateAsync(Patient patient,int id);
        //Task<List<Patient>> GetAsync();
        Task<APIResponse> GetAsync();
        Task<APIResponse> GetAsync(int ID);
        Task<APIResponse> RemoveAsync(int ID);
        Task<APIResponse> CreateAsync(Patient patient);


    }
}
