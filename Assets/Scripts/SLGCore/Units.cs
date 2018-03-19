using System;
using System.Collections.Generic;
using System.Linq;

public class Units
{
    public List<Person> persons = new List<Person>();

    public Person CreatePerson(bool isPlayer, Place place)
    {
        Person person = new Person(isPlayer, place);

        if(place != null)
        {
            place.AddUnit(person);
        }

        persons.Add(person);

        return person;
    }
}