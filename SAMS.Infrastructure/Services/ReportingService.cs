using Microsoft.EntityFrameworkCore;
using SAMS.Core.DTOs;
using SAMS.Core.Entities;
using SAMS.Core.Services;
using SAMS.Infrastructure.Data;

namespace SAMS.Infrastructure.Services
{
    public class ReportingService : IReportingService
    {
        private readonly ApplicationDbContext _context;

        public ReportingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClassAttendanceReportDto> GetClassAttendanceReportAsync(int classId, DateTime? startDate, DateTime? endDate)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .ThenInclude(s => s.AppUser)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (classEntity == null) throw new KeyNotFoundException("Class not found");

            var query = _context.Attendances.AsQueryable();
            query = query.Where(a => a.QRSession.ClassId == classId);

            if (startDate.HasValue) query = query.Where(a => a.Timestamp >= startDate.Value);
            if (endDate.HasValue) query = query.Where(a => a.Timestamp <= endDate.Value);

            var attendances = await query.ToListAsync();
            var totalSessions = await _context.QRSessions
                .Where(s => s.ClassId == classId && (!startDate.HasValue || s.CreatedAt >= startDate) && (!endDate.HasValue || s.CreatedAt <= endDate))
                .CountAsync();

            var report = new ClassAttendanceReportDto
            {
                ClassId = classEntity.Id,
                ClassName = classEntity.ClassName,
                TotalStudents = classEntity.Enrollments.Count
            };

            foreach (var enrollment in classEntity.Enrollments)
            {
                var studentAttendances = attendances.Where(a => a.StudentId == enrollment.StudentId).ToList();
                var presentCount = studentAttendances.Count(a => a.Status == AttendanceStatus.Present);
                var absentCount = totalSessions - presentCount; // Simplified logic

                report.StudentReports.Add(new AttendanceReportDto
                {
                    StudentId = enrollment.StudentId,
                    StudentName = enrollment.Student.AppUser.FullName,
                    EnrollmentNo = enrollment.Student.EnrollmentNo,
                    TotalSessions = totalSessions,
                    PresentCount = presentCount,
                    AbsentCount = absentCount,
                    AttendancePercentage = totalSessions > 0 ? (double)presentCount / totalSessions * 100 : 0
                });
            }

            report.AverageAttendance = report.StudentReports.Any() ? report.StudentReports.Average(r => r.AttendancePercentage) : 0;

            return report;
        }

        public async Task<List<AttendanceReportDto>> GetStudentAttendanceReportAsync(int studentId)
        {
            // Simplified: Get all classes for student and calculate stats
            var enrollments = await _context.StudentClassEnrollments
                .Include(e => e.Class)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();

            var reports = new List<AttendanceReportDto>();

            foreach (var enrollment in enrollments)
            {
                var totalSessions = await _context.QRSessions.CountAsync(s => s.ClassId == enrollment.ClassId);
                var presentCount = await _context.Attendances.CountAsync(a => a.StudentId == studentId && a.QRSession.ClassId == enrollment.ClassId && a.Status == AttendanceStatus.Present);

                reports.Add(new AttendanceReportDto
                {
                    StudentId = studentId,
                    StudentName = "", // Can fetch if needed
                    EnrollmentNo = "",
                    TotalSessions = totalSessions,
                    PresentCount = presentCount,
                    AbsentCount = totalSessions - presentCount,
                    AttendancePercentage = totalSessions > 0 ? (double)presentCount / totalSessions * 100 : 0
                });
            }

            return reports;
        }
        public async Task<StudentHistoryDto> GetStudentHistoryAsync(int studentId)
        {
            // Get all classes student is enrolled in
            var enrollments = await _context.StudentClassEnrollments
                .Include(e => e.Class)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();

            var classIds = enrollments.Select(e => e.ClassId).ToList();

            // Get all sessions for these classes
            var sessions = await _context.QRSessions
                .Include(s => s.Class)
                .Where(s => classIds.Contains(s.ClassId))
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            // Get all attendance records for this student
            var attendances = await _context.Attendances
                .Where(a => a.StudentId == studentId)
                .ToListAsync();

            var history = new StudentHistoryDto();
            var records = new List<AttendanceRecordDto>();

            foreach (var session in sessions)
            {
                var attendance = attendances.FirstOrDefault(a => a.QRSessionId == session.Id);
                var status = attendance != null ? attendance.Status.ToString() : "Absent";
                
                records.Add(new AttendanceRecordDto
                {
                    Date = session.CreatedAt,
                    ClassName = session.Class.ClassName,
                    Status = status,
                    Remarks = attendance != null ? "Marked via QR" : "Missed Session"
                });

                history.TotalSessions++;
                if (attendance != null && attendance.Status == AttendanceStatus.Present) history.PresentCount++;
                else history.AbsentCount++;
            }

            history.AttendanceRecords = records;
            history.AttendancePercentage = history.TotalSessions > 0 
                ? (double)history.PresentCount / history.TotalSessions * 100 
                : 0;

            return history;
        }
    }
}
