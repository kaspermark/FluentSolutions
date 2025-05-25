using FluentAssertions;
using System.Net;

namespace FluentHttpClient.Tests;
public class GeneralFluentHttpClientTests
{
    private readonly FluentClient Client = new FluentClient(new HttpClient());

    [Theory]
    [InlineData("GET", "https", "httpbin.org/get")]
    [InlineData("GET", "http", "httpbin.org/get")]
    [InlineData("POST", "https", "httpbin.org/post")]
    [InlineData("POST", "http", "httpbin.org/post")]
    public async Task Request_WithCorrectRequest_ShouldCreateCorrectRequest(string methodName, string scheme, string url)
    {
        var method = new HttpMethod(methodName);

        var clientBuilder = methodName switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            _ => throw new NotSupportedException()
        };

        var requestBuilder = scheme switch
        {
            "https" => clientBuilder.UseHttps(url),
            "http" => clientBuilder.UseHttp(url),
            _ => throw new NotSupportedException()
        };

        var response = await requestBuilder.ExecuteAsync();

        response.IsSuccessStatusCode.Should().BeTrue();
        response.RequestMessage.Should().NotBeNull();

        response.RequestMessage.Method.Should().Be(method);
        response.RequestMessage.RequestUri.Should().NotBeNull();
        response.RequestMessage.RequestUri.Scheme.Should().Be(scheme);
        response.RequestMessage.RequestUri.Host.Should().Be($"{url.Split("/")[0]}");
        response.RequestMessage.RequestUri.AbsolutePath.Should().Be($"/{url.Split("/")[1]}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("GET", "httpbin.org/post", HttpStatusCode.MethodNotAllowed)]
    [InlineData("POST", "httpbin.org/get", HttpStatusCode.MethodNotAllowed)]
    public async Task Request_WithWrongMethodEndpoint_ShouldReturnExpectedStatusCode(string method, string url, HttpStatusCode expectedStatus)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            _ => throw new NotSupportedException()
        };

        var response = await builder.UseHttps(url).ExecuteAsync();
        response.StatusCode.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData("GET", "httpbin.ork/get")]
    [InlineData("POST", "httpbin.ork/post")]
    public async Task Request_WithFaultyHttpsUrl_ShouldThrowException(string method, string url)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            _ => throw new NotSupportedException()
        };

        Func<Task> act = async () => await builder.UseHttps(url).ExecuteAsync();

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Theory]
    [InlineData("GET", "httpbin.ork/get")]
    [InlineData("POST", "httpbin.ork/post")]
    public async Task Request_WithFaultyHttpUrl_ShouldThrowException(string method, string url)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            _ => throw new NotSupportedException()
        };

        Func<Task> act = async () => await builder.UseHttp(url).ExecuteAsync();

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Theory]
    [InlineData("GET", "httpbin.org/get")]
    [InlineData("POST", "httpbin.org/post")]
    public async Task Request_WithTwoHttpsUrl_ShouldThrowException(string method, string url)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            _ => throw new NotSupportedException()
        };

        Func<Task> act = async () => await builder.UseHttps(url).UseHttps(url).ExecuteAsync();

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData("GET", "httpbin.org/get")]
    [InlineData("POST", "httpbin.org/post")]
    public async Task Request_WithTwoHttpUrl_ShouldThrowException(string method, string url)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            _ => throw new NotSupportedException()
        };

        Func<Task> act = async () => await builder.UseHttp(url).UseHttp(url).ExecuteAsync();

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData("GET", 1)]
    [InlineData("GET", 5)]
    [InlineData("GET", 10)]
    [InlineData("GET", 100)]
    public async Task Request_WithHeaders_ShouldHaveCorrectAmountOfHeaders(string methodName, int expectedHeaderCount)
    {
        var method = new HttpMethod(methodName);

        var clientBuilder = methodName switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        clientBuilder
            .UseHttps("httpbin.org/anything");

        for (int i = 0; i < expectedHeaderCount; i++)
        {
            clientBuilder.AddHeader($"Name{i}", $"Value{i}");
        }

        var response = await clientBuilder.ExecuteAsync();

        response.RequestMessage.Should().NotBeNull();
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
    public void Request_WithInvalidHeader_ShouldThrowArgumentException(string methodName, string headerName, string headerValue)
    {
        var method = new HttpMethod(methodName);

        var clientBuilder = methodName switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        Action act = () => clientBuilder
            .UseHttps("httpbin.org/anything")
            .AddHeader(headerName, headerValue);

        act.Should().Throw<ArgumentException>();
    }
}
