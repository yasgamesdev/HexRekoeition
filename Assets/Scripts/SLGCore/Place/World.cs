using System;
using System.Collections.Generic;
using System.Linq;

public class World : Place
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public List<Castle> Castles { get; private set; } = new List<Castle>();
    public List<Town> Towns { get; private set; } = new List<Town>();

    const int castleMinDistance = 4;
    const int townMinDistance = 3;

    public World(int width, int height) : base(PlaceType.World, null)
    {
        Width = width;
        Height = height;

        SetTerrain();

        SetBuilding();
    }

    void SetTerrain()
    {
        float[,] noise = NoiseGenerator.Generate(Width, Height, 6.0f, 6.0f * 0.75f, 2.0f, 1.0f);

        for (int z = 0; z < noise.GetLength(1); z++)
        {
            for (int x = 0; x < noise.GetLength(0); x++)
            {
                Province province = new Province(x, z, Width, this);
                province.SetTerrain(noise[x, z] >= 0 ? TerrainType.Land : TerrainType.Sea);
                ChildPlaces.Add(province);
            }
        }
    }

    void SetBuilding()
    {
        SetCastle(120, castleMinDistance);

        SetTown(160, townMinDistance);

        SetRoad();

        SetSecondaryRoad(8, 12);

        SetTerritory();

        //SetNeighboringCastles();
    }

    void SetCastle(int castleCount, int minDistance)
    {
        List<Province> provinces = ChildPlaces.Cast<Province>().ToList();
        List<Province> possibleProvinces = provinces.Where(x => x.Terrain == TerrainType.Land).ToList();

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
                Castle castle = new Castle(province);
                province.AddPlace(castle);
                Castles.Add(castle);
                possibleProvinces.RemoveAll(x => Province.GetDistance(province, x) <= minDistance);
            }
        }
    }

    void SetTown(int townCount, int minDistance)
    {
        List<Province> provinces = ChildPlaces.Cast<Province>().ToList();
        List<Province> possibleProvinces = provinces.Where(x => x.Terrain == TerrainType.Land && x.ChildPlaces.Count == 0).ToList();

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
                Town town = new Town(province);
                province.AddPlace(town);
                Towns.Add(town);
                possibleProvinces.RemoveAll(x => Province.GetDistance(province, x) <= minDistance);
            }
        }
    }

    void SetRoad()
    {
        List<Province> provinces = ChildPlaces.Cast<Province>().ToList();
        List<Province> castleOrTownProvinces = provinces.Where(x => x.ChildPlaces.Count > 0).ToList();

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

                HexPathFinder.GetPath(minNotSetRoadProvince, minSetRoadProvince, Width, Height, provinces).ForEach(x => x.SetIsRoad(true));
            }
        }
    }

    void SetSecondaryRoad(int range, int diff)
    {
        List<Province> provinces = ChildPlaces.Cast<Province>().ToList();
        List<Province> castleOrTownProvinces = provinces.Where(x => x.ChildPlaces.Count > 0).ToList();

        foreach (Province province in castleOrTownProvinces)
        {
            List<Province> closeProvinces = castleOrTownProvinces.Where(x => 0 < Province.GetDistance(x, province) && Province.GetDistance(x, province) <= range).ToList();

            foreach (Province closeProvince in closeProvinces)
            {
                int distance = Province.GetDistance(province, closeProvince);

                List<Province> path = HexPathFinder.GetPath(province, closeProvince, Width, Height, provinces);
                int cost = HexPathFinder.GetMovementCost(path);

                if (cost - distance >= diff)
                {
                    path.ForEach(x => x.SetIsRoad(true));
                }
            }
        }
    }

    void SetTerritory()
    {
        List<Province> provinces = ChildPlaces.Cast<Province>().ToList();

        List<Province> castleProvinces = new List<Province>();
        Castles.ForEach(x => castleProvinces.Add((Province)x.ParentPlace));

        int range = 0;

        while(provinces.Count > 0)
        {
            foreach(Province province in castleProvinces)
            {
                List<Province> closeProvinces = provinces.Where(x => range - 1 < Province.GetDistance(x, province) && Province.GetDistance(x, province) <= range).ToList();
                foreach(Province closeProvince in closeProvinces)
                {
                    if(closeProvince.territoryCastle == null)
                    {
                        closeProvince.SetTerritoryCastle((Castle)province.ChildPlaces[0]);
                        ((Castle)province.ChildPlaces[0]).AddTerritoryProvince(closeProvince);
                    }
                }
            }

            provinces.RemoveAll(x => x.territoryCastle != null);
            range++;
        }
    }

    void SetNeighboringCastles()
    {
        //bool[] check = new bool[ChildPlaces.Count];

        //foreach(Castle castle in Castles)
        //{
        //    for(int i=0; i<check.Length; i++)
        //    {
        //        check[i] = false;
        //    }
        //    List<Castle> neighboringCastles = new List<Castle>();

        //    Province province = (Province)castle.ParentPlace;
        //    CheckNeighboringProvince(province.x, province.z, check, neighboringCastles, castle);

        //    castle.AddNeighboringCastles(neighboringCastles);
        //}
        List<Province> provinces = ChildPlaces.Cast<Province>().ToList();

        List<Province> castleProvinces = new List<Province>();
        Castles.ForEach(x => castleProvinces.Add((Province)x.ParentPlace));
        foreach (Province province in castleProvinces)
        {
            List<Province> closeProvinces = castleProvinces.Where(x => 0 < Province.GetDistance(x, province) && Province.GetDistance(x, province) <= castleMinDistance * 2).ToList();
            closeProvinces = closeProvinces.Where(x => HexPathFinder.GetMovementCost(HexPathFinder.GetPath(x, province, Width, Height, provinces)) <= castleMinDistance * 2).ToList();
            List <Castle> closeCastles = new List<Castle>();
            closeProvinces.ForEach(x => closeCastles.Add((Castle)x.ChildPlaces[0]));
            ((Castle)province.ChildPlaces[0]).AddNeighboringCastles(closeCastles);
        }
    }

    void CheckNeighboringProvince(int x, int z, bool[] check, List<Castle> neighboringCastles, Castle targetCastle)
    {
        if (0 <= x && x < Width && 0 <= z && z < Height)
        {
            if (check[x + Width * z] == false)
            {
                int index = x + Width * z;
                check[index] = true;
                if (ChildPlaces[index].ChildPlaces.Count > 0 && ChildPlaces[index].ChildPlaces[0] is Castle && ChildPlaces[index].ChildPlaces[0] != targetCastle)
                {
                    neighboringCastles.Add((Castle)ChildPlaces[index].ChildPlaces[0]);
                }
                else if((ChildPlaces[index].ChildPlaces.Count > 0 && (ChildPlaces[index].ChildPlaces[0] is Town || ChildPlaces[index].ChildPlaces[0] == targetCastle)) || (ChildPlaces[index].ChildPlaces.Count == 0 && ((Province)ChildPlaces[index]).IsRoad))
                {
                    Province province = (Province)ChildPlaces[index];
                    CheckNeighboringProvince(province.x + (province.z % 2), province.z + 1, check, neighboringCastles, targetCastle);
                    CheckNeighboringProvince(province.x + 1, province.z, check, neighboringCastles, targetCastle);
                    CheckNeighboringProvince(province.x + (province.z % 2), province.z - 1, check, neighboringCastles, targetCastle);
                    CheckNeighboringProvince(province.x - ((province.z + 1) % 2), province.z - 1, check, neighboringCastles, targetCastle);
                    CheckNeighboringProvince(province.x - 1, province.z, check, neighboringCastles, targetCastle);
                    CheckNeighboringProvince(province.x - ((province.z + 1) % 2), province.z + 1, check, neighboringCastles, targetCastle);
                }
            }
        }
    }

    public Province GetProvince(int x, int z)
    {
        return (Province)ChildPlaces[x + z * Width];
    }
}
