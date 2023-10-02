namespace HealthcareManagementApp.Repositories;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    IEnumerable<T> GetAll();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
