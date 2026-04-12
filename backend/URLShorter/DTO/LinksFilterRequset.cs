using URLShorter.Models;

namespace URLShorter.DTO
{
    public class LinksFilterRequset
    {
        public int? Page { get; init; } = 1;
        public int? PageSize { get; init; } = 10;

    }
}
