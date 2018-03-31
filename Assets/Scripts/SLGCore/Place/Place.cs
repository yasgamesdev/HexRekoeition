using System;
using System.Collections.Generic;
using System.Linq;

public class Place
{
    public PlaceType Type { get; private set; }

    public Place ParentPlace { get; private set; }
    public List<Place> ChildPlaces { get; private set; } = new List<Place>();

    public List<Person> StayPersons { get; private set; } = new List<Person>();

    public Place(PlaceType type, Place parentPlace)
    {
        Type = type;
        ParentPlace = parentPlace;
    }

    public void AddPlace(Place place)
    {
        ChildPlaces.Add(place);
    }

    public void AddStayPerson(Person person)
    {
        StayPersons.Add(person);
    }

    public void RemoveStayUnit(Person person)
    {
        StayPersons.Remove(person);
    }

    public bool ContainPlaceType(PlaceType childPlayType)
    {
        return ChildPlaces.Any(x => x.Type == childPlayType);
    }
}