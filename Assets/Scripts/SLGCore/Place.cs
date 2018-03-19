using System;
using System.Collections.Generic;
using System.Linq;

public class Place
{
    public PlaceType type { get; private set; }

    public Place parent { get; private set; }
    public List<Place> children { get; private set; } = new List<Place>();

    public List<Unit> units { get; private set; } = new List<Unit>();

    public Place(PlaceType type, Place parent)
    {
        this.type = type;
        this.parent = parent;
    }

    public void AddChild(Place child)
    {
        children.Add(child);
    }
}