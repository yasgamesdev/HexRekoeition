using System;
using System.Collections.Generic;
using System.Linq;

public class Town : Place
{
    public Town(Place parent) : base(PlaceType.Town, parent)
    {
    }
}