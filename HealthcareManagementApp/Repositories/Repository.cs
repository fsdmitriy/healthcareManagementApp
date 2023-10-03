using HealthcareManagementApp.Data;
using HealthcareManagementApp.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementApp.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _entities;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _entities = _context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public Task<List<T>> GetAllAsync()
    {
        return _entities.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id) ?? throw new RepositoryException($"Cannot fid the item with id '{id}'");

        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
