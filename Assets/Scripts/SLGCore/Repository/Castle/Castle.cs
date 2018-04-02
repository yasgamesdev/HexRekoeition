using System;
using System.Collections.Generic;
using System.Linq;

public class Castle : RepositoryData
{
    int provinceId;

    List<int> territoryProvinceIds = new List<int>();
    List<int> neighboringCastleIds = new List<int>();

    int factionId;
    int joshuPersonId;

    public Castle(Province province, Repository repository) : base(repository)
    {
        provinceId = province.Id;

        province.SetProvinceType(ProvinceType.Castle);
        province.SetCastle(this);
    }

    public Province GetProvince()
    {
        return ProvinceRepository.Instance.GetProvince(provinceId);
    }

    public void AddTerritoryProvince(Province territoryProvince)
    {
        territoryProvinceIds.Add(territoryProvince.Id);
    }

    public List<Province> GetTerritoryProvince()
    {
        return territoryProvinceIds.ConvertAll(x => ProvinceRepository.Instance.GetProvince(x));
    }

    public void AddUniqueNeighboringCastle(Castle neighboringCastle)
    {
        if(!neighboringCastleIds.Contains(neighboringCastle.Id))
        {
            neighboringCastleIds.Add(neighboringCastle.Id);
        }
    }

    public List<Castle> GetNeighboringCastles()
    {
        return neighboringCastleIds.ConvertAll(x => CastleRepository.Instance.GetCastle(x));
    }

    public void SetFaction(Faction faction)
    {
        factionId = faction.Id;
    }

    public Faction GetFaction()
    {
        return FactionRepository.Instance.GetFaction(factionId);
    }

    public void SetJoshu(Person joshuPerson)
    {
        joshuPersonId = joshuPerson.Id;
    }

    public Person GetJoshu()
    {
        return PersonRepository.Instance.GetPerson(joshuPersonId);
    }

    public List<House> GetHouses()
    {
        return HouseRepository.Instance.GetHouses(this);
    }
}