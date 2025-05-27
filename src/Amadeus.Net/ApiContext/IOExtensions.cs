using Amadeus.Net.Clients.Response;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public static class IOExtensions
{
    public static ValueTask<Either<ErrorResponse, R>> InvokeAsync<R>(
        this IO<Either<ErrorResponse, R>> operation,
        CancellationToken cancellationToken) =>
            operation.RunAsync(EnvIO.New(token: cancellationToken));
}
