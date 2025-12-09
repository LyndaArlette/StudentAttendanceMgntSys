namespace SAMS.Core.DTOs
{
    public class AttendanceReportDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string EnrollmentNo { get; set; } = string.Empty;
        public int TotalSessions { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public double AttendancePercentage { get; set; }
    }

    public class ClassAttendanceReportDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public double AverageAttendance { get; set; }
        public List<AttendanceReportDto> StudentReports { get; set; } = new();
    }
}
