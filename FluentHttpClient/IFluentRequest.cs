namespace FluentHttpClient;
/// <summary>
/// Defines a fluent interface for building and executing HTTP requests.
/// </summary>
public interface IFluentRequest
{
    /// <summary>
    /// Sets the URI for the HTTP request by prepending "https://" to the provided URL string. 
    /// </summary>
    /// <param name="urlWithoutScheme">The URL without a scheme (e.g. "example.com/api").</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    /// /// <exception cref="InvalidOperationException">
    /// Thrown if the URL has already been set. Only one of <c>UseHttp</c> or <c>UseHttps</c> may be called.
    /// </exception>
    IFluentRequest UseHttps(string urlWithoutScheme);

    /// <summary>
    /// Sets the URI for the HTTP request by prepending "http://" to the provided URL string. 
    /// </summary>
    /// <param name="urlWithoutScheme">The URL without a scheme (e.g. "example.com/api").</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    /// /// <exception cref="InvalidOperationException">
    /// Thrown if the URL has already been set. Only one of <c>UseHttp</c> or <c>UseHttps</c> may be called.
    /// </exception>
    IFluentRequest UseHttp(string urlWithoutScheme);

    /// <summary>
    /// Adds a header to the HTTP request.
    /// </summary>
    /// <param name="name">The name of the header.</param>
    /// <param name="value">The value of the header.</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    IFluentRequest AddHeader(string name, string value);

    /// <summary>
    /// Sets the request body using a raw <see cref="HttpContent"/> instance.
    /// Can only be called once per request.
    /// </summary>
    /// <param name="content">The content to send in the request body.</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    IFluentRequest WithBody(HttpContent content);

    /// <summary>
    /// Sets the request body from a string with an optional media type.
    /// Can only be called once per request.
    /// </summary>
    /// <param name="content">The string content to send in the request body.</param>
    /// <param name="mediaType">The media type (e.g., "application/json", "text/plain"). Defaults to "text/plain".</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    IFluentRequest WithBody(string content, string mediaType = "text/plain");

    /// <summary>
    /// Serializes the specified model to JSON and sets it as the request body with "application/json" media type.
    /// Can only be called once per request.
    /// </summary>
    /// <typeparam name="T">The type of the model to serialize.</typeparam>
    /// <param name="model">The model to serialize and send as JSON.</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    IFluentRequest WithBody<T>(T model);

    /// <summary>
    /// Adds a query parameter to the Uri once the request is executed.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with an <see cref="HttpResponseMessage"/> result.</returns>
    IFluentRequest AddQueryParam(string name, string value);

    /// <summary>
    /// Sends the HTTP request asynchronously and returns the response.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with an <see cref="HttpResponseMessage"/> result.</returns>
    Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken = default);
}

