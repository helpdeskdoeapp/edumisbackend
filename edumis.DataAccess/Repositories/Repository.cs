using edumis.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace edumis.DataAccess.Repositories;

public class Repository<T>(ApplicationDBContext dBContext) : BaseADODbHandler(dBContext), IRepository<T>
    where T : class {
    private readonly ApplicationDBContext dBContext = dBContext;
    DbSet<T> _dbSet = dBContext.Set<T>();

    public async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRange(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        IQueryable<T> query = _dbSet;
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filters = null)
    {
        IQueryable<T> query = _dbSet;
        if (filters != null)
            return await query.Where(filters).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<bool> Exists(Expression<Func<T, bool>>? filters = null)
    {
        IQueryable<T> query = _dbSet;
        if (filters != null)
            return await query.AnyAsync(filters);
        return false;
    }

    public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>>? filters = null)
    {
        IQueryable<T> query = _dbSet;
        if (filters != null)
            query = query.Where(filters);
        return await query.FirstOrDefaultAsync();
    }

    public async Task Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task<T?> GetLastOrDefault(Expression<Func<T, bool>>? filters = null)
    {
        IQueryable<T> query = _dbSet;
        if (filters != null)
            query = query.Where(filters);
        return await query.LastOrDefaultAsync();
    }

    public async Task<T?> GetFirstOrDefaultByOrder<TKey>(Expression<Func<T, TKey>> OrderByFilter, Expression<Func<T, bool>>? Searchfilters = null, bool DescendingOrder = false)
    {
        IQueryable<T> query = _dbSet;
        if (Searchfilters != null)
            query = DescendingOrder ? query.Where(Searchfilters).OrderByDescending(OrderByFilter) : query.Where(Searchfilters).OrderBy(OrderByFilter);
        else
            query = DescendingOrder ? query.OrderByDescending(OrderByFilter) : query.OrderBy(OrderByFilter);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<T?> GetLastOrDefaultByOrder<TKey>(Expression<Func<T, TKey>> OrderByFilter, Expression<Func<T, bool>>? Searchfilters = null, bool DescendingOrder = false)
    {
        IQueryable<T> query = _dbSet;
        if (Searchfilters != null)
            query = DescendingOrder ? query.Where(Searchfilters).OrderByDescending(OrderByFilter) : query.Where(Searchfilters).OrderBy(OrderByFilter);
        else
            query = DescendingOrder ? query.OrderByDescending(OrderByFilter) : query.OrderBy(OrderByFilter);
        return await query.LastOrDefaultAsync();
    }

    public async Task Save() => await dBContext.SaveChangesAsync();
}
