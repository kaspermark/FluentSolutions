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
    [InlineData("PUT", "https", "httpbin.org/put")]
    [InlineData("PUT", "http", "httpbin.org/put")]
    [InlineData("PATCH", "https", "httpbin.org/patch")]
    [InlineData("PATCH", "http", "httpbin.org/patch")]
    [InlineData("DELETE", "https", "httpbin.org/delete")]
    [InlineData("DELETE", "http", "httpbin.org/delete")]
    public async Task Request_WithCorrectRequest_ShouldCreateCorrectRequest(string methodName, string scheme, string url)
    {
        var method = new HttpMethod(methodName);

        var clientBuilder = methodName switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
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
        response.RequestMessage.Should().NotBe(null);

        response.RequestMessage.Method.Should().Be(method);
        response.RequestMessage.RequestUri.Should().NotBe(null);
        response.RequestMessage.RequestUri.Scheme.Should().Be(scheme);
        response.RequestMessage.RequestUri.Host.Should().Be($"{url.Split("/")[0]}");
        response.RequestMessage.RequestUri.AbsolutePath.Should().Be($"/{url.Split("/")[1]}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("GET", "httpbin.org/post", HttpStatusCode.MethodNotAllowed)]
    [InlineData("GET", "httpbin.org/put", HttpStatusCode.MethodNotAllowed)]
    [InlineData("GET", "httpbin.org/patch", HttpStatusCode.MethodNotAllowed)]
    [InlineData("GET", "httpbin.org/delete", HttpStatusCode.MethodNotAllowed)]
    [InlineData("POST", "httpbin.org/get", HttpStatusCode.MethodNotAllowed)]
    [InlineData("POST", "httpbin.org/put", HttpStatusCode.MethodNotAllowed)]
    [InlineData("POST", "httpbin.org/patch", HttpStatusCode.MethodNotAllowed)]
    [InlineData("POST", "httpbin.org/delete", HttpStatusCode.MethodNotAllowed)]
    [InlineData("PUT", "httpbin.org/get", HttpStatusCode.MethodNotAllowed)]
    [InlineData("PUT", "httpbin.org/post", HttpStatusCode.MethodNotAllowed)]
    [InlineData("PUT", "httpbin.org/patch", HttpStatusCode.MethodNotAllowed)]
    [InlineData("PUT", "httpbin.org/delete", HttpStatusCode.MethodNotAllowed)]
    [InlineData("PATCH", "httpbin.org/get", HttpStatusCode.MethodNotAllowed)]
    [InlineData("PATCH", "httpbin.org/post", HttpStatusCode.MethodNotAllowed)]
    [InlineData("PATCH", "httpbin.org/put", HttpStatusCode.MethodNotAllowed)]
    [InlineData("PATCH", "httpbin.org/delete", HttpStatusCode.MethodNotAllowed)]
    [InlineData("DELETE", "httpbin.org/get", HttpStatusCode.MethodNotAllowed)]
    [InlineData("DELETE", "httpbin.org/post", HttpStatusCode.MethodNotAllowed)]
    [InlineData("DELETE", "httpbin.org/put", HttpStatusCode.MethodNotAllowed)]
    [InlineData("DELETE", "httpbin.org/patch", HttpStatusCode.MethodNotAllowed)]
    public async Task Request_WithWrongMethodEndpoint_ShouldReturnExpectedStatusCode(string method, string url, HttpStatusCode expectedStatus)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        var response = await builder.UseHttps(url).ExecuteAsync();
        response.StatusCode.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData("GET", "httpbin.ork/get")]
    [InlineData("POST", "httpbin.ork/post")]
    [InlineData("PUT", "httpbin.ork/put")]
    [InlineData("PATCH", "httpbin.ork/patch")]
    [InlineData("DELETE", "httpbin.ork/delete")]
    public async Task Request_WithFaultyHttpsUrl_ShouldThrowException(string method, string url)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        Func<Task> act = async () => await builder.UseHttps(url).ExecuteAsync();

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Theory]
    [InlineData("GET", "httpbin.ork/get")]
    [InlineData("POST", "httpbin.ork/post")]
    [InlineData("PUT", "httpbin.ork/put")]
    [InlineData("PATCH", "httpbin.ork/patch")]
    [InlineData("DELETE", "httpbin.ork/delete")]
    public async Task Request_WithFaultyHttpUrl_ShouldThrowException(string method, string url)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        Func<Task> act = async () => await builder.UseHttp(url).ExecuteAsync();

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Theory]
    [InlineData("GET", "httpbin.org/get")]
    [InlineData("POST", "httpbin.org/post")]
    [InlineData("PUT", "httpbin.org/put")]
    [InlineData("PATCH", "httpbin.org/patch")]
    [InlineData("DELETE", "httpbin.org/delete")]
    public async Task Request_WithTwoHttpsUrl_ShouldThrowException(string method, string url)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        Func<Task> act = async () => await builder.UseHttps(url).UseHttps(url).ExecuteAsync();

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData("GET", "httpbin.org/get")]
    [InlineData("POST", "httpbin.org/post")]
    [InlineData("PUT", "httpbin.org/put")]
    [InlineData("PATCH", "httpbin.org/patch")]
    [InlineData("DELETE", "httpbin.org/delete")]
    public async Task Request_WithTwoHttpUrl_ShouldThrowException(string method, string url)
    {
        var builder = method switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
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
        var httpMethod = new HttpMethod(method);

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

        var response = await clientBuilder.ExecuteAsync();

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
    public void Request_WithInvalidHeader_ShouldThrowArgumentException(string methodName, string headerName, string headerValue)
    {
        var method = new HttpMethod(methodName);

        var clientBuilder = methodName switch
        {
            "GET" => Client.Get(),
            "POST" => Client.Post(),
            "PUT" => Client.Put(),
            "PATCH" => Client.Patch(),
            "DELETE" => Client.Delete(),
            _ => throw new NotSupportedException()
        };

        Action act = () => clientBuilder
            .UseHttps("httpbin.org/anything")
            .AddHeader(headerName, headerValue);

        act.Should().Throw<ArgumentException>();
    }
}
