using SAMS.Core.DTOs;

namespace SAMS.Core.Services
{
    public interface IReportingService
    {
        Task<ClassAttendanceReportDto> GetClassAttendanceReportAsync(int classId, DateTime? startDate, DateTime? endDate);
        Task<List<AttendanceReportDto>> GetStudentAttendanceReportAsync(int studentId);
        Task<StudentHistoryDto> GetStudentHistoryAsync(int studentId);
    }
}
