using System;
using System.Collections.Generic;
using System.Linq;

public class House : Place
{
    public Person OwnerPerson { get; private set; }

    public House(Person ownerPerson, Place parentPlace) : base(PlaceType.House, parentPlace)
    {
        OwnerPerson = ownerPerson;
    }
}