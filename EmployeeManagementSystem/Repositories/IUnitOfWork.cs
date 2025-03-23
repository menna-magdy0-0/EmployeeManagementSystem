namespace EmployeeManagementSystem.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; }
        Task<int> CompleteAsync();
    }
}
