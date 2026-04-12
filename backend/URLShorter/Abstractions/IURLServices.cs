using URLShorter.Models;

namespace URLShorter.Abstractions
{
    public interface IURLServices
    {
        Task<Link> CreateLinkAsync(Link link, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<string?> RedirectAsync(string shortUrl, CancellationToken ct);
        Task<List<Link>> GetAllAsync(CancellationToken ct);
        Task<Link?> UpdateUrlAsync(Guid id, string URL, CancellationToken ct);
    }
}