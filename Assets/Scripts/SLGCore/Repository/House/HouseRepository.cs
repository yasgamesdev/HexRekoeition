using System;
using System.Collections.Generic;
using System.Linq;

public class HouseRepository : Repository
{
    private static HouseRepository instance = new HouseRepository();

    public static HouseRepository Instance
    {
        get
        {
            return instance;
        }
    }

    private HouseRepository()
    {
    }

    public House GetHouse(int houseId)
    {
        return (House)GetRepositoryData(houseId);
    }

    public House CreateHouse(Person ownerPerson)
    {
        return new House(ownerPerson, this);
    }

    public List<House> GetHouses(Castle castle)
    {
        return GetAllRepositoryData().Cast<House>().Where(x => x.HouseType == HouseType.Castle && x.GetCastle() == castle).ToList();
    }

    public List<House> GetHouses(Town town)
    {
        return GetAllRepositoryData().Cast<House>().Where(x => x.HouseType == HouseType.Town && x.GetTown() == town).ToList();
    }
}