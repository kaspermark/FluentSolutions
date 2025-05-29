using FluentAssertions;

namespace FluentHttpClient.Tests.Features;
public class AddHeaderTests
{
    private readonly FluentClient Client = new(new HttpClient());

    [Theory]
    [InlineData("GET", 1)]
    [InlineData("GET", 5)]
    [InlineData("GET", 10)]
    [InlineData("GET", 100)]
    [InlineData("POST", 1)]
    [InlineData("POST", 5)]
    [InlineData("POST", 10)]
    [InlineData("POST", 100)]
    [InlineData("PUT", 1)]
    [InlineData("PUT", 5)]
    [InlineData("PUT", 10)]
    [InlineData("PUT", 100)]
    [InlineData("PATCH", 1)]
    [InlineData("PATCH", 5)]
    [InlineData("PATCH", 10)]
    [InlineData("PATCH", 100)]
    [InlineData("DELETE", 1)]
    [InlineData("DELETE", 5)]
    [InlineData("DELETE", 10)]
    [InlineData("DELETE", 100)]
    public async Task Request_WithHeaders_ShouldHaveCorrectAmountOfHeaders(string method, int expectedHeaderCount)
    {
        // Arrange
        var clientBuilder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        clientBuilder
            .UseHttps("httpbin.org/anything");

        for (int i = 0; i < expectedHeaderCount; i++)
        {
            clientBuilder.AddHeader($"Name{i}", $"Value{i}");
        }

        // Act
        var response = await clientBuilder.ExecuteAsync();

        // Assert
        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Headers.Should().HaveCount(expectedHeaderCount);
    }

    [Theory]
    [InlineData("GET", null, "Value")]
    [InlineData("GET", "", "Value")]
    [InlineData("GET", "   ", "Value")]
    [InlineData("GET", "Name", null)]
    [InlineData("GET", "Name", "")]
    [InlineData("GET", "Name", "        ")]
    [InlineData("POST", null, "Value")]
    [InlineData("POST", "", "Value")]
    [InlineData("POST", "   ", "Value")]
    [InlineData("POST", "Name", null)]
    [InlineData("POST", "Name", "")]
    [InlineData("POST", "Name", "        ")]
    [InlineData("PUT", null, "Value")]
    [InlineData("PUT", "", "Value")]
    [InlineData("PUT", "   ", "Value")]
    [InlineData("PUT", "Name", null)]
    [InlineData("PUT", "Name", "")]
    [InlineData("PUT", "Name", "        ")]
    [InlineData("PATCH", null, "Value")]
    [InlineData("PATCH", "", "Value")]
    [InlineData("PATCH", "   ", "Value")]
    [InlineData("PATCH", "Name", null)]
    [InlineData("PATCH", "Name", "")]
    [InlineData("PATCH", "Name", "        ")]
    [InlineData("DELETE", null, "Value")]
    [InlineData("DELETE", "", "Value")]
    [InlineData("DELETE", "   ", "Value")]
    [InlineData("DELETE", "Name", null)]
    [InlineData("DELETE", "Name", "")]
    [InlineData("DELETE", "Name", "        ")]
    public void Request_WithInvalidHeader_ShouldThrowArgumentException(string methodName, string? headerName, string? headerValue)
    {
        // Arrange
        var clientBuilder = methodName switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        // Act
        Action act = () => clientBuilder
            .UseHttps("httpbin.org/anything")
            .AddHeader(headerName!, headerValue!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task AddHeader_WithDuplicateHeaders_ShouldOnlyHaveOneHeaderInRequest()
    {
        // Arrange
        var request = Client.Get()
            .AddHeader("Header", "Value")
            .AddHeader("Header", "Value")
            .UseHttps("httpbin.org/anything");

        // Act
        var response = await request
                                .ExecuteAsync();

        // Assert
        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Headers.Count().Should().Be(1);
    }
}
