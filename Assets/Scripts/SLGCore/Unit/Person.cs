using System;
using System.Collections.Generic;
using System.Linq;

public class Person : Unit
{
    public bool IsPlayer { get; private set; }
    public CastleOrTownBase HomeCastleOrTown { get; private set; }

    public Person(bool isPlayer, CastleOrTownBase homeCastleOrTown, Place curPlace) : base(curPlace)
    {
        IsPlayer = isPlayer;
        SetHomePlace(homeCastleOrTown);
    }

    public void SetHomePlace(CastleOrTownBase homeCastleOrTown)
    {
        if (HomeCastleOrTown != null)
        {
            HomeCastleOrTown.RemoveHouse(this);
        }

        HomeCastleOrTown = homeCastleOrTown;

        HomeCastleOrTown.AddHouse(this);
    }

    protected override void Abort()
    {
        base.Abort();

        SetCurPlace(HomeCastleOrTown.GetHouse(this));
    }
}