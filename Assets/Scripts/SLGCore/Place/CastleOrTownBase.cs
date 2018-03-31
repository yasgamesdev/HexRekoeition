using System;
using System.Collections.Generic;
using System.Linq;

public class CastleOrTownBase : Place
{
    public CastleOrTownBase(PlaceType type, Place parentPlace) : base(type, parentPlace)
    {
    }

    //public House AddHouse(Person ownerPerson)
    //{
    //    House house = new House(ownerPerson, this);
    //    AddPlace(house);

    //    return house;
    //}

    //public void RemoveHouse(Person ownerPerson)
    //{
    //    ChildPlaces.RemoveAll(x => x.Type == PlaceType.House && ((House)x).OwnerPerson == ownerPerson);
    //}

    public House GetHouse(Person ownerPerson)
    {
        return (House)ChildPlaces.First(x => x.Type == PlaceType.House && ((House)x).OwnerPerson == ownerPerson);
    }
}