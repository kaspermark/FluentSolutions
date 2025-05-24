using FluentAssertions;

namespace FluentHttpClient.Tests;
public class FluentHttpClientTests
{
    [Fact]
    public async Task Get_ToPublicApi_ShouldReturnSuccessStatusCode()
    {
        // Arrange
        var client = new FluentClient(new HttpClient());

        // Act
        var response = await client
            .Get()
            .Url("https://jsonplaceholder.typicode.com/posts/1")
            .WithBody("Test")
            .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("\"id\": 1");
    }

    [Fact]
    public async Task Get_ToPublicApi_ShouldReturnSuccessStatusCodeNoBody()
    {
        // Arrange
        var client = new FluentClient(new HttpClient());

        // Act
        var response = await client
            .Get()
            .Url("https://jsonplaceholder.typicode.com/posts/1")
            .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("\"id\": 1");
    }
}
