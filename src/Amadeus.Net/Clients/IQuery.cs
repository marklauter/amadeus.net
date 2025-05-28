using LanguageExt;

namespace Amadeus.Net.Clients;

public interface IQuery
{
    Seq<KeyValuePair<string, string>> ToParams();
}
