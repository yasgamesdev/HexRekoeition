using System;
using System.Collections.Generic;
using System.Linq;

public class CastleRepository : Repository
{
    private static CastleRepository instance = new CastleRepository();

    public static CastleRepository Instance
    {
        get
        {
            return instance;
        }
    }

    private CastleRepository()
    {
    }

    public void GenerateCastle(int castleCount, int minDistance, ProvinceRepository provinceRepository)
    {
        List<Province> possibleProvinces = provinceRepository.GetAllLandAndNoneProvince();

        Random rand = new Random();

        for (int i = 0; i < castleCount; i++)
        {
            if (possibleProvinces.Count == 0)
            {
                break;
            }
            else
            {
                int index = rand.Next(possibleProvinces.Count);
                Province province = possibleProvinces[index];
                Castle castle = new Castle(province, this);
                possibleProvinces.RemoveAll(x => Province.GetDistance(province, x) <= minDistance);
            }
        }
    }

    public Castle GetCastle(int castleId)
    {
        return (Castle)GetRepositoryData(castleId);
    }
}