using AutoMapper;
using Microsoft.EntityFrameworkCore;
using URLShorter.Abstractions;

namespace URLShorter.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class, IDocument
    {
        protected readonly URLShorterDbContext _context;
        protected readonly DbSet<T> _dbSet;
        //protected readonly IMapper _mapper;
        protected readonly ShortCodeGenerator _shortCodeGenerator;

        public GenericRepository(URLShorterDbContext context, ShortCodeGenerator shortCodeGenerator, IMapper mapper)
        {
            //_mapper = mapper;
            _context = context;
            _dbSet = context.Set<T>();
            _shortCodeGenerator = shortCodeGenerator;
        }

        public virtual async Task<T> CreateAsync(T entity, CancellationToken ct)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
                entity.CreatedAt = DateTime.UtcNow;
            }

            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);

            return entity;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = Activator.CreateInstance<T>();
            var idProperty = typeof(T).GetProperty("Id");
            idProperty?.SetValue(entity, id);

            _dbSet.Attach(entity);
            _dbSet.Remove(entity);

            try
            {
                await _context.SaveChangesAsync(ct);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct)
        {
            return await _dbSet.AsNoTracking().ToListAsync(ct);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public virtual async Task<T?> UpdateAsync(T entity, CancellationToken ct)
        {
            if (entity == null || entity.Id == Guid.Empty)
                return null;

            var existing = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id, ct);
            if (existing == null)
                return null;

            //_mapper.Map(entity, existing);
            await _context.SaveChangesAsync(ct);
            return existing;
        }
    }
}