using FluentAssertions;
using System.Text;

namespace FluentHttpClient.Tests.Methods;
public class GetFluentHttpClientTests
{
    private readonly FluentClient Client = new(new HttpClient());

    [Fact]
    public async Task Get_WithNormalRequest_ShouldGenerateNoBodyInRequest()
    {
        // Arrange

        // Act
        var response = await Client
            .Get()
            .UseHttps("httpbin.org/get")
            .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Content.Should().Be(null);
    }

    [Fact]
    public async Task Get_WithHttpContentBody_ShouldThrowException()
    {
        // Arrange
        var json = "{\"Id\":1}";
        HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .WithBody(httpContent)
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Get_WithStringBody_ShouldThrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .WithBody("{\"id\":1}", "text/plain")
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Get_WithModelBody_ShouldThrowException()
    {
        // Arrange
        var model = new
        {
            Id = 1
        };

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .WithBody(model)
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
