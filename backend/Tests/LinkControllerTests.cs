using Microsoft.AspNetCore.Mvc;
using Moq;
using URLShorter.Controllers;
using URLShorter.Abstractions;
using URLShorter.Models;
using URLShorter.DTO;


namespace URLShorter.Tests.Controllers
{
    public class LinkControllerSimpleTests
    {
        private readonly Mock<IURLServices> _mockService;
        private readonly LinkController _controller;
        private readonly CancellationToken _cancellationToken;

        public LinkControllerSimpleTests()
        {
            _mockService = new Mock<IURLServices>();
            _controller = new LinkController(_mockService.Object);
            _cancellationToken = CancellationToken.None;
        }

        // GetLinks успешное получение списка ссылок
        [Fact]
        public async Task GetLinks_ShouldReturnOkWithLinksList()
        {
            var expectedLinks = new List<Link>
            {
                new Link
                {
                    Id = Guid.NewGuid(),
                    Url = "https://example1.com",
                    ShortUrl = "abc123",
                    CountClick = 0
                },
                new Link
                {
                    Id = Guid.NewGuid(),
                    Url = "https://example2.com",
                    ShortUrl = "def456",
                    CountClick = 5
                }
            };

            _mockService.Setup(x => x.GetAllAsync(_cancellationToken))
                        .ReturnsAsync(expectedLinks);  

            var result = await _controller.GetLinks(new LinksFilterRequset(), _cancellationToken);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLinks = Assert.IsType<List<Link>>(okResult.Value);
            Assert.Equal(2, returnedLinks.Count);
            Assert.Equal("https://example1.com", returnedLinks[0].Url);
        }

        // CreateLink успешное создание ссылки
        [Fact]
        public async Task CreateLink_ShouldReturnOkWithCreatedLink()
        {
            // Arrange
            var request = new CreateLinkRequest { Url = "https://newlink.com" };
            var createdLink = new Link
            {
                Id = Guid.NewGuid(),
                Url = request.Url,
                ShortUrl = "new123",
                CountClick = 0
            };

            _mockService.Setup(x => x.CreateLinkAsync(It.IsAny<Link>(), _cancellationToken))
                        .ReturnsAsync(createdLink);  // Возвращаем Link, а не CreateLinkResponse

            // Act
            var result = await _controller.CreateLink(request, _cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLink = Assert.IsType<Link>(okResult.Value);
            Assert.Equal("https://newlink.com", returnedLink.Url);
            Assert.Equal("new123", returnedLink.ShortUrl);
        }

        // RedirectToOriginal успешный редирект по короткому коду
        [Fact]
        public async Task RedirectToOriginal_ShouldReturnRedirectToOriginalUrl()
        {
            var shortCode = "abc123";
            var originalUrl = "https://original-site.com";

            _mockService.Setup(x => x.RedirectAsync(shortCode, _cancellationToken))
                        .ReturnsAsync(originalUrl);

            var result = await _controller.RedirectToOriginal(shortCode, _cancellationToken);

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(originalUrl, redirectResult.Url);
        }

        //DeleteLink успешное удаление ссылки
        [Fact]
        public async Task DeleteLink_ShouldReturnOkWhenDeleted()
        {
            var id = Guid.NewGuid();

            _mockService.Setup(x => x.DeleteAsync(id, _cancellationToken))
                        .ReturnsAsync(true);

            var result = await _controller.DeleteLink(id, _cancellationToken);

            Assert.IsType<OkResult>(result);
        }

        //UpdateUrlAsync успешное обновление URL
        [Fact]
        public async Task UpdateUrlAsync_ShouldReturnOkWithUpdatedLink()
        {
            var id = Guid.NewGuid();
            var request = new UpdateUrlRequest { Url = "https://updated-link.com" };
            var updatedLink = new Link
            {
                Id = id,
                Url = request.Url,
                ShortUrl = "updated123",
                CountClick = 0
            };

            _mockService.Setup(x => x.UpdateUrlAsync(id, request.Url, _cancellationToken))
                        .ReturnsAsync(updatedLink);  // Возвращаем Link

            var result = await _controller.UpdateUrlAsync(id, request, _cancellationToken);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedLink = Assert.IsType<Link>(okResult.Value);
            Assert.Equal("https://updated-link.com", returnedLink.Url);
            Assert.Equal("updated123", returnedLink.ShortUrl);
        }

        //CreateLink возвращает null
        [Fact]
        public async Task CreateLink_WhenServiceFails_ReturnsBadRequest()
        {
            var request = new CreateLinkRequest { Url = "https://newlink.com" };

            _mockService.Setup(x => x.CreateLinkAsync(It.IsAny<Link>(), _cancellationToken))
                        .ReturnsAsync((Link)null!);  


            var result = await _controller.CreateLink(request, _cancellationToken);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Failed to create link", badRequestResult.Value);
        }
    }
}