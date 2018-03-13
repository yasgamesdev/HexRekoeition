using System.Collections;
using System.Collections.Generic;

public class HexCell
{
    public Terrain terrain { get; private set; }

    public HexCell(Terrain terrain)
    {
        this.terrain = terrain;
    }
}
