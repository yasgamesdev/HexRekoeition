using System;
using System.Collections.Generic;
using System.Linq;

public class SLGCore
{
    World world;
    Persons persons;
    Forces forces;

    public SLGCore(int width, int height)
    {
        world = new World(width, height);
        persons = new Persons();
        forces = new Forces();

        persons.SetReference(world);
    }

    public World GetWorld()
    {
        return world;
    }
}