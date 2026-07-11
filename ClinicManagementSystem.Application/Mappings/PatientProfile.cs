using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Patients;
using ClinicManagementSystem.Application.Features.Patients.Commands.CreatePatient;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Mappings
{
  public class PatientProfile:Profile
    {
        public PatientProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<CreatePatientCommand, Patient>();
        }
    }
}
