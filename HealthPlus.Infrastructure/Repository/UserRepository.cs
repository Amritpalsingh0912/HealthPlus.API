using HealthPlus.Core.IRepository;
using HealthPlus.Domain.Dtos;
using HealthPlus.Domain.Entities;
using HealthPlus.Domain.ViewModels;
using HealthPlus.Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;



namespace HealthPlus.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSetting _appSetting;
       
        public UserRepository(ApplicationDbContext context, IOptions<AppSetting> appSetting)
        {
            _context = context;
            _appSetting = appSetting.Value;
          
        }
        public async Task<PatientInfo> Register(PatientInfoDTO patientInfoDTO)
        {
           
            if (patientInfoDTO == null)
                return null!;

            var config = new TypeAdapterConfig();
            config.NewConfig<PatientInfoDTO, PatientInfo>()
                .Ignore(dest => dest.Id) // Ignore Id mapping
                .Ignore(dest => dest.CreatedAt) // Ignore CreatedAt mapping
                .Ignore(dest => dest.Token); // Ignore Token mapping

            var patientInfo = patientInfoDTO.Adapt<PatientInfo>(config);

            var salt = HashingPass.CreateSalt(patientInfo.Email);
            var hash = HashingPass.CreateHash(patientInfo.Password, salt);
            patientInfo.Password = hash;

            await _context.PatientInfo.AddAsync(patientInfo);
            await _context.SaveChangesAsync();

            return patientInfo;
        }


        public async Task<PatientInfo> Authentication(AuthenticationViewModel model)
        {

            var patientInfo = await _context.PatientInfo.FirstOrDefaultAsync(a => a.Email == model.LoginWith || a.PhoneNumber == model.LoginWith);
            if (patientInfo == null) return null;
            var success = HashingPass.VerifyHash(model.Password, patientInfo.Email, patientInfo.Password);
            if (!success)
                return null;
           //jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, patientInfo.Id.ToString()),

                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
               SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            patientInfo.Token = tokenHandler.WriteToken(token);
             patientInfo.Password = "";
            return patientInfo;
        }
    }
}
