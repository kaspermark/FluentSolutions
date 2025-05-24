using System.Text;
using System.Text.Json;

namespace FluentHttpClient;
public class FluentRequest : IFluentRequest
{
    private readonly HttpClient _httpClient;
    private readonly HttpRequestMessage _request;
    private bool _bodySet = false;

    public FluentRequest(HttpClient httpClient, HttpMethod method)
    {
        _httpClient = httpClient;
        _request = new HttpRequestMessage(method, "");
    }

    public IFluentRequest Url(string url)
    {
        _request.RequestUri = new Uri(url);
        return this;
    }

    public IFluentRequest WithHeader(string name, string value)
    {
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
        return await _httpClient.SendAsync(_request);
    }

    private void EnsureBodyNotSet()
    {
        if (_bodySet)
            throw new InvalidOperationException("WithBody can only be called once.");
    }
}
