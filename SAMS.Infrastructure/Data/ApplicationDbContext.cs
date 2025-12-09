using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAMS.Core.Entities;

namespace SAMS.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClassEnrollment> StudentClassEnrollments { get; set; }
        public DbSet<QRSession> QRSessions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<StudentClassEnrollment>()
                .HasKey(e => new { e.StudentId, e.ClassId }); // Composite Key (Wait, BaseEntity has Id, so maybe keep Id as PK and add unique index?)
            
            // Actually, BaseEntity has Id, so let's use that as PK, but ensure uniqueness
            builder.Entity<StudentClassEnrollment>()
                .HasIndex(e => new { e.StudentId, e.ClassId })
                .IsUnique();

            builder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            builder.Entity<Attendance>()
                .HasOne(a => a.QRSession)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Class>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Classes)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
