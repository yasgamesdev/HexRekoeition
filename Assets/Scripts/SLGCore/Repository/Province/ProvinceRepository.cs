using System;
using System.Collections.Generic;
using System.Linq;

public class ProvinceRepository : Repository
{
    private static ProvinceRepository instance = new ProvinceRepository();

    public static ProvinceRepository Instance
    {
        get
        {
            return instance;
        }
    }

    private ProvinceRepository()
    {
    }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public void GenerateProvince(int width, int height, float size)
    {
        Width = width;
        Height = height;

        Random rand = new Random();

        float[,] noise = NoiseGenerator.Generate(Width, Height, size, size * 0.75f, (float)rand.NextDouble() * 1024, (float)rand.NextDouble() * 1024);

        for (int z = 0; z < noise.GetLength(1); z++)
        {
            for (int x = 0; x < noise.GetLength(0); x++)
            {
                Province province = new Province(x, z, Width, this);
                province.SetTerrainType(noise[x, z] >= 0 ? TerrainType.Land : TerrainType.Sea);
            }
        }
    }

    public Province GetProvince(int provinceId)
    {
        return (Province)GetRepositoryData(provinceId);
    }

    public Province GetProvince(int x, int z)
    {
        return (Province)GetRepositoryData(x + z * Width);
    }

    public List<Province> GetAllProvince()
    {
        return GetAllRepositoryData().Cast<Province>().ToList();
    }

    public List<Province> GetAllLandAndNoneProvince()
    {
        return GetAllRepositoryData().Cast<Province>().Where(x => x.TerrainType == TerrainType.Land && x.ProvinceType == ProvinceType.None).ToList();
    }

    public void GenerateRoad(int range, int diff)
    {
        GenerateMainRoad();
        GenerateSubRoad(range, diff);
    }

    void GenerateMainRoad()
    {
        List<Province> castleOrTownProvinces = GetAllRepositoryData().Cast<Province>().Where(x => x.ProvinceType == ProvinceType.Castle || x.ProvinceType == ProvinceType.Town).ToList();

        List<Province> setRoadProvinces = new List<Province>();
        setRoadProvinces.Add(castleOrTownProvinces[0]);

        while (true)
        {
            if (setRoadProvinces.Count == castleOrTownProvinces.Count)
            {
                break;
            }
            else
            {
                List<Province> notSetRoadProvinces = new List<Province>();
                for (int i = 0; i < castleOrTownProvinces.Count; i++)
                {
                    if (!setRoadProvinces.Contains(castleOrTownProvinces[i]))
                    {
                        notSetRoadProvinces.Add(castleOrTownProvinces[i]);
                    }
                }

                int minDistance = int.MaxValue;
                Province minSetRoadProvince = null, minNotSetRoadProvince = null;

                foreach (Province fromProvince in notSetRoadProvinces)
                {
                    foreach (Province toProvince in setRoadProvinces)
                    {
                        int distance = Province.GetDistance(fromProvince, toProvince);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            minNotSetRoadProvince = fromProvince;
                            minSetRoadProvince = toProvince;
                        }
                    }
                }

                setRoadProvinces.Add(minNotSetRoadProvince);

                foreach(Province province in HexPathFinder.GetPath(minNotSetRoadProvince, minSetRoadProvince))
                {
                    if(province.ProvinceType == ProvinceType.None)
                    {
                        province.SetProvinceType(ProvinceType.Road);
                    }
                }
            }
        }
    }

    void GenerateSubRoad(int range, int diff)
    {
        List<Province> castleOrTownProvinces = GetAllRepositoryData().Cast<Province>().Where(x => x.ProvinceType == ProvinceType.Castle || x.ProvinceType == ProvinceType.Town).ToList();

        foreach (Province province in castleOrTownProvinces)
        {
            List<Province> closeProvinces = castleOrTownProvinces.Where(x => 0 < Province.GetDistance(x, province) && Province.GetDistance(x, province) <= range).ToList();

            foreach (Province closeProvince in closeProvinces)
            {
                int distance = Province.GetDistance(province, closeProvince);

                List<Province> path = HexPathFinder.GetPath(province, closeProvince);
                int cost = path.Sum(x => x.GetMovementCost());

                if (cost - distance >= diff)
                {
                    foreach (Province _province in path)
                    {
                        if (_province.ProvinceType == ProvinceType.None)
                        {
                            _province.SetProvinceType(ProvinceType.Road);
                        }
                    }
                }
            }
        }
    }

    public void SetTerritory()
    {
        List<Province> provinces = GetAllProvince();
        List<Province> castleProvinces = provinces.Where(x => x.ProvinceType == ProvinceType.Castle).ToList();

        int range = 0;

        while (provinces.Count > 0)
        {
            foreach (Province province in castleProvinces)
            {
                List<Province> closeProvinces = provinces.Where(x => range - 1 < Province.GetDistance(x, province) && Province.GetDistance(x, province) <= range).ToList();
                foreach (Province closeProvince in closeProvinces)
                {
                    closeProvince.SetTerritoryCastle(province.GetCastle());
                    province.GetCastle().AddTerritoryProvince(closeProvince);

                    provinces.Remove(closeProvince);
                }
            }

            range++;
        }
    }

    public void SetNeighboringCastles()
    {
        List<Province> provinces = GetAllProvince();
        List<Province> roadProvinces = provinces.Where(x => x.ProvinceType != ProvinceType.None).ToList();

        foreach (Province roadProvince in roadProvinces)
        {
            CheckBoarder(roadProvince, roadProvince.x + (roadProvince.z % 2), roadProvince.z + 1);
            CheckBoarder(roadProvince, roadProvince.x + 1, roadProvince.z);
            CheckBoarder(roadProvince, roadProvince.x + (roadProvince.z % 2), roadProvince.z - 1);
            CheckBoarder(roadProvince, roadProvince.x - ((roadProvince.z + 1) % 2), roadProvince.z - 1);
            CheckBoarder(roadProvince, roadProvince.x - 1, roadProvince.z);
            CheckBoarder(roadProvince, roadProvince.x - ((roadProvince.z + 1) % 2), roadProvince.z + 1);
        }
    }

    void CheckBoarder(Province roadProvince, int x, int z)
    {
        if (0 <= x && x < Width && 0 <= z && z < Height)
        {
            Province boarderProvince = GetProvince(x, z);

            if (boarderProvince.ProvinceType != ProvinceType.None && roadProvince.GetTerritoryCastle() != boarderProvince.GetTerritoryCastle())
            {
                roadProvince.GetTerritoryCastle().AddUniqueNeighboringCastle(boarderProvince.GetTerritoryCastle());
                boarderProvince.GetTerritoryCastle().AddUniqueNeighboringCastle(roadProvince.GetTerritoryCastle());
            }
        }
    }
}