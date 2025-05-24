namespace FluentHttpClient;
public interface IFluentClient
{
    /// <summary>
    /// Creates a new HTTP GET request.
    /// </summary>
    /// <returns>An instance of <see cref="IFluentRequest"/> representing the GET request.</returns>
    IFluentRequest Get();

    /// <summary>
    /// Creates a new HTTP POST request.
    /// </summary>
    /// <returns>An instance of <see cref="IFluentRequest"/> representing the POST request.</returns>
    IFluentRequest Post();

    /// <summary>
    /// Creates a new HTTP DELETE request.
    /// </summary>
    /// <returns>An instance of <see cref="IFluentRequest"/> representing the DELETE request.</returns>
    IFluentRequest Delete();

    /// <summary>
    /// Creates a new HTTP PATCH request.
    /// </summary>
    /// <returns>An instance of <see cref="IFluentRequest"/> representing the PATCH request.</returns>
    IFluentRequest Patch();

    /// <summary>
    /// Creates a new HTTP PUT request.
    /// </summary>
    /// <returns>An instance of <see cref="IFluentRequest"/> representing the PUT request.</returns>
    IFluentRequest Put();

    /// <summary>
    /// Creates a new HTTP HEAD request.
    /// </summary>
    /// <returns>An instance of <see cref="IFluentRequest"/> representing the HEAD request.</returns>
    IFluentRequest Head();

    /// <summary>
    /// Creates a new HTTP OPTIONS request.
    /// </summary>
    /// <returns>An instance of <see cref="IFluentRequest"/> representing the OPTIONS request.</returns>
    IFluentRequest Options();
}
