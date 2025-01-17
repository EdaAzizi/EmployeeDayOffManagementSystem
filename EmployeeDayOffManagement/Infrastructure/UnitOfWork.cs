using System.Collections;
using EmployeeDayOffManagement.Application.Interfaces;

namespace EmployeeDayOffManagement.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly LmsDbContext _dbContext;

    private Hashtable _repositories;

    public UnitOfWork(LmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Complete()
    {
        var numberOfAffectedRows = _dbContext.SaveChanges();
        return numberOfAffectedRows > 0;
    }

    public ILmsRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories == null)
            _repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.Contains(type))
        {
            var repositoryType = typeof(LmsRepository<>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (ILmsRepository<TEntity>)_repositories[type];
    }
}