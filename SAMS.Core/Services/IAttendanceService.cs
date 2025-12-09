using SAMS.Core.DTOs;
using SAMS.Core.Entities;

namespace SAMS.Core.Services
{
    public interface IAttendanceService
    {
        Task<Attendance> MarkAttendanceAsync(MarkAttendanceDto dto);
        Task<IReadOnlyList<AttendanceDto>> GetAttendanceBySessionAsync(int sessionId);
        Task<IReadOnlyList<AttendanceDto>> GetStudentAttendanceHistoryAsync(int studentId);
    }
}
