public interface IRepository<T> where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken ct);
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);
    Task<T?> UpdateAsync(T entity, CancellationToken ct);  
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken ct);
    

}