using System;
using System.Collections.Generic;
using System.Linq;

public class CastleOrTownBase : Place
{
    public CastleOrTownBase(PlaceType type, Place parentPlace) : base(type, parentPlace)
    {
    }

    public void AddHouse(Person ownerPerson)
    {
        AddPlace(new House(ownerPerson, this));
    }

    public void RemoveHouse(Person ownerPerson)
    {
        ChildPlaces.RemoveAll(x => x.Type == PlaceType.House && ((House)x).OwnerPerson == ownerPerson);
    }

    public House GetHouse(Person ownerPerson)
    {
        return (House)ChildPlaces.First(x => x.Type == PlaceType.House && ((House)x).OwnerPerson == ownerPerson);
    }
}