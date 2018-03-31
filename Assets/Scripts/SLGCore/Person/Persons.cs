using System;
using System.Collections.Generic;
using System.Linq;

public class Persons
{
    World world;

    public Person playerPerson { get; private set; }
    List<Person> persons = new List<Person>();

    public void SetReference(World world)
    {
        this.world = world;
    }

    public Person AddDaimyo(string name, CastleOrTownBase homeCastleOrTown)
    {
        Person person = new Person(false, name, PersonStatus.Daimyo, null, null, homeCastleOrTown, null);
        person.daimyo = person;

        persons.Add(person);

        return person;
    }
}