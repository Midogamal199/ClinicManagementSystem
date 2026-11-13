using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.DTOs.Search
{
    public class GlobalSearchResultDto
    {
        public string Term { get; set; } = string.Empty;
        public List<SearchHitDto> Patients { get; set; } = new();
        public List<SearchHitDto> Employees { get; set; } = new();
        public List<SearchHitDto> Prescriptions { get; set; } = new();
        public List<SearchHitDto> Reports { get; set; } = new();

    }
}
