using System;
using System.Collections.Generic;
using System.Linq;

public class Unit
{
    public bool isPlayer { get; private set; }
    public Place place { get; private set; }
    public HexDirection direction { get; private set; }
    public int moveProgress { get; private set; }

    public Unit(bool isPlayer, Place place)
    {
        this.isPlayer = isPlayer;
        this.place = place;
    }

    public void SetPlace(Place place)
    {
        this.place = place;
    }
}