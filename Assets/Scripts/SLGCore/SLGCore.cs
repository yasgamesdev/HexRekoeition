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

        units.CreatePerson(true, world.children.Cast<Province>().ToList().Where(x => x.children.Count > 0).ToList()[20]);
    }

    public World GetWorld()
    {
        return world;
    }
}