using System;
using System.Collections.Generic;
using System.Linq;

public class Units
{
    List<Person> persons = new List<Person>();

    public Person CreatePerson(bool isPlayer, CastleOrTownBase homeCastleOrTown, Place curPlace)
    {
        Person person = new Person(isPlayer, homeCastleOrTown, curPlace);

        persons.Add(person);

        return person;
    }

    public void ProgressQuarterDay()
    {
        persons.ForEach(x => x.ExecCommand());
    }

    public Person GetPlayerPerson()
    {
        return persons.First(x => x.IsPlayer);
    }
}