using LibraryManagement.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Persistence.Postgres.Repos;

public class GenericRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
{
    protected readonly LibraryDbContext _context;

    protected readonly DbSet<TEntity> _dbSet;

    private bool _isDisposed;

    public GenericRepository(LibraryDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>() ?? throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} is not registered in the DbContext.");
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Dispose()
    {
        if (_context != null)
            _context.Dispose();
        _isDisposed = true;
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _dbSet.AsNoTracking().ToList();
    }

    public TEntity? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
}
