using FluentAssertions;
using System.Net;

namespace FluentHttpClient.Tests.Features;
public class ExecuteAsyncTests
{
    private readonly FluentClient Client = new(new HttpClient());

    [Fact]
    public async Task ExecuteAsync_With404_ShouldReturnStatusCode()
    {
        // Arrange
        var builder = Client
            .Get()
            .UseHttps("httpbin.org/status/404");

        // Act
        var response = await builder.ExecuteAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ExecuteAsync_WithNoUrl_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var builder = Client.Get(); // No URL set

        // Act
        Func<Task> act = async () => await builder.ExecuteAsync();

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>();
    }
}
