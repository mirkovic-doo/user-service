namespace UserService.Contracts.Repositories;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T> UpdateAsync(T entity);
    Task<T> CreateAsync(T entity);
    Task DeleteAsync(T entity);
}

