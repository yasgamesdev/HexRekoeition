using System;
using System.Collections.Generic;
using System.Linq;

public class Edge
{
    int fromCastleId;
    int toCastleId;
    List<int> pathProvinceIds = new List<int>();

    public Edge(Castle fromCastle, Castle toCastle, List<Province> pathProvinces)
    {
        fromCastleId = fromCastle.Id;
        toCastleId = toCastle.Id;
        pathProvinces.ForEach(x => pathProvinceIds.Add(x.Id));
    }

    public Castle GetFromCastle()
    {
        return CastleRepository.Instance.GetCastle(fromCastleId);
    }

    public Castle GetToCastle()
    {
        return CastleRepository.Instance.GetCastle(toCastleId);
    }

    public List<Province> GetPathProvinces()
    {
        return pathProvinceIds.ConvertAll(x => ProvinceRepository.Instance.GetProvince(x));
    }
}