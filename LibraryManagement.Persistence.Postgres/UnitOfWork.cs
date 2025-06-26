using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Persistence.Postgres;

public class UnitOfWork<TContext> : IUnitOfWork, IDisposable where TContext : DbContext
{
    private bool _disposed = false;
    private string _errorMessage = string.Empty;

    private readonly TContext _context;

    private IDbContextTransaction? _contextTransaction;

    public UnitOfWork(TContext context)
    {
        _context = context;
    }

    public void CreateTransaction()
    {
        _contextTransaction = _context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        HandleNotCreatedTransaction();
        _contextTransaction?.Commit();
    }

    public void RollbackTransaction()
    {
        HandleNotCreatedTransaction();
        _contextTransaction?.Rollback();
        _contextTransaction?.Dispose();
    }

    public void SaveChanges()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            _errorMessage = "An error occurred while saving changes to the database.";
            throw new InvalidOperationException(_errorMessage, ex);
        }
        catch (Exception ex)
        {
            _errorMessage = "An unexpected error occurred while saving changes.";
            throw new InvalidOperationException(_errorMessage, ex);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();
        _disposed = true;
    }

    private void HandleNotCreatedTransaction()
    {
        if (_contextTransaction == null)
        {
            _errorMessage = "No transaction has been created.";
            throw new InvalidOperationException(_errorMessage);
        }
    }
}
