using LanguageExt;
using LanguageExt.Traits;
using System.Diagnostics.CodeAnalysis;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

public sealed record AirlineCodeFilter(Seq<string> Codes)
    : Monoid<AirlineCodeFilter>
{
    [SuppressMessage("Style", "IDE0301:Simplify collection initialization")]
    public static AirlineCodeFilter Empty { get; } =
        new(Seq<string>.Empty);

    public static AirlineCodeFilter From(Seq<string> codes) =>
        new(codes);

    public AirlineCodeFilter Add(string code) =>
        new(Codes.Add(code));

    public AirlineCodeFilter Combine(AirlineCodeFilter rhs) =>
        new(Codes.Combine(rhs.Codes));
}
