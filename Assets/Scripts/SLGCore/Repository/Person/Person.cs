using System;
using System.Collections.Generic;
using System.Linq;

public class Person : RepositoryData
{
    public bool IsPlayer { get; private set; }
    public string Name { get; private set; }
    public PersonStatus Status { get; private set; }
    public int FactionId { get; private set; }
    public int BossPersonId { get; private set; }
    public int HouseId { get; private set; }
    
    public PlaceType curPlaceType { get; private set; }
    public int placeId { get; private set; }

    public int kunko { get; private set; }

    public Person(bool isPlayer, string name, PersonStatus status, Repository repository) :base(repository)
    {
        IsPlayer = isPlayer;
        Name = name;
        Status = status;

        kunko = GetKunko(status);
    }

    public void SetFactionId(int factionId)
    {

    }

    public void SetBossPersonId(int bossPersonId)
    {

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