using Amadeus.Net.Clients.Response;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

/// <summary>
/// Defines the signature for an operation that can be executed by an <see cref="Endpoint{R, F}"/>.
/// </summary>
/// <typeparam name="F">The type of the filter or parameters for the operation.</typeparam>
/// <typeparam name="R">The type of the successful domain result from the operation.</typeparam>
/// <param name="filter">The filter or parameters for the operation.</param>
/// <returns>An <see cref="IO{A}"/> that, when run, will execute the operation and produce an <see cref="Either{L, R}"/> of <see cref="ErrorResponse"/> or <typeparamref name="R"/>.</returns>
public delegate IO<Either<ErrorResponse, R>> EndpointOperation<in F, R>(F filter);

/// <summary>
/// Represents a configurable API endpoint operation.
/// </summary>
/// <typeparam name="F">The type of the filter or parameters for the operation.</typeparam>
/// <typeparam name="R">The type of the successful domain result from the operation.</typeparam>
public sealed class Endpoint<F, R>(EndpointOperation<F, R> operation)
{
    private readonly EndpointOperation<F, R> operation = operation
        ?? throw new ArgumentNullException(nameof(operation));

    public IO<Either<ErrorResponse, R>> Filter(F filter) => operation(filter);
}
