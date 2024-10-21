
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Infrastructure.Persistence;
using System.Collections;

namespace DGPCE.Sigemad.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable _repositories;
    private readonly SigemadDbContext _context;

    public UnitOfWork(SigemadDbContext context)
    {
        _context = context;
    }

    public SigemadDbContext SigemadDbContext => _context;

    public async Task<int> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories == null)
        {
            _repositories = new Hashtable();
        }

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return _repositories[type] as IAsyncRepository<TEntity>;
    }

}
