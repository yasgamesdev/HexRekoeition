using System;
using System.Collections.Generic;
using System.Linq;

public class Place
{
    public PlaceType Type { get; private set; }

    public Place ParentPlace { get; private set; }
    public List<Place> ChildPlaces { get; private set; } = new List<Place>();

    public List<Unit> StayUnits { get; private set; } = new List<Unit>();

    public Place(PlaceType type, Place parentPlace)
    {
        Type = type;
        ParentPlace = parentPlace;
    }

    public void AddPlace(Place place)
    {
        ChildPlaces.Add(place);
    }

    public void AddStayUnit(Unit unit)
    {
        StayUnits.Add(unit);
    }

    public void RemoveStayUnit(Unit unit)
    {
        StayUnits.Remove(unit);
    }

    public bool ContainPlaceType(PlaceType childPlayType)
    {
        return ChildPlaces.Any(x => x.Type == childPlayType);
    }
}