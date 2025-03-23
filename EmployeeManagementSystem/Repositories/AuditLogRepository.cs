using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class AuditLogRepository: IAuditLogRepository
    {
        private readonly EmployeeContext _context;

        public AuditLogRepository(EmployeeContext context)
        {
            _context = context;
        }

        // Get all audit logs
        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .Include(a => a.User) // Include User for context
                .ToListAsync();
        }
    }
}
