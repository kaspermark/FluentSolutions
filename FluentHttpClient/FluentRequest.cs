using System.Text;
using System.Text.Json;

namespace FluentHttpClient;
public class FluentRequest(HttpClient httpClient, HttpMethod method) : IFluentRequest
{
    private readonly HttpRequestMessage _request = new(method, "");
    private bool _bodySet = false;
    private bool _urlSet = false;

    public IFluentRequest UseHttps(string urlWithoutScheme)
    {
        EnsureUrlNotSet();
        var url = "https://" + urlWithoutScheme;
        _request.RequestUri = new Uri(url);
        _urlSet = true;
        return this;
    }

    public IFluentRequest UseHttp(string urlWithoutScheme)
    {
        EnsureUrlNotSet();
        var url = "http://" + urlWithoutScheme;
        _request.RequestUri = new Uri(url);
        _urlSet = true;
        return this;
    }

    public IFluentRequest AddHeader(string name, string value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Header name must not be null, empty or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Header value must not be null, empty or whitespace.", nameof(value));
        }

        _request.Headers.TryAddWithoutValidation(name, value);
        return this;
    }

    public IFluentRequest WithBody(HttpContent content)
    {
        EnsureBodyNotSet();
        _request.Content = content;
        _bodySet = true;
        return this;
    }

    public IFluentRequest WithBody(string content, string mediaType = "text/plain")
    {
        var httpContent = new StringContent(content, Encoding.UTF8, mediaType);
        return WithBody((HttpContent)httpContent);
    }

    public IFluentRequest WithBody<T>(T model)
    {
        var json = JsonSerializer.Serialize(model);
        return WithBody(json, "application/json");
    }

    public async Task<HttpResponseMessage> ExecuteAsync()
    {
        return await httpClient.SendAsync(_request);
    }

    private void EnsureBodyNotSet()
    {
        if (_bodySet)
            throw new InvalidOperationException("WithBody can only be called once.");

        if (_request.Method == HttpMethod.Get)
            throw new InvalidOperationException($"HTTP method {_request.Method} does not support a request body.");
    }

    private void EnsureUrlNotSet()
    {
        if (_urlSet)
            throw new InvalidOperationException("URL can only be set once, via Url(), UseHttp() or UseHttps().");
    }
}
