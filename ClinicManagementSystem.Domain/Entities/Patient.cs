using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Patient: BaseEntity
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender {get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public MedicalFile? MedicalFile { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();


    }
}
