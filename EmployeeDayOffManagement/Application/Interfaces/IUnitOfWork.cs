namespace EmployeeDayOffManagement.Application.Interfaces;

public interface IUnitOfWork
{
    public ILmsRepository<TEntity> Repository<TEntity>() where TEntity : class;

    bool Complete();
}