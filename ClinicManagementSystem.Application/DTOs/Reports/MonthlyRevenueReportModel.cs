using System;
using System.Collections.Generic;

namespace ClinicManagementSystem.Application.DTOs.Reports
{
    public class MonthlyRevenueReportModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PaymentsCount { get; set; }
        public List<RevenueByMethodItem> ByMethod { get; set; } = new();
        public List<RevenueByDayItem> ByDay { get; set; } = new();
        public List<RevenuePaymentRowDto> Payments { get; set; } = new();
    }

    public class RevenueByMethodItem
    {
        public string Method { get; set; } = string.Empty;
        public int PaymentsCount { get; set; }
        public decimal Amount { get; set; }
    }

    public class RevenueByDayItem
    {
        public DateTime Date { get; set; }
        public int PaymentsCount { get; set; }
        public decimal Amount { get; set; }
    }
}