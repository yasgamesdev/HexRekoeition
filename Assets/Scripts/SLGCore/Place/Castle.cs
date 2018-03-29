using System;
using System.Collections.Generic;
using System.Linq;

public class Castle : CastleOrTownBase
{
    List<Castle> neighboringCastles = new List<Castle>();

    public Castle(Place parentPlace) : base(PlaceType.Castle, parentPlace)
    {
    }

    public void AddNeighboringCastles(List<Castle> neighboringCastles)
    {
        this.neighboringCastles.AddRange(neighboringCastles);
    }

    public List<Castle> GetNeighboringCastles()
    {
        return neighboringCastles;
    }
}