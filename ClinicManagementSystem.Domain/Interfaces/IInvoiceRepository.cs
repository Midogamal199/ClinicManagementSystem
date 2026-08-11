using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IInvoiceRepository: IGenericRepository<Invoice>
    {
        Task<Invoice?> GetByIdWithDetailsAsync(Guid id);

        Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPagedWithDetailsAsync(
            int pageNumber,
            int pageSize,
            Guid? patientId,
            InvoiceStatus? status);
    }
}
