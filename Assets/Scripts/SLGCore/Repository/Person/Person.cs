using System;
using System.Collections.Generic;
using System.Linq;

public class Person : RepositoryData
{ 
    public bool IsPlayer { get; private set; }
    public string Name { get; private set; }
    public PersonStatus Status { get; private set; }
    int factionId;
    int bossPersonId;
    int houseId;

    public int Kunko { get; private set; }

    public Person(bool isPlayer, string name, PersonStatus status, Repository repository) :base(repository)
    {
        AddComponent(new Place());
        AddComponent(new CommandQueue());

        IsPlayer = isPlayer;
        Name = name;
        Status = status;

        Kunko = GetKunko(status);
    }

    public void SetIsPlayer(bool isPlayer)
    {
        IsPlayer = isPlayer;
    }

    public void SetFaction(Faction faction)
    {
        factionId = faction.Id;
    }

    public Faction GetFaction()
    {
        return FactionRepository.Instance.GetFaction(factionId);
    }

    public void SetBossPerson(Person bossPerson)
    {
        bossPersonId = bossPerson.Id;
    }

    public Person GetBossPerson()
    {
        return PersonRepository.Instance.GetPerson(bossPersonId);
    }

    public void SetHouse(House house)
    {
        houseId = house.Id;
    }

    public House GetHouse()
    {
        return HouseRepository.Instance.GetHouse(houseId);
    }

    public static int GetKunko(PersonStatus status)
    {
        switch(status)
        {
            case PersonStatus.Ronin:
            case PersonStatus.AshigaruKumigashira:
                return 0;
            case PersonStatus.AshigaruDaisho:
                return 200;
            case PersonStatus.SamuraiDaisho:
                return 600;
            case PersonStatus.Busho:
                return 1400;
            case PersonStatus.Karo:
                return 3000;
            case PersonStatus.Joshu:
                return 4500;
            case PersonStatus.Kokushu:
                return 10000;
            case PersonStatus.Daimyo:
                return 60000;
            default:
                return 0;
        }
    }
}