using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPlus.Domain.ViewModels
{
    public class Verification
    {
        public string PhoneNumber { get; set; }=string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
