using Microsoft.EntityFrameworkCore;
using URLShorter.Models;

namespace URLShorter.Repositories
{
    public class URLRepository : IURLRepository
    {
        private readonly URLShorterDbContext _context;
        private readonly ShortCodeGenerator _shortCodeGenerator;

        public URLRepository(URLShorterDbContext context, ShortCodeGenerator shortCodeGenerator)
        {
            _context = context;
            _shortCodeGenerator = shortCodeGenerator;
        }

        public async Task<Link> CreateAsync(Link link, CancellationToken ct)
        {
            if (link.Id == Guid.Empty)
            {
                link.Id = Guid.NewGuid();
                link.CreatedAt = DateTime.UtcNow;
                link.ShortUrl = _shortCodeGenerator.Generate(7);   //возвращаем код, присваиваем
            }
            await _context.links.AddAsync(link, ct);
            await _context.SaveChangesAsync(ct);
            return link;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var link = new Link { Id = id };
            _context.links.Attach(link);
            _context.links.Remove(link);

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

        public async Task<List<Link>> GetAllAsync(CancellationToken ct)
        {
            return await _context.links.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Link?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.links.FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async Task<Link?> UpdateAsync(Link link, CancellationToken ct)
        {
            if (link == null || link.Id == Guid.Empty)
            {
                return null;
            }
            var existing = await _context.links.FirstOrDefaultAsync(e => e.Id == link.Id, ct); // можно исключить на случай если нужно будет менять ещё что-то
            if (existing == null)
            {
                return null;
            }
            existing.Url = link.Url;
            existing.ShortUrl = link.ShortUrl;

            await _context.SaveChangesAsync(ct);
            return existing;
        }

        public async Task<Link?> UpdateUrlAsync(Guid id, string newUrl, CancellationToken ct)
        {
            var link = await _context.links.FirstOrDefaultAsync(e => e.Id == id, ct);
            if (link == null)
                return null;

            link.Url = newUrl;
            await _context.SaveChangesAsync(ct);
            return link;
        }

        public async Task<string?> RedirectToOriginalAsync(string shortUrl, CancellationToken ct)
        {
            var link = await _context.links.FirstOrDefaultAsync(l => l.ShortUrl == shortUrl, ct);
            if (link == null)
            {
                return null;
            }
            link.CountClick++;
            await _context.SaveChangesAsync(ct);
            return link.Url;
        }
    }
}