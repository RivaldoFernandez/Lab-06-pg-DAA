

using System.Collections;
using Lab_06_Roman_Fernandez.Models;

namespace Lab_06_Roman_Fernandez.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ColegioDB _context;
    private readonly Hashtable _repositories;

    public UnitOfWork(ColegioDB context)
    {
        _context = context;
        _repositories = new Hashtable();
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity).Name;

        if (_repositories.ContainsKey(type))
            return (IGenericRepository<TEntity>)_repositories[type]!;

        var repoType = typeof(GenericRepository<>);
        var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _context);

        if (repoInstance == null)
            throw new Exception($"No se pudo crear el repositorio para el tipo {type}");

        _repositories.Add(type, repoInstance);
        return (IGenericRepository<TEntity>)repoInstance;
    }
}