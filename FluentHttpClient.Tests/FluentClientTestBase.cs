using Moq;
using Moq.Protected;

namespace FluentHttpClient.Tests;
public abstract class FluentClientTestBase
{
    protected HttpClient CreateMockedHttpClient(Func<HttpRequestMessage, bool> match, HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => match(req)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response)
            .Verifiable();

        return new HttpClient(handlerMock.Object);
    }
}
