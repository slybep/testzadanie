using URLShorter.Models;

public interface IURLRepository
{
    Task<Link> CreateAsync(Link link, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<List<Link>> GetAllAsync(CancellationToken ct);
    Task<Link?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<string?> RedirectToOriginalAsync(string shortUrl, CancellationToken ct);
    Task<Link?> UpdateAsync(Link link, CancellationToken ct);
    Task<Link?> UpdateUrlAsync(Guid id, string newUrl, CancellationToken ct);
}