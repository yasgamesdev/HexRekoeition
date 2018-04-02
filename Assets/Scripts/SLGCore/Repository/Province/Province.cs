using System;
using System.Collections;
using System.Collections.Generic;

public class Province : RepositoryData
{
    public int x { get; private set; }
    public int z { get; private set; }
    public int i { get; private set; }

    public TerrainType TerrainType { get; private set; }
    public ProvinceType ProvinceType { get; private set; }

    int castleId;
    int townId;
    int territoryCastleId;

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

    public Province(int x, int z, int width, Repository repository) : base(repository)
    {
        this.x = x;
        this.z = z;
        this.i = x + z * width;
    }

    public void SetTerrainType(TerrainType terrainType)
    {
        TerrainType = terrainType;
    }

    public void SetProvinceType(ProvinceType provinceType)
    {
        ProvinceType = provinceType;
    }

    public void SetCastle(Castle castle)
    {
        castleId = castle.Id;
    }

    public Castle GetCastle()
    {
        return CastleRepository.Instance.GetCastle(castleId);
    }

    public void SetTown(Town town)
    {
        townId = town.Id;
    }

    public Town GetTown()
    {
        return TownRepository.Instance.GetTown(townId);
    }

    public void SetTerritoryCastle(Castle territoryCastle)
    {
        territoryCastleId = territoryCastle.Id;
    }

    public Castle GetTerritoryCastle()
    {
        return CastleRepository.Instance.GetCastle(territoryCastleId);
    }

    public static int GetDistance(Province fromProvince, Province toProvince)
    {
        int dX = Math.Abs(fromProvince.X - toProvince.X);
        int dY = Math.Abs(fromProvince.Y - toProvince.Y);
        int dZ = Math.Abs(fromProvince.Z - toProvince.Z);

        return (dX + dY + dZ) / 2;
    }

    public int GetMovementCost()
    {
        if (ProvinceType != ProvinceType.None)
        {
            return TerrainType == TerrainType.Sea ? MovementCost.SeaRoute : MovementCost.LandRoute;
        }
        else
        {
            return TerrainType == TerrainType.Sea ? MovementCost.Sea : MovementCost.Land;
        }
    }
}
