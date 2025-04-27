using Lab_06_Roman_Fernandez.Repository;

namespace Lab_06_Roman_Fernandez.Repository;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> Complete();
}