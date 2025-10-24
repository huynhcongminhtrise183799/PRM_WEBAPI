using System.Linq.Expressions;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
	Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

	Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);

	IQueryable<T> GetQueryable();
	void DeleteRange(IEnumerable<T> entities);
}