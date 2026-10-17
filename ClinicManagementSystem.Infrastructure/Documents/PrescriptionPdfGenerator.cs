using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Prescriptions;
using ClinicManagementSystem.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace ClinicManagementSystem.Infrastructure.Documents
{
    public class PrescriptionPdfGenerator : IPrescriptionPdfGenerator
    {
        public byte[] Generate(PrescriptionPdfModel model)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(11));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Clinic Management System").FontSize(18).Bold();
                        col.Item().Text("Medical Prescription").FontSize(12).FontColor(Colors.Grey.Darken2);
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });

                    page.Content().PaddingVertical(15).Column(col =>
                    {
                        col.Spacing(8);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text(t =>
                            {
                                t.Span("Patient: ").SemiBold();
                                t.Span(model.PatientFullName);
                            });
                            row.RelativeItem().AlignRight().Text(t =>
                            {
                                t.Span("Date: ").SemiBold();
                                t.Span($"{model.IssuedAt:yyyy-MM-dd}");
                            });
                        });

                        col.Item().Text(t =>
                        {
                            t.Span("Doctor: ").SemiBold();
                            t.Span($"{model.DoctorFullName} (License: {model.DoctorLicenseNumber})");
                        });

                        col.Item().PaddingTop(10).Text("Rx").FontSize(16).Bold();

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(25);
                                c.RelativeColumn(2);
                                c.RelativeColumn(2);
                                c.RelativeColumn(1);

                            });

                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(1).PaddingVertical(4).Text("#").SemiBold();
                                header.Cell().BorderBottom(1).PaddingVertical(4).Text("Medicine").SemiBold();
                                header.Cell().BorderBottom(1).PaddingVertical(4).Text("Dosage").SemiBold();
                                header.Cell().BorderBottom(1).PaddingVertical(4).Text("Duration").SemiBold();

                            });

                            for (var i = 0; i < model.Items.Count; i++)
                            {
                                var item = model.Items[i];
                                table.Cell().BorderBottom(0.5f).PaddingVertical(4).Text($"{i + 1}");
                                table.Cell().BorderBottom(0.5f).PaddingVertical(4).Text(item.MedicineName);
                                table.Cell().BorderBottom(0.5f).PaddingVertical(4).Text(item.Dosage);
                                table.Cell().BorderBottom(0.5f).PaddingVertical(4)
                                .Text(item.DurationInDays is null ? "-" : $"{item.DurationInDays} day(s)");
                            }
                        });

                        col.Item().PaddingTop(30).AlignRight().Text("Doctor signature: ______________");
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Page ");
                        t.CurrentPageNumber();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
