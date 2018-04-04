using System;
using System.Collections.Generic;
using System.Linq;

public class PersonRepository : Repository
{
    private static PersonRepository instance = new PersonRepository();

    public static PersonRepository Instance
    {
        get
        {
            return instance;
        }
    }

    private PersonRepository()
    {
    }

    public Person GetPerson(int personId)
    {
        return (Person)GetRepositoryData(personId);
    }

    public void GenerateFactions(int maxCastleNum)
    {
        List<Castle> copyCastles = CastleRepository.Instance.GetAllCastle();
        Random rand = new Random();

        while (copyCastles.Count > 0)
        {
            Castle homeCastle = copyCastles[rand.Next(copyCastles.Count)];
            copyCastles.Remove(homeCastle);

            int castleNum = rand.Next(1, maxCastleNum + 1);
            List<Castle> subCastles = new List<Castle>();

            foreach (Castle neightboringCastle in homeCastle.GetNeighboringCastles())
            {
                CheckNieghtboringCastle(neightboringCastle, copyCastles, castleNum, subCastles);
            }

            Person daimyo = new Person(false, NameGenerator.Instance.Generate(), PersonStatus.Daimyo, this);

            House house = HouseRepository.Instance.CreateHouse(daimyo);
            house.SetCastle(homeCastle);
            daimyo.SetHouse(house);

            daimyo.GetPlaceComponent().SetCurPlace(house);

            Faction faction = FactionRepository.Instance.CreateFaction(daimyo.Name);
            daimyo.SetFaction(faction);
            daimyo.SetBossPerson(daimyo);

            homeCastle.SetFaction(faction);
            homeCastle.SetJoshu(daimyo);
            subCastles.ForEach(x => x.SetFaction(faction));
            subCastles.ForEach(x => x.SetJoshu(daimyo));
        }
    }

    void CheckNieghtboringCastle(Castle castle, List<Castle> copyCastles, int castleNum, List<Castle> subCastles)
    {
        if (1 + subCastles.Count < castleNum)
        {
            if (copyCastles.Contains(castle))
            {
                subCastles.Add(castle);
                copyCastles.Remove(castle);

                if (1 + subCastles.Count < castleNum)
                {
                    foreach (Castle neightboringCastle in castle.GetNeighboringCastles())
                    {
                        CheckNieghtboringCastle(neightboringCastle, copyCastles, castleNum, subCastles);
                    }
                }
            }
        }
    }

    public void GenerateSamurai(int num)
    {
        List<Castle> copyCastles = CastleRepository.Instance.GetAllCastle();
        Random rand = new Random();

        for(int i=0; i<num; i++)
        {
            Castle castle = copyCastles[rand.Next(copyCastles.Count)];
            Faction faction = castle.GetFaction();
            Person daimyo = GetDaimyo(faction);

            Castle homeCastle = daimyo.GetHouse().GetCastle();

            Person samurai = new Person(false, NameGenerator.Instance.Generate(), PersonStatus.AshigaruKumigashira, this);

            House house = HouseRepository.Instance.CreateHouse(samurai);
            house.SetCastle(homeCastle);
            samurai.SetHouse(house);

            samurai.GetPlaceComponent().SetCurPlace(house);

            samurai.SetFaction(faction);
            samurai.SetBossPerson(daimyo);
        }
    }

    public Person GetDaimyo(Faction faction)
    {
        return GetAllRepositoryData().Cast<Person>().First(x => x.GetFaction() == faction && x.Status == PersonStatus.Daimyo);
    }

    public void GenerateRonin(int num)
    {
        List<Town> copyTowns = TownRepository.Instance.GetAllTown();
        Random rand = new Random();

        for (int i = 0; i < num; i++)
        {
            Town homeTown = copyTowns[rand.Next(copyTowns.Count)];

            Person ronin = new Person(false, NameGenerator.Instance.Generate(), PersonStatus.Ronin, this);

            House house = HouseRepository.Instance.CreateHouse(ronin);
            house.SetTown(homeTown);
            ronin.SetHouse(house);

            ronin.GetPlaceComponent().SetCurPlace(house);
        }
    }

    public void SetPlayerPerson()
    {
        Person playerPerson = GetPerson(0);
        playerPerson.SetIsPlayer(true);
        House house = playerPerson.GetHouse();
        if(house.HouseType == HouseType.Castle)
        {
            playerPerson.GetPlaceComponent().SetCurPlace(house.GetCastle().GetProvince());
        }
        else
        {
            playerPerson.GetPlaceComponent().SetCurPlace(house.GetTown().GetProvince());
        }
    }

    public Person GetPlayerPerson()
    {
        return GetAllRepositoryData().Cast<Person>().First(x => x.IsPlayer);
    }
}