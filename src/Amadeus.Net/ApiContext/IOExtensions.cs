using Amadeus.Net.Clients.Response;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public static class IOExtensions
{
    public static ValueTask<Either<ErrorResponse, R>> GetAsync<R>(
        this IO<Either<ErrorResponse, R>> operation,
        CancellationToken cancellationToken) =>
            operation.Fork().Await().RunAsync(EnvIO.New(token: cancellationToken));
}
