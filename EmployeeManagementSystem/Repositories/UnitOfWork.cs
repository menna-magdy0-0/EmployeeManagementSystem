using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeContext _context;

        public IEmployeeRepository EmployeeRepository { get; }

        public UnitOfWork(EmployeeContext context, IEmployeeRepository employeeRepository)
        {
            _context = context;
            EmployeeRepository = employeeRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
