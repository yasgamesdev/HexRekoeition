using System;
using System.Collections.Generic;
using System.Linq;

public class Units
{
    public List<Person> persons = new List<Person>();

    public Person CreatePerson(bool isPlayer, CastleOrTownBase homeCastleOrTown, Place curPlace)
    {
        Person person = new Person(isPlayer, homeCastleOrTown, curPlace);

        persons.Add(person);

        return person;
    }
}