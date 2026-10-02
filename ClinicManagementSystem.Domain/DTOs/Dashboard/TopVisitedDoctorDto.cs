namespace ClinicManagementSystem.Domain.DTOs.Dashboard
{
    public class TopVisitedDoctorDto
    {
        public Guid DoctorId { get; set; }
        public string DoctorFullName { get; set; }
        public int VisitCount { get; set; }
    }
}