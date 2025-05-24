using FluentAssertions;
using System.Text;

namespace FluentHttpClient.Tests;
public class GetFluentHttpClientTests : FluentClientTestBase
{
    private readonly FluentClient Client = new FluentClient(new HttpClient());

    [Fact]
    public async Task Get_WithHttpsUrl_ShouldBuildCorrectRequest()
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
        response.RequestMessage.Method.Should().Be(HttpMethod.Get);
        response.RequestMessage.RequestUri.Should().NotBe(null);
        response.RequestMessage.RequestUri.Scheme.Should().Be("https");
        response.RequestMessage.RequestUri.Should().Be("https://httpbin.org/get");
    }

    [Fact]
    public async Task Get_WithHttpUrl_ShouldBuildCorrectRequest()
    {
        // Arrange

        // Act
        var response = await Client
            .Get()
            .UseHttp("httpbin.org/get")
            .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Content.Should().Be(null);
        response.RequestMessage.Method.Should().Be(HttpMethod.Get);
        response.RequestMessage.RequestUri.Should().NotBe(null);
        response.RequestMessage.RequestUri.Scheme.Should().Be("http");
        response.RequestMessage.RequestUri.Should().Be("http://httpbin.org/get");
    }

    [Fact]
    public async Task Get_WithFaultyHttpsUrl_ShouldThrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
            .Get()
            .UseHttps("httpbin.ork/get")
            .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
        .WithMessage("*httpbin.ork*");
    }

    [Fact]
    public async Task Get_WithFaultyHttpUrl_ShouldThrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
            .Get()
            .UseHttp("httpbin.ork/get")
            .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
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
             .UseHttp("httpbin.org/get")
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
             .UseHttp("httpbin.org/get")
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
             .UseHttp("httpbin.org/get")
             .WithBody(model)
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Get_WithHeaders_ShouldHaveCorrektAmountOfHeaders()
    {
        // Arrange

        // Act
        var response = await Client
            .Get()
            .UseHttps("httpbin.org/get")
            .AddHeader("Accept", "application/json")
            .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Headers.Should().HaveCount(1);
    }

    [Fact]
    public async Task Get_With15Headers_ShouldHaveCorrektAmountOfHeaders()
    {
        // Arrange

        // Act
        var response = await Client
            .Get()
            .UseHttps("httpbin.org/get")
            .AddHeader("Accept1", "application/json")
            .AddHeader("Accept2", "application/json")
            .AddHeader("Accept3", "application/json")
            .AddHeader("Accept4", "application/json")
            .AddHeader("Accept5", "application/json")
            .AddHeader("Accept6", "application/json")
            .AddHeader("Accept7", "application/json")
            .AddHeader("Accept8", "application/json")
            .AddHeader("Accept9", "application/json")
            .AddHeader("Accept10", "application/json")
            .AddHeader("Accept11", "application/json")
            .AddHeader("Accept12", "application/json")
            .AddHeader("Accept13", "application/json")
            .AddHeader("Accept14", "application/json")
            .AddHeader("Accept15", "application/json")
            .ExecuteAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        response.RequestMessage.Should().NotBe(null);
        response.RequestMessage.Headers.Should().HaveCount(15);
    }

    [Fact]
    public async Task Get_WithNullHeaderName_ShouldthrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .AddHeader(null, "Value")
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Get_WithEmptyHeaderName_ShouldthrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .AddHeader("", "Value")
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Get_WithWhiteSpaceHeaderName_ShouldthrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .AddHeader("          ", "Value")
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Get_WithNullHeaderValue_ShouldthrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .AddHeader("Name", null)
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Get_WithEmptyHeaderValue_ShouldthrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .AddHeader("Name", "")
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Get_WithWhiteSpaceHeaderValue_ShouldthrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await Client
             .Get()
             .UseHttps("httpbin.org/get")
             .AddHeader("Name", "              ")
             .ExecuteAsync();

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
