using System;
using System.Collections.Generic;
using System.Linq;

public class Person
{
    public bool isPlayer;
    public string name;
    public PersonStatus status;
    public Person daimyo;
    public Person boss;
    public CastleOrTownBase homeCastleOrTown;
    public Place curPlace;
    public int kunko;

    public Person(bool isPlayer, string name, PersonStatus status, Person daimyo, Person boss, CastleOrTownBase homeCastleOrTown, Place curPlace)
    {
        this.isPlayer = isPlayer;
        this.name = name;
        this.status = status;
        this.daimyo = daimyo;
        this.boss = boss;
        this.homeCastleOrTown = homeCastleOrTown;
        this.curPlace = curPlace;
        kunko = GetKunko(status);
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