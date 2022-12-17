using Shoelace.Bejeweld;

public record Match
{
    public Tile[] Tiles { get; }

    public Match(params Tile[] tiles)
    {
        Tiles = tiles;
    }
}