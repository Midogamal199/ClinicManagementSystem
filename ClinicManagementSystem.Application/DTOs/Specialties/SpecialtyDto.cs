using System;

namespace ClinicManagementSystem.Application.DTOs.Specialties
{
    public class SpecialtyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DoctorCount { get; set; }
    }
}