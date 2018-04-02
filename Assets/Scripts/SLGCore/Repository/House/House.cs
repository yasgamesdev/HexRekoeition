using System;
using System.Collections.Generic;
using System.Linq;

public class House : RepositoryData
{
    int ownerPersonId;

    public HouseType HouseType { get; private set; } = HouseType.None;
    int placeId;

    public House(Person ownerPerson, Repository repository) : base(repository)
    {
        ownerPersonId = ownerPerson.Id;
    }

    public Person GetOwnerPerson()
    {
        return PersonRepository.Instance.GetPerson(ownerPersonId);
    }

    public void SetCastle(Castle castle)
    {
        HouseType = HouseType.Castle;
        placeId = castle.Id;
    }

    public Castle GetCastle()
    {
        return CastleRepository.Instance.GetCastle(placeId);
    }

    public void SetTown(Town town)
    {
        HouseType = HouseType.Town;
        placeId = town.Id;
    }

    public Town GetTown()
    {
        return TownRepository.Instance.GetTown(placeId);
    }
}