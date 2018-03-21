using System;
using System.Collections.Generic;
using System.Linq;

public class Town : CastleOrTownBase
{
    public Town(Place parentPlace) : base(PlaceType.Town, parentPlace)
    {
    }
}