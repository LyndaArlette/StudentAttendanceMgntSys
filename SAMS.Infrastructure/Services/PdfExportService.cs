using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using SAMS.Core.DTOs;
using SAMS.Core.Services;

namespace SAMS.Infrastructure.Services
{
    public class PdfExportService : IPdfExportService
    {
        public byte[] GenerateClassReportPdf(ClassAttendanceReportDto report, DateTime? startDate, DateTime? endDate)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Title
                var titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                document.Add(new Paragraph(new Text("Class Attendance Report").SetFont(titleFont))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                // Class Info
                document.Add(new Paragraph($"Class: {report.ClassName}")
                    .SetFontSize(14));
                
                string dateRange = "All Time";
                if (startDate.HasValue && endDate.HasValue)
                    dateRange = $"{startDate.Value:d} - {endDate.Value:d}";
                else if (startDate.HasValue)
                    dateRange = $"From {startDate.Value:d}";
                else if (endDate.HasValue)
                    dateRange = $"Until {endDate.Value:d}";

                document.Add(new Paragraph($"Date Range: {dateRange}"));
                document.Add(new Paragraph($"Total Students: {report.TotalStudents}"));
                document.Add(new Paragraph($"Average Attendance: {report.AverageAttendance:F1}%"));
                document.Add(new Paragraph("\n"));

                // Table
                var table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 2, 1, 1, 1 }));
                table.SetWidth(UnitValue.CreatePercentValue(100));

                // Header
                var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                table.AddHeaderCell(new Cell().Add(new Paragraph(new Text("Student Name").SetFont(boldFont))));
                table.AddHeaderCell(new Cell().Add(new Paragraph(new Text("Enrollment No").SetFont(boldFont))));
                table.AddHeaderCell(new Cell().Add(new Paragraph(new Text("Present").SetFont(boldFont))));
                table.AddHeaderCell(new Cell().Add(new Paragraph(new Text("Absent").SetFont(boldFont))));
                table.AddHeaderCell(new Cell().Add(new Paragraph(new Text("%").SetFont(boldFont))));

                // Rows
                foreach (var student in report.StudentReports)
                {
                    table.AddCell(student.StudentName);
                    table.AddCell(student.EnrollmentNo);
                    table.AddCell(student.PresentCount.ToString());
                    table.AddCell(student.AbsentCount.ToString());
                    table.AddCell($"{student.AttendancePercentage:F1}%");
                }

                document.Add(table);
                document.Close();

                return stream.ToArray();
            }
        }
    }
}
