using System;
using System.Collections.Generic;
using System.Linq;

public class House : RepositoryData
{
    int ownerPersonId;

    public HouseType houseType { get; private set; } = HouseType.None;
    int placeId;

    public House(Person person, Repository repository) : base(repository)
    {
    }

    public Person GetOwnerPerson()
    {
        return PersonRepository.Instance.GetPerson(ownerPersonId);
    }

    public void SetCastle(int castleId)
    {
        houseType = HouseType.Castle;
        placeId = castleId;
    }

    public Castle GetCastle()
    {
        return CastleRepository.Instance.GetCastle(placeId);
    }

    public void SetTown(int townId)
    {
        houseType = HouseType.Town;
        placeId = townId;
    }

    public Town GetTown()
    {
        return TownRepository.Instance.GetTown(placeId);
    }
}