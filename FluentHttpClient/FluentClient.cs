namespace FluentHttpClient;
/// <summary>
/// A fluent HTTP client that simplifies building and sending HTTP requests.
/// Wraps an <see cref="HttpClient"/> instance and provides fluent methods for HTTP operations.
/// </summary>
public class FluentClient(HttpClient httpClient) : IFluentClient
{
    public IFluentRequest Get()
    {
        return new FluentRequest(httpClient, HttpMethod.Get);
    }

    public IFluentRequest Post()
    {
        return new FluentRequest(httpClient, HttpMethod.Post);
    }

    public IFluentRequest Delete()
    {
        return new FluentRequest(httpClient, HttpMethod.Delete);
    }

    public IFluentRequest Patch()
    {
        return new FluentRequest(httpClient, HttpMethod.Patch);
    }

    public IFluentRequest Put()
    {
        return new FluentRequest(httpClient, HttpMethod.Put);
    }

    public IFluentRequest Head()
    {
        return new FluentRequest(httpClient, HttpMethod.Head);
    }

    public IFluentRequest Options()
    {
        return new FluentRequest(httpClient, HttpMethod.Options);
    }
}
