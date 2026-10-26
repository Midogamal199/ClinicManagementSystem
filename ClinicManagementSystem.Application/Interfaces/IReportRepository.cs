using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Reports;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IReportRepository
    {
        Task<List<RevenuePaymentRowDto>> GetPaymentsInRangeAsync(DateTime fromInclusive, DateTime toExclusive);
    }
}