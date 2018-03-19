using System;
using System.Collections.Generic;
using System.Linq;

public class Castle : Place
{
    public Castle(Place parent) : base(PlaceType.Castle, parent)
    {
    }
}