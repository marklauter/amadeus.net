namespace Amadeus.Net.Clients.FlightInspiration;

public sealed record DepartureDateSelection
{
    public DateOnly Start { get; init; }
    public DateOnly? End { get; init; }

    public DepartureDateSelection(DateOnly start, DateOnly? end = null)
    {
        Start = start;
        End = end;
    }

    public override string ToString() => End is not null ? $"{Start:yyyy-MM-dd},{End:yyyy-MM-dd}" : Start.ToString("yyyy-MM-dd");
}


