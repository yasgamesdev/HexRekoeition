using System;
using System.Collections.Generic;
using System.Linq;

public class TownRepository : Repository
{
    private static TownRepository instance = new TownRepository();

    public static TownRepository Instance
    {
        get
        {
            return instance;
        }
    }

    private TownRepository()
    {
    }

    public void GenerateTown(int townCount, int minDistance, ProvinceRepository provinceRepository)
    {
        List<Province> possibleProvinces = provinceRepository.GetAllLandAndNoneProvince();

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
                Town town = new Town(province, this);
                possibleProvinces.RemoveAll(x => Province.GetDistance(province, x) <= minDistance);
            }
        }
    }

    public Town GetTown(int townId)
    {
        return (Town)GetRepositoryData(townId);
    }
}