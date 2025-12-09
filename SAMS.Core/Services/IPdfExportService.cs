using SAMS.Core.DTOs;

namespace SAMS.Core.Services
{
    public interface IPdfExportService
    {
        byte[] GenerateClassReportPdf(ClassAttendanceReportDto report, DateTime? startDate, DateTime? endDate);
    }
}
