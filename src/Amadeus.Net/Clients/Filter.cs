using LanguageExt;

namespace Amadeus.Net.Clients;

public interface IFilter
{
    Seq<KeyValuePair<string, string>> AsQuery();
}
