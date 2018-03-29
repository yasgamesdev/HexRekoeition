using System;
using System.Collections.Generic;
using System.Linq;

public class Castle : CastleOrTownBase
{
    List<Province> territoryProvinces = new List<Province>();
    List<Castle> neighboringCastles = new List<Castle>();

    public Castle(Place parentPlace) : base(PlaceType.Castle, parentPlace)
    {
    }

    public void AddTerritoryProvince(Province province)
    {
        territoryProvinces.Add(province);
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