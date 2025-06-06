﻿using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace FluentHttpClient;
public class FluentRequest(HttpClient httpClient, HttpMethod method) : IFluentRequest
{
    private readonly HttpRequestMessage _request = new(method, "");
    private readonly QueryBuilder _queryBuilder = [];

    public IFluentRequest UseHttps(string urlWithoutScheme)
    {
        EnsureUrlNotSet();
        var url = "https://" + urlWithoutScheme;
        _request.RequestUri = new Uri(url);
        return this;
    }

    public IFluentRequest UseHttp(string urlWithoutScheme)
    {
        EnsureUrlNotSet();
        var url = "http://" + urlWithoutScheme;
        _request.RequestUri = new Uri(url);
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
        return this;
    }

    public IFluentRequest WithBody(string content, string mediaType = "text/plain")
    {
        var httpContent = new StringContent(content, Encoding.UTF8, mediaType);
        return WithBody((HttpContent)httpContent);
    }
    public IFluentRequest WithBody<T>(T model)
    {
        var json = JsonConvert.SerializeObject(model);
        return WithBody(json, "application/json");
    }

    public IFluentRequest AddQueryParam(string name, string value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("name must not be null, empty or whitespace.", nameof(name));
        }

        if (_request.RequestUri == null)
        {
            throw new InvalidOperationException("Url has to be set before adding a query paramater");
        }

        _queryBuilder.Add(name, value);
        return this;
    }

    private static string AppendQueryParameters(string uri, Dictionary<string, string> parameters)
    {
        var uriBuilder = new UriBuilder(uri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var parameter in parameters)
        {
            query[parameter.Key] = parameter.Value;
        }

        uriBuilder.Query = query.ToString();
        return uriBuilder.Uri.ToString();
    }

    public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (_request.RequestUri == null)
        {
            throw new InvalidOperationException("Url has to be set");
        }

        var newUrl = AppendQueryParameters(_request.RequestUri.AbsoluteUri, _queryBuilder.ToDictionary());
        _request.RequestUri = new Uri(newUrl);

        return await httpClient.SendAsync(_request, cancellationToken);
    }

    public async Task<T> ExecuteAndDeserializeAsync<T>(CancellationToken cancellationToken = default)
    {
        var response = await ExecuteAsync(cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<T>(content);

        return result == null ? throw new JsonSerializationException("Deserialization returned null.") : result;
    }

    private void EnsureBodyNotSet()
    {
        if (IsBodySet())
        {
            throw new InvalidOperationException("WithBody can only be called once.");
        }

        if (!SupportsRequestBody(_request.Method))
        {
            throw new InvalidOperationException($"HTTP method {_request.Method} does not support a request body.");
        }
    }

    private void EnsureUrlNotSet()
    {
        if (IsUrlSet())
        {
            throw new InvalidOperationException("URL can only be set once, UseHttp() or UseHttps().");
        }
    }

    private static bool SupportsRequestBody(HttpMethod method)
    {
        return method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch;
    }

    private bool IsBodySet()
    {
        return _request.Content != null;
    }

    private bool IsUrlSet()
    {
        return _request.RequestUri != null;
    }
}
