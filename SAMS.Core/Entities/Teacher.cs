using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Core.Entities
{
    public class Teacher : BaseEntity
    {
        [Required]
        public string AppUserId { get; set; } = string.Empty;

        [ForeignKey("AppUserId")]
        public AppUser? AppUser { get; set; }

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Designation { get; set; }

        // Navigation properties
        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
