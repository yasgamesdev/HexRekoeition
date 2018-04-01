using System;
using System.Collections.Generic;
using System.Linq;

public class House : RepositoryData
{
    public int PersonId { get; private set; }

    public HouseType houseType { get; private set; } = HouseType.None;
    int placeId;

    public House(Person person, Repository repository) : base(repository)
    {
    }

    public void SetCastle(int castleId)
    {
        UnsetHouse();

        houseType = HouseType.Castle;
        placeId = castleId;
        CastleRepository.Instance.GetCastle(castleId).AddHouse(Id);
    }

    public void SetTown(int townId)
    {
        UnsetHouse();

        houseType = HouseType.Town;
        placeId = townId;
        TownRepository.Instance.GetTown(townId).AddHouse(Id);
    }

    void UnsetHouse()
    {
        if(houseType == HouseType.Castle)
        {
            CastleRepository.Instance.GetCastle(placeId).RemoveHouse(Id);
        }
        else if(houseType == HouseType.Town)
        {
            TownRepository.Instance.GetTown(placeId).RemoveHouse(Id);
        }

        houseType = HouseType.None;
    }
}