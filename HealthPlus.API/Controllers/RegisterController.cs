using FluentValidation;
using HealthPlus.Core.IRepository;
using HealthPlus.Domain.Dtos;
using HealthPlus.Domain.Entities;
using HealthPlus.Domain.Validators;
using HealthPlus.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthPlus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository _patientInfoRepo;
        private readonly IValidator<PatientInfoDTO> _validator;
        public RegisterController(IUserRepository patientInfoRepo,IValidator<PatientInfoDTO> validator)
        {
            _patientInfoRepo = patientInfoRepo;
            _validator = validator;
        }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromForm] PatientInfoDTO patientInfo)
        {
            PatientInfoValidators validator = new PatientInfoValidators();
            var validatorResult = validator.Validate(patientInfo);

            if (!validatorResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validatorResult.Errors);
            }
            if (patientInfo == null) return BadRequest();
            await _patientInfoRepo.Register(patientInfo);
            return Ok();

        }
        [HttpPost("AuthenticationUser")]
        public async Task<IActionResult> AuthenticationUser([FromForm] AuthenticationViewModel model)
        {
            if (model == null) return BadRequest(); 
            var user=await _patientInfoRepo.Authentication(model);
            return Ok(user);    
        }
    }
}
