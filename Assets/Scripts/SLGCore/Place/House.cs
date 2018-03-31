using System;
using System.Collections.Generic;
using System.Linq;

public class House : Place
{
    public Person OwnerPerson { get; private set; }

    public House(Place parentPlace) : base(PlaceType.House, parentPlace)
    {
    }

    public House(Person ownerPerson, Place parentPlace) : base(PlaceType.House, parentPlace)
    {
        OwnerPerson = ownerPerson;
    }

    public void SetOwnerPerson(Person ownerPerson)
    {
        OwnerPerson = ownerPerson;
    }
}