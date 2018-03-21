using System;
using System.Collections.Generic;
using System.Linq;

public class SLGCore
{
    public World world { get; private set; }
    public Units units { get; private set; }

    public SLGCore(int width, int height)
    {
        world = new World(width, height);
        units = new Units();

        Town playerTown = world.Towns[0];

        units.CreatePerson(true, playerTown, playerTown.ParentPlace);
    }

    public World GetWorld()
    {
        return world;
    }

    public void ProgressQuarterDay()
    {
        units.ProgressQuarterDay();
    }

    public Person GetPlayerPerson()
    {
        return units.GetPlayerPerson();
    }
}