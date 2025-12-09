using Microsoft.EntityFrameworkCore;
using SAMS.Core.DTOs;
using SAMS.Core.Entities;
using SAMS.Core.Services;
using SAMS.Infrastructure.Data;

namespace SAMS.Infrastructure.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Attendance> MarkAttendanceAsync(MarkAttendanceDto dto)
        {
            // Check if already marked
            var existing = await _context.Attendances
                .AnyAsync(a => a.StudentId == dto.StudentId && a.QRSessionId == dto.QRSessionId);

            if (existing)
            {
                throw new InvalidOperationException("Attendance already marked for this session.");
            }

            // Validate Session
            var session = await _context.QRSessions.FindAsync(dto.QRSessionId);
            if (session == null || session.ExpiresAt < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Invalid or expired session.");
            }

            // Location Verification Logic (Simplified for now)
            bool locationVerified = false;
            if (session.Latitude.HasValue && session.Longitude.HasValue && dto.Latitude.HasValue && dto.Longitude.HasValue)
            {
                var distance = GetDistance(session.Latitude.Value, session.Longitude.Value, dto.Latitude.Value, dto.Longitude.Value);
                if (distance <= (session.RadiusMeters ?? 100)) // Default 100m radius
                {
                    locationVerified = true;
                }
            }
            else
            {
                // If no location constraints, assume verified or handle as policy dictates
                locationVerified = true; 
            }

            var attendance = new Attendance
            {
                StudentId = dto.StudentId,
                QRSessionId = dto.QRSessionId,
                Timestamp = DateTime.UtcNow,
                Status = AttendanceStatus.Present,
                LocationVerified = locationVerified,
                StudentLatitude = dto.Latitude,
                StudentLongitude = dto.Longitude,
                DeviceInfo = dto.DeviceInfo
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<IReadOnlyList<AttendanceDto>> GetAttendanceBySessionAsync(int sessionId)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .ThenInclude(s => s.AppUser)
                .Where(a => a.QRSessionId == sessionId)
                .Select(a => new AttendanceDto
                {
                    Id = a.Id,
                    StudentName = a.Student.AppUser.FullName,
                    EnrollmentNo = a.Student.EnrollmentNo,
                    Timestamp = a.Timestamp,
                    Status = a.Status,
                    LocationVerified = a.LocationVerified
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AttendanceDto>> GetStudentAttendanceHistoryAsync(int studentId)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .ThenInclude(s => s.AppUser)
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.Timestamp)
                .Select(a => new AttendanceDto
                {
                    Id = a.Id,
                    StudentName = a.Student.AppUser.FullName,
                    EnrollmentNo = a.Student.EnrollmentNo,
                    Timestamp = a.Timestamp,
                    Status = a.Status,
                    LocationVerified = a.LocationVerified
                })
                .ToListAsync();
        }

        // Haversine formula for distance in meters
        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371e3; // metres
            var φ1 = lat1 * Math.PI / 180;
            var φ2 = lat2 * Math.PI / 180;
            var Δφ = (lat2 - lat1) * Math.PI / 180;
            var Δλ = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }
    }
}
