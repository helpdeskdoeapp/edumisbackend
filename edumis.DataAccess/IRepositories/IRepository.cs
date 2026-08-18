using System.Linq.Expressions;

namespace edumis.DataAccess.IRepositories;

public interface IRepository<T> where T : class {
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filters = null);
    Task Add(T entity);
    Task Remove(T entity);
    Task AddRange(IEnumerable<T> entities);
    Task RemoveRange(IEnumerable<T> entities);
    Task<T?> GetFirstOrDefault(Expression<Func<T, bool>>? filters = null);
    Task<bool> Exists(Expression<Func<T, bool>>? filters = null);
    Task<T?> GetLastOrDefault(Expression<Func<T, bool>>? filters = null);
    Task<T?> GetFirstOrDefaultByOrder<TKey>(Expression<Func<T, TKey>> OrderByFilter, Expression<Func<T, bool>>? Searchfilters = null, bool DescendingOrder = false);
    Task<T?> GetLastOrDefaultByOrder<TKey>(Expression<Func<T, TKey>> OrderByFilter, Expression<Func<T, bool>>? Searchfilters = null, bool DescendingOrder = false);
    Task Save();
}
