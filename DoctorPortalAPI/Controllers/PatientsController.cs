using AutoMapper;
using DoctorPortalAPI.Models.Dto;
using DoctorPortalAPI.Models;
using DoctorPortalAPI.Repository.IRepostiory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DoctorPortalAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DoctorPortalAPI.Controllers
{
    [Route("api/Patient")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _dbPatient;
        protected APIResponse _response;
        public PatientsController(IPatientRepository dbPatient)
        {
            _dbPatient = dbPatient;
            _response = new();
        }

        // GET: api/Patients
        [HttpGet]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPatients()
        {
            var result = await _dbPatient.GetAsync();

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPatient(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var result = await _dbPatient.GetAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePatient(int id, Patient patient)
        {
           
                if (id != patient.Id || patient == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var result = await _dbPatient.UpdateAsync(patient, id);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                return NotFound(result);
                }
                else
                {
                return BadRequest(result);
                }

        }

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreatePatient(Patient patient)
        {

            if (patient == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            var result = await _dbPatient.CreateAsync(patient);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }

            // return CreatedAtAction("GetPatient", new { id = patient.Id });

        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> DeletePatient(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var result = await _dbPatient.RemoveAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }


        }

    }
}
