namespace ClinicManagementSystem.Domain.DTOs.Dashboard
{
    public class FrequentVisitorDto
    {
        public Guid PatientId { get; set; }
        public string PatientFullName { get; set; }
        public int VisitCount { get; set; }
    }
}