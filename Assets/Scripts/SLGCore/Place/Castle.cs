using System;
using System.Collections.Generic;
using System.Linq;

public class Castle : CastleOrTownBase
{
    public Castle(Place parentPlace) : base(PlaceType.Castle, parentPlace)
    {
    }
}