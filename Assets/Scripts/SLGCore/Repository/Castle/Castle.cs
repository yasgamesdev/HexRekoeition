using System;
using System.Collections.Generic;
using System.Linq;

public class Castle : RepositoryData
{
    int provinceId;

    List<int> territoryProvinceIds = new List<int>();
    List<int> neighboringCastleIds = new List<int>();
    public Person Daimyo { get; private set; }
    public Person Joshu { get; private set; }

    public Castle(Province province, Repository repository) : base(repository)
    {
        provinceId = province.Id;

        province.SetProvinceType(ProvinceType.Castle);
        province.SetCastleId(Id);
    }

    public Province GetProvince()
    {
        return ProvinceRepository.Instance.GetProvince(provinceId);
    }

    public void AddTerritoryProvinceId(int territoryProvinceId)
    {
        territoryProvinceIds.Add(territoryProvinceId);
    }

    public void AddUniqueNeighboringCastleId(int neighboringCastleId)
    {
        if(!neighboringCastleIds.Contains(neighboringCastleId))
        {
            neighboringCastleIds.Add(neighboringCastleId);
        }
    }

    public List<int> GetNeighboringCastles()
    {
        return new List<int>(neighboringCastleIds);
    }

    public void SetDaimyo(Person daimyo)
    {
        Daimyo = daimyo;
    }

    public void SetJoshu(Person joshu)
    {
        Joshu = joshu;
    }
}