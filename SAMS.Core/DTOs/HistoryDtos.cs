namespace SAMS.Core.DTOs
{
    public class StudentHistoryDto
    {
        public int TotalSessions { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public double AttendancePercentage { get; set; }
        public List<AttendanceRecordDto> AttendanceRecords { get; set; } = new();
    }

    public class AttendanceRecordDto
    {
        public DateTime Date { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}
