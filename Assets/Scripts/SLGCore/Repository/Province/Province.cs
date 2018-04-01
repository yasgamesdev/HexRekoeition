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

    public int castleId { get; private set; }
    public int townId { get; private set; }
    public int territoryCastleId { get; private set; }

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

    public void SetCastleId(int castleId)
    {
        this.castleId = castleId;
    }

    public void SetTownId(int townId)
    {
        this.townId = townId;
    }

    public void SetTerritoryCastleId(int territoryCastleId)
    {
        this.territoryCastleId = territoryCastleId;
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
