using Azure;
using DoctorPortalAPI.Data;
using DoctorPortalAPI.Models;
using DoctorPortalAPI.Models.Dto;
using DoctorPortalAPI.Repository.IRepostiory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DoctorPortalAPI.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _db;
        public PatientRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<APIResponse> GetAsync()
        {
            try
            {
                var patient =await _db.Patients.ToListAsync();
                return new APIResponse()
                {
                    Result = patient,
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                };

            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    Message = ex.Message,
                    ErrorMessages = new List<string>() { ex.ToString() },
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                };
            }

        }

		public bool PatientExists(int id)
		{
			try
			{
				var PatientExists = _db.Patients.Any(e => e.Id == id);
                return PatientExists;
			}
			catch (Exception ex)
			{
                return false;
			}

		}

		public async Task<APIResponse> GetAsync(int id)
        {
            try
            {
                var Patient =await _db.Patients.FindAsync(id);


				if (Patient == null)
                {
                    return new APIResponse()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Patient not found"
                    };
                }
                return new APIResponse()
                {
                    Result = Patient,
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                };
            }

        }

        public async Task<APIResponse> UpdateAsync(Patient patient,int id)
        {
            try
            {
                patient.Id = id;
                patient.UpdatedDate = DateTime.Now;
                _db.Patients.Update(patient);
                await _db.SaveChangesAsync();


                return new APIResponse()
                {
                    Result = patient,
                    Message = "Patient updated successfully",
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                var pExists = PatientExists(id);

                if (!pExists)
                {
                    return new APIResponse()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Patient not found",
                    };
                }
                return new APIResponse()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                };
            }

        }


        public async Task<APIResponse> RemoveAsync(int id)
        {
            try
            {
                var PatientExists = await GetAsync(id);
                
                if (PatientExists.Result == null)
                    return new APIResponse()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Patient not found",
                    };

                _db.Remove(PatientExists.Result);
                await _db.SaveChangesAsync();
                
                return new APIResponse()
                {
                    Result = PatientExists.Result,
                    Message = "Patient deleted successfully",
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                };
            }

        }

        public bool IsUniquePatient(string fname,string lname,string phonno)
        {
            var patient = _db.Patients.FirstOrDefault(x => x.FirstName == fname && x.LastName == lname && x.PhoneNumber == phonno);
            if (patient == null)
            {
                return true;
            }
            return false;
        }

        public async Task<APIResponse> CreateAsync(Patient patient)
        {
            try
            {
                bool ifPatientNameUnique = IsUniquePatient(patient.FirstName,patient.LastName,patient.PhoneNumber);
                if (!ifPatientNameUnique)
                    return new APIResponse()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Patient already exists with same name and phone no",
                    };

                _db.Patients.Add(patient);
                await _db.SaveChangesAsync();

                return new APIResponse()
                {
                    Result = patient,
                    Message = "Patient created successfully",
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                };
            }

        }

    }
}