using System;
using System.Collections.Generic;
using System.Linq;

public class Faction : RepositoryData
{
    public string Name { get; private set; }

    public Faction(string name, Repository repository) :base(repository)
    {
        Name = name;
    }
}