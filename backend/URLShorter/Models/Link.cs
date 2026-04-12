using URLShorter.Abstractions;

namespace URLShorter.Models
{
    public class Link : IDocument
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CountClick { get; set; }
        public string Url { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
    }
}
