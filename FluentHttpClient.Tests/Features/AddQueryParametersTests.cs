using FluentAssertions;

namespace FluentHttpClient.Tests.Features;
public class AddQueryParametersTests
{
    private readonly FluentClient Client = new(new HttpClient());

    [Theory]
    [InlineData("GET")]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("PATCH")]
    [InlineData("DELETE")]
    public void AddQueryParameters_WithEmptyName_ShouldThrowArgumentException(string methodName)
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
            .UseHttps("httpbin.org/anything").AddQueryParam("", "value");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("GET", "Params1", "Value")]
    [InlineData("POST", "Params1", "Value")]
    [InlineData("PUT", "Params1", "Value")]
    [InlineData("PATCH", "Params1", "Value")]
    [InlineData("DELETE", "Params1", "Value")]

    public async Task AddQueryParameters_ShouldIncludeQueryParametersInURI(string method, string name, string value)
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
            .UseHttps("httpbin.org/anything")
            .AddQueryParam(name, value);

        // Act
        var response = await clientBuilder.ExecuteAsync();

        // Assert
        response.RequestMessage.Should().NotBeNull();
        response.RequestMessage.RequestUri.Should().NotBeNull();
        response.RequestMessage.RequestUri.Query.Should().Contain(name + "=" + value);
    }

    [Theory]
    [InlineData("GET")]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("PATCH")]
    [InlineData("DELETE")]
    public async Task AddQueryParameters_ShouldIncludeMultipleQueryParametersInURI(string methodName)
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

        clientBuilder
            .UseHttps("httpbin.org/anything")
            .AddQueryParam("Param1", "value")
            .AddQueryParam("Param2", "value");

        // Act
        var response = await clientBuilder.ExecuteAsync();

        // Assert
        response.RequestMessage.Should().NotBeNull();
        response.RequestMessage.RequestUri.Should().NotBeNull();
        response.RequestMessage.RequestUri.Query.Should().Contain("Param1=value");
        response.RequestMessage.RequestUri.Query.Should().Contain("Param2=value");
    }

    [Theory]
    [InlineData("GET")]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("PATCH")]
    [InlineData("DELETE")]
    public void AddQueryParam_WithoutSettingUrl_ShouldThrowInvalidOperationException(string methodName)
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
        Action act = () => clientBuilder.AddQueryParam("key", "value");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>();
    }
}
