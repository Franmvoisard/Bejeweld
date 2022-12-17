using System.Collections.Generic;
using System.Text;
using Shoelace.Bejeweld;

public record Match
{
    public readonly List<Tile> Tiles = new List<Tile>();

    public void Append(params Tile[] tiles)
    {
        Tiles.AddRange(tiles);
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"Match of id {Tiles[0].TypeId}: (");
        foreach (var tile in Tiles)
        {
            stringBuilder.Append($"[ {tile.TypeId} ]");
        }
        stringBuilder.Append(")");
        return stringBuilder.ToString();
    }
}