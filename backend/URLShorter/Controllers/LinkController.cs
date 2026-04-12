using Microsoft.AspNetCore.Mvc;
using URLShorter.DTO;
using URLShorter.Models;
using URLShorter.Abstractions;
using URLShorter.Serivces;

namespace URLShorter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinkController : ControllerBase
    {
        private readonly IURLServices _uRLServices;
        public LinkController(IURLServices uRLServices)
        {
            _uRLServices = uRLServices;
        }
        [HttpGet("urls")]
        public async Task<ActionResult<List<CreateLinkResponse>>> GetLinks([FromQuery] LinksFilterRequset dto, CancellationToken ct)
        {
            var result = await _uRLServices.GetAllAsync(ct);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Link>> CreateLink([FromBody]CreateLinkRequest dto, CancellationToken ct)
        {
            var link = new Link
            {
                Url = dto.Url,
                CountClick = 0
            };

            var result = await _uRLServices.CreateLinkAsync(link, ct);

            if(result == null)
            {
                return BadRequest("Failed to create link");
            }

            return Ok(result);
        }
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode, CancellationToken ct)
        {
            var originalUrl = await _uRLServices.RedirectAsync(shortCode, ct);

            if (string.IsNullOrEmpty(originalUrl))
            {
                return NotFound("Short URL not found");
            }

            return Redirect(originalUrl);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLink(Guid id, CancellationToken ct)
        {
            var link = await _uRLServices.DeleteAsync(id, ct);

            if (link == false)
            {
                return NotFound("Object not found");
            }

            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUrlAsync(Guid id, [FromBody] UpdateUrlRequest request, CancellationToken ct)
        {
            var link = await _uRLServices.UpdateUrlAsync(id, request.Url, ct);

            if (link == null)
            {
                return NotFound();
            }

            return Ok(link);
        }
    }
}
