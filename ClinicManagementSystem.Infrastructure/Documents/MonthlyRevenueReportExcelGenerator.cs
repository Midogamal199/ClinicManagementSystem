using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Reports;
using ClinicManagementSystem.Application.Interfaces;
using ClosedXML.Excel;

namespace ClinicManagementSystem.Infrastructure.Documents
{
    public class MonthlyRevenueReportExcelGenerator : IMonthlyRevenueReportGenerator
    {
        private const string MoneyFormat = "#,##0.00";

        public byte[] Generate(MonthlyRevenueReportModel model)
        {
            using var workbook = new XLWorkbook();
            BuildSummarySheet(workbook, model);
            BuildDailySheet(workbook, model);
            BuildPaymentsSheet(workbook, model);
            using var stream= new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        private void BuildPaymentsSheet(XLWorkbook workbook, MonthlyRevenueReportModel model)
        {
            var sheet = workbook.Worksheets.Add("Payments");

            sheet.Cell("A1").Value = "Paid at (UTC)";
            sheet.Cell("B1").Value = "Invoice";
            sheet.Cell("C1").Value = "Patient";
            sheet.Cell("D1").Value = "Method";
            sheet.Cell("E1").Value = "Amount (EGP)";
            sheet.Range("A1:E1").Style.Font.Bold = true;

            var row = 2;
            foreach (var payment in model.Payments)
            {
                sheet.Cell(row, 1).Value = payment.PaidAt;
                sheet.Cell(row, 1).Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
                sheet.Cell(row, 2).Value = payment.InvoiceId.ToString();
                sheet.Cell(row, 3).Value = payment.PatientFullName;
                sheet.Cell(row, 4).Value = payment.Method;
                sheet.Cell(row, 5).Value = payment.Amount;
                sheet.Cell(row, 5).Style.NumberFormat.Format = MoneyFormat;
                row++;
            }

            sheet.SheetView.FreezeRows(1);
            sheet.Columns().AdjustToContents();        
        }

        private void BuildDailySheet(XLWorkbook workbook, MonthlyRevenueReportModel model)
        {
            var sheet = workbook.Worksheets.Add("Daily");
            sheet.Cell("A1").Value = "Date";
            sheet.Cell("B1").Value = "Payments";
            sheet.Cell("C1").Value = "Amount (EGP)";
            sheet.Range("A1:C1").Style.Font.Bold = true;
            var row = 2;
            foreach (var day in model.ByDay)
            {
                sheet.Cell(row, 1).Value = day.Date;
                sheet.Cell(row, 1).Style.DateFormat.Format = "yyyy-MM-dd";
                sheet.Cell(row, 2).Value = day.PaymentsCount;
                sheet.Cell(row, 3).Value = day.Amount;
                sheet.Cell(row, 3).Style.NumberFormat.Format = MoneyFormat;
                row++;
            }

        }

        private static void  BuildSummarySheet(XLWorkbook workbook, MonthlyRevenueReportModel model)
        {
            var sheet = workbook.Worksheets.Add("Summary");
            sheet.Cell("A1").Value = "Monthly Revenue Report";
            sheet.Cell("A1").Style.Font.Bold = true;
            sheet.Cell("A1").Style.Font.FontSize = 16;

            sheet.Cell("A3").Value = "Month";
            sheet.Cell("B3").Value = $"{model.Year}-{model.Month:00}";
            sheet.Cell("A4").Value = "Total revenue (EGP)";
            sheet.Cell("B4").Value = model.TotalRevenue;
            sheet.Cell("B4").Style.NumberFormat.Format = MoneyFormat;
            sheet.Cell("A5").Value = "Number of payments";
            sheet.Cell("B5").Value = model.PaymentsCount;
            sheet.Range("A3:A5").Style.Font.Bold = true;

            sheet.Cell("A7").Value = "Payment method";
            sheet.Cell("B7").Value = "Payments";
            sheet.Cell("C7").Value = "Amount (EGP)";
            sheet.Range("A7:C7").Style.Font.Bold = true;

            var row = 8;
            foreach (var item in model.ByMethod)
            {
                sheet.Cell(row, 1).Value = item.Method;
                sheet.Cell(row, 2).Value = item.PaymentsCount;
                sheet.Cell(row, 3).Value = item.Amount;
                sheet.Cell(row, 3).Style.NumberFormat.Format = MoneyFormat;
                row++;
            }
            sheet.Columns().AdjustToContents();

        }

    }
}
