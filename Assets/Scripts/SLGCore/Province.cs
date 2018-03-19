using System;
using System.Collections;
using System.Collections.Generic;

public class Province : Place
{
    public int x { get; private set; }
    public int z { get; private set; }
    public int i { get; private set; }

    public TerrainType terrain { get; private set; }
    public bool isRoad { get; private set; }

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

    

    public Province(int x, int z, int width, Place parent) : base(PlaceType.Province, parent)
    {
        this.x = x;
        this.z = z;
        this.i = x + z * width;
    }

    public void SetTerrain(TerrainType terrain)
    {
        this.terrain = terrain;
    }

    public void SetIsRoad(bool isRoad)
    {
        this.isRoad = isRoad;
    }

    public static int GetDistance(Province fromProvince, Province toProvince)
    {
        int dX = Math.Abs(fromProvince.X - toProvince.X);
        int dY = Math.Abs(fromProvince.Y - toProvince.Y);
        int dZ = Math.Abs(fromProvince.Z - toProvince.Z);

        return (dX + dY + dZ) / 2;
    }
}
