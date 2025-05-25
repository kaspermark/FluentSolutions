using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace FluentHttpClient.Tests;
public class PostFluentHttpClientTests
{
    private readonly FluentClient Client = new FluentClient(new HttpClient());

    [Fact]
    public async Task Post_WithHttpContentBody_ShouldBuildCorrectRequest()
    {
        // Arrange
        var expectedJson = "{\"Id\":1}";
        HttpContent httpContent = new StringContent(expectedJson, Encoding.UTF8, "application/json");

        // Act
        var response = await Client
             .Post()
             .UseHttps("httpbin.org/post")
             .WithBody(httpContent)
             .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Content.Should().NotBe(null);

        var sentContent = await response.RequestMessage.Content.ReadAsStringAsync();
        sentContent.Should().Be(expectedJson);

        response.RequestMessage.Content.Headers.ContentType.Should().NotBe(null);
        response.RequestMessage.Content.Headers.ContentType.MediaType.Should().Be("application/json");
        response.RequestMessage.Content.Headers.ContentType.CharSet.Should().Be("utf-8");
    }

    [Fact]
    public async Task Post_WithStringBody_ShouldBuildCorrectRequest()
    {
        // Arrange
        var expectedBody = "{\"id\":1}";
        var mediaType = "text/plain";

        // Act
        var response = await Client
            .Post()
            .UseHttps("httpbin.org/post")
            .WithBody(expectedBody, mediaType)
            .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Content.Should().NotBe(null);

        var actualBody = await response.RequestMessage.Content.ReadAsStringAsync();
        actualBody.Should().Be(expectedBody);

        response.RequestMessage.Content.Headers.ContentType.Should().NotBe(null);
        response.RequestMessage.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
    }

    [Fact]
    public async Task Post_WithModelBody_ShouldBuildCorrectRequest()
    {
        // Arrange
        var model = new { Id = 1 };
        var expectedJson = JsonConvert.SerializeObject(model);

        // Act
        var response = await Client
            .Post()
            .UseHttps("httpbin.org/post")
            .WithBody(model)
            .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Content.Should().NotBe(null);

        var actualJson = await response.RequestMessage.Content.ReadAsStringAsync();
        actualJson.Should().Be(expectedJson);

        response.RequestMessage.Content.Headers.ContentType.Should().NotBe(null);
        response.RequestMessage.Content.Headers.ContentType.MediaType.Should().Be("application/json");
        response.RequestMessage.Content.Headers.ContentType.CharSet.Should().Be("utf-8");
    }

    [Fact]
    public async Task Post_WithBodyCalledTwice_ShouldThrowException()
    {
        // Arrange
        var body = new { Id = 1 };
        var secondBody = "This should not be allowed";

        // Act
        Func<Task> act = async () => await Client
            .Post()
            .UseHttps("httpbin.org/post")
            .WithBody(body)
            .WithBody(secondBody)
            .ExecuteAsync();

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>();
    }
}
