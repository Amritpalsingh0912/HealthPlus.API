using HealthPlus.Domain.Dtos;
using HealthPlus.Domain.Entities;
using HealthPlus.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPlus.Core.IRepository
{
    public interface IUserRepository
    {
        Task<PatientInfo>Register(PatientInfoDTO patientInfo);
        Task<PatientInfo>Authentication(AuthenticationViewModel model);
    }
}
