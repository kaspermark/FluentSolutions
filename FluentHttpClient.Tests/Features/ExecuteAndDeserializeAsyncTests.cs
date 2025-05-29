using FluentAssertions;

namespace FluentHttpClient.Tests.Features;
public class ExecuteAndDeserializeAsyncTests
{
    private readonly FluentClient Client = new(new HttpClient());

    [Fact]
    public async Task ExecuteAndDeserializeAsync_ShouldDeserializeValidJson()
    {
        // Arrange
        var builder = Client
            .Get()
            .UseHttps("httpbin.org/json");

        // Act
        var result = await builder
            .ExecuteAndDeserializeAsync<Dictionary<string, object>>();

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKey("slideshow");
    }

    [Fact]
    public void ExecuteAndDeserializeAsync_WithEmptyContent_ShouldThrowException()
    {
        // Arrange

        // Act
        Action act = () => Client
                            .Get()
                            .UseHttps("httpbin.org/anything")
                            .AddQueryParam("", "value");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    public class SampleModel
    {
        public string? Slug { get; set; }
    }
    [Fact]
    public async Task ExecuteAndDeserializeAsync_WithInvalidJson_ShouldModelWithNoMatchingValues()
    {
        // Arrange

        // Act
        var sample = await Client
            .Get()
            .UseHttps("httpbin.org/json")
            .ExecuteAndDeserializeAsync<SampleModel>();

        // Assert
        sample.Slug.Should().Be(null);
    }

    [Fact]
    public async Task ExecuteAndDeserializeAsync_WithErrorStatus_ShouldThrow()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
            .Get()
            .UseHttps("httpbin.org/status/400")
            .ExecuteAndDeserializeAsync<object>();

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
