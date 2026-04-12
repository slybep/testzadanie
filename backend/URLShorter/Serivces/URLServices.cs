using URLShorter.Abstractions;
using URLShorter.Models;
namespace URLShorter.Serivces
{
    public class URLServices : IURLServices
    {
        private readonly IURLRepository _repository;


        public URLServices(IURLRepository urlrepository)
        {
            _repository = urlrepository;
        }
        public async Task<List<Link>> GetAllAsync(CancellationToken ct)
        {
            return await _repository.GetAllAsync(ct);
        }
        public async Task<Link> CreateLinkAsync(Link link, CancellationToken ct)
        {
            return await _repository.CreateAsync(link, ct);
        }
        public async Task<Link?> UpdateUrlAsync(Guid id, string URL, CancellationToken ct)
        {
            return await _repository.UpdateUrlAsync(id, URL, ct);
        }
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            return await _repository.DeleteAsync(id, ct);
        }
        public async Task<string?> RedirectAsync(string shortUrl, CancellationToken ct)
        {
            return await _repository.RedirectToOriginalAsync(shortUrl, ct);
        }
    }
}
