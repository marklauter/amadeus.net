using LanguageExt;

namespace Amadeus.Net.Clients;

public interface IQuery
{
    Seq<QueryParameter> ToParams();
}
