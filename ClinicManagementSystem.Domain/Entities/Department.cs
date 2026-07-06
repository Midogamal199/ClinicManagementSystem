using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Department:BaseEntity
    {
     public string Name { get; set; }
     public ICollection<Employee> Employees { get; set; }= new List<Employee>();


    }
}
