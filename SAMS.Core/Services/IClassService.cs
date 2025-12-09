using SAMS.Core.DTOs;
using SAMS.Core.Entities;

namespace SAMS.Core.Services
{
    public interface IClassService
    {
        Task<Class> CreateClassAsync(CreateClassDto dto);
        Task<IReadOnlyList<ClassDto>> GetAllClassesAsync();
        Task<ClassDto?> GetClassByIdAsync(int id);
        Task EnrollStudentAsync(int classId, int studentId);
        Task<IReadOnlyList<Student>> GetEnrolledStudentsAsync(int classId);
    }
}
