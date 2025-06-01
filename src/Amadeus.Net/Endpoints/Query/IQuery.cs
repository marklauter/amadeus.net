using LanguageExt;

namespace Amadeus.Net.Endpoints.Query;

public interface IQuery
{
    Seq<QueryParameter> ToParams();
}
