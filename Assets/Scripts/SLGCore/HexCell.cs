using System;
using System.Collections;
using System.Collections.Generic;

public class HexCell
{
    public int x { get; private set; }
    public int z { get; private set; }
    public int i { get; private set; }

    public int X
    {
        get
        {
            return x - z / 2;
        }
    }

    public int Z
    {
        get
        {
            return z;
        }
    }

    public int Y
    {
        get
        {
            return -X - Z;
        }
    }

    public Terrain terrain { get; private set; }
    public Building building { get; private set; }

    public HexCell(int x, int z, int width)
    {
        this.x = x;
        this.z = z;
        this.i = x + z * width;
    }

    public void SetTerrain(Terrain terrain)
    {
        this.terrain = terrain;
    }

    public void SetBuilding(Building building)
    {
        this.building = building;
    }

    public static int GetDistance(HexCell fromCell, HexCell toCell)
    {
        int dX = Math.Abs(fromCell.X - toCell.X);
        int dY = Math.Abs(fromCell.Y - toCell.Y);
        int dZ = Math.Abs(fromCell.Z - toCell.Z);

        return (dX + dY + dZ) / 2;
    }
}
