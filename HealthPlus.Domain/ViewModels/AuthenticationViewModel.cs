using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPlus.Domain.ViewModels
{
    public class AuthenticationViewModel
    {
      
        public string LoginWith { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty;    
    }
}
