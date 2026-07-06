using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Specialty: BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
