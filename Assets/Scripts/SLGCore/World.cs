using System;
using System.Collections.Generic;
using System.Linq;

public class World : Place
{
    public int width { get; private set; }
    public int height { get; private set; }

    public World(int width, int height) : base(PlaceType.World, null)
    {
        this.width = width;
        this.height = height;

        float[,] noise = NoiseGenerator.Generate(width, height, 6.0f, 6.0f * 0.75f, 2.0f, 1.0f);

        for (int z = 0; z < noise.GetLength(1); z++)
        {
            for (int x = 0; x < noise.GetLength(0); x++)
            {
                Province province = new Province(x, z, width, this);
                province.SetTerrain(noise[x, z] >= 0 ? TerrainType.Land : TerrainType.Sea);
                children.Add(province);
            }
        }

        SetBuilding();
    }

    public Province GetProvince(int x, int z)
    {
        return (Province)children[x + z * width];
    }

    void SetBuilding()
    {
        SetCastle(120, 4);

        SetTown(160, 3);

        SetRoad();

        SetSecondaryRoad(10, 6);
    }

    void SetCastle(int castleCount, int minDistance)
    {
        List<Province> provinces = children.Cast<Province>().ToList();
        List<Province> possibleProvinces = provinces.Where(x => x.terrain == TerrainType.Land).ToList();

        Random rand = new Random();

        for(int i=0; i<castleCount; i++)
        {
            if(possibleProvinces.Count == 0)
            {
                break;
            }
            else
            {
                int index = rand.Next(possibleProvinces.Count);
                Province province = possibleProvinces[index];
                province.AddChild(new Castle(province));
                possibleProvinces.RemoveAll(x => Province.GetDistance(province, x) <= minDistance);
            }
        }
    }

    void SetTown(int townCount, int minDistance)
    {
        List<Province> provinces = children.Cast<Province>().ToList();
        List<Province> possibleProvinces = provinces.Where(x => x.terrain == TerrainType.Land && x.children.Count == 0).ToList();

        Random rand = new Random();

        for (int i = 0; i < townCount; i++)
        {
            if (possibleProvinces.Count == 0)
            {
                break;
            }
            else
            {
                int index = rand.Next(possibleProvinces.Count);
                Province province = possibleProvinces[index];
                province.AddChild(new Town(province));
                possibleProvinces.RemoveAll(x => Province.GetDistance(province, x) <= minDistance);
            }
        }
    }

    void SetRoad()
    {
        List<Province> provinces = children.Cast<Province>().ToList();
        List<Province> castleOrTownProvinces = provinces.Where(x => x.children.Count > 0).ToList();

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

                foreach(Province fromProvince in notSetRoadProvinces)
                {
                    foreach(Province toProvince in setRoadProvinces)
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

                HexPathFinder.GetPath(minNotSetRoadProvince, minSetRoadProvince, width, height, provinces).ForEach(x => x.SetIsRoad(true));
            }
        }
    }

    void SetSecondaryRoad(int range, int diff)
    {
        List<Province> provinces = children.Cast<Province>().ToList();
        List<Province> castleOrTownProvinces = provinces.Where(x => x.children.Count > 0).ToList();

        foreach (Province province in castleOrTownProvinces)
        {
            List<Province> closeProvinces = castleOrTownProvinces.Where(x => 0 < Province.GetDistance(x, province) && Province.GetDistance(x, province) <= range).ToList();

            foreach (Province closeProvince in closeProvinces)
            {
                int distance = Province.GetDistance(province, closeProvince);

                List<Province> path = HexPathFinder.GetPath(province, closeProvince, width, height, provinces);
                int cost = HexPathFinder.GetCost(path);

                if (cost - distance >= diff)
                {
                    path.ForEach(x => x.SetIsRoad(true));
                }
            }
        }
    }
}
