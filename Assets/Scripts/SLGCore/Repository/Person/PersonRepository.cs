using System;
using System.Collections.Generic;
using System.Linq;

public class PersonRepository : Repository
{
    private static PersonRepository instance = new PersonRepository();

    public static PersonRepository Instance
    {
        get
        {
            return instance;
        }
    }

    private PersonRepository()
    {
    }

    public Person GetPerson(int personId)
    {
        return (Person)GetRepositoryData(personId);
    }
}