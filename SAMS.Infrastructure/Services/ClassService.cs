using Microsoft.EntityFrameworkCore;
using SAMS.Core.DTOs;
using SAMS.Core.Entities;
using SAMS.Core.Services;
using SAMS.Infrastructure.Data;

namespace SAMS.Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _context;

        public ClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Class> CreateClassAsync(CreateClassDto dto)
        {
            var newClass = new Class
            {
                ClassName = dto.ClassName,
                SubjectCode = dto.SubjectCode,
                Semester = dto.Semester,
                TeacherId = dto.TeacherId
            };

            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();
            return newClass;
        }

        public async Task<IReadOnlyList<ClassDto>> GetAllClassesAsync()
        {
            return await _context.Classes
                .Include(c => c.Teacher)
                .ThenInclude(t => t.AppUser)
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    SubjectCode = c.SubjectCode,
                    Semester = c.Semester,
                    TeacherId = c.TeacherId,
                    TeacherName = c.Teacher.AppUser.FullName,
                    StudentCount = c.Enrollments.Count
                })
                .ToListAsync();
        }

        public async Task<ClassDto?> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.Teacher)
                .ThenInclude(t => t.AppUser)
                .Where(c => c.Id == id)
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    SubjectCode = c.SubjectCode,
                    Semester = c.Semester,
                    TeacherId = c.TeacherId,
                    TeacherName = c.Teacher.AppUser.FullName,
                    StudentCount = c.Enrollments.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task EnrollStudentAsync(int classId, int studentId)
        {
            var exists = await _context.StudentClassEnrollments
                .AnyAsync(e => e.ClassId == classId && e.StudentId == studentId);

            if (!exists)
            {
                var enrollment = new StudentClassEnrollment
                {
                    ClassId = classId,
                    StudentId = studentId
                };
                _context.StudentClassEnrollments.Add(enrollment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Student>> GetEnrolledStudentsAsync(int classId)
        {
            return await _context.StudentClassEnrollments
                .Where(e => e.ClassId == classId)
                .Include(e => e.Student)
                .ThenInclude(s => s.AppUser)
                .Select(e => e.Student)
                .ToListAsync();
        }
    }
}
