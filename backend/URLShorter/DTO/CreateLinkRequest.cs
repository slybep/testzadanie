using URLShorter.Abstractions;

namespace URLShorter.DTO
{
    public class CreateLinkRequest 
    { 
        public string Url { get; set; } = string.Empty;
    }
}
