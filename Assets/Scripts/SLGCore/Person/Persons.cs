using System;
using System.Collections.Generic;
using System.Linq;

public class Persons
{
    public Person playerPerson { get; private set; }
    List<Person> persons = new List<Person>();

    public void AddPerson(Person person)
    {
        persons.Add(person);

        if(person.isPlayer)
        {
            playerPerson = person;
        }
    }
}