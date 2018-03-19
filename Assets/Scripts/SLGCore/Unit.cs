using System;
using System.Collections.Generic;
using System.Linq;

public class Unit
{
    public Place place { get; private set; }
    public HexDirection direction { get; private set; }
    public int moveProgress { get; private set; }
}