using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPlus.Domain.Entities
{
    public enum GenderType { Male = 0, Female = 1, Other = 2 }
    public class PatientInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public GenderType Gender { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Password { get; set; } = string.Empty;
        [NotMapped]
        public string Token { get; set; }=string.Empty;
    }
}
