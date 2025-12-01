using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudentAttendanceMgntSys.Pages.Models
{
    // Keep the StudentRecord class
    public class StudentRecord
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsPresent { get; set; }
    }

    // Add the StudentRecordModel PageModel class
    public class StudentRecordModel : PageModel
    {
        public List<StudentRecord> Students { get; set; } = new List<StudentRecord>();

        public void OnGet()
        {
            // Your logic here
        }
    }
}