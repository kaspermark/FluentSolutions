namespace FluentHttpClient;
/// <summary>
/// Defines a fluent interface for building and executing HTTP requests.
/// </summary>
public interface IFluentRequest
{
    /// <summary>
    /// Sets the URL for the HTTP request.
    /// </summary>
    /// <param name="url">The URL to send the request to.</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    IFluentRequest Url(string url);

    /// <summary>
    /// Adds a header to the HTTP request.
    /// </summary>
    /// <param name="name">The name of the header.</param>
    /// <param name="value">The value of the header.</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    IFluentRequest WithHeader(string name, string value);

    /// <summary>
    /// Sets the request body using a raw <see cref="HttpContent"/> instance.
    /// Can only be called once per request.
    /// </summary>
    /// <param name="content">The content to send in the request body.</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    IFluentRequest WithBody(HttpContent content);

    /// <summary>
    /// Sets the request body from a string with an optional media type.
    /// Can only be called once per request.
    /// </summary>
    /// <param name="content">The string content to send in the request body.</param>
    /// <param name="mediaType">The media type (e.g., "application/json", "text/plain"). Defaults to "text/plain".</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    IFluentRequest WithBody(string content, string mediaType = "text/plain");

    /// <summary>
    /// Serializes the specified model to JSON and sets it as the request body with "application/json" media type.
    /// Can only be called once per request.
    /// </summary>
    /// <typeparam name="T">The type of the model to serialize.</typeparam>
    /// <param name="model">The model to serialize and send as JSON.</param>
    /// <returns>The current <see cref="IFluentRequest"/> instance for method chaining.</returns>
    IFluentRequest WithBody<T>(T model);

    /// <summary>
    /// Sends the HTTP request asynchronously and returns the response.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with an <see cref="HttpResponseMessage"/> result.</returns>
    Task<HttpResponseMessage> ExecuteAsync();
}

