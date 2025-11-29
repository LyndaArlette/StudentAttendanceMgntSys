namespace StudentAttendanceMgntSys.Pages.Models
{
    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        
        public int TeacherId { get; set; }

        public User? Teacher { get; set; }
    }
}
