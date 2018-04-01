using System;
using System.Collections.Generic;
using System.Linq;

public class SLGCore
{
    private static SLGCore instance = new SLGCore();

    public static SLGCore Instance
    {
        get
        {
            return instance;
        }
    }

    private SLGCore()
    {
    }

    public void GenerateWorld(int width, int height, float size)
    {
        ProvinceRepository.Instance.GenerateProvince(width, height, size);

        CastleRepository.Instance.GenerateCastle(100, 4, ProvinceRepository.Instance);
        TownRepository.Instance.GenerateTown(100, 3, ProvinceRepository.Instance);

        ProvinceRepository.Instance.GenerateRoad(8, 12);

        ProvinceRepository.Instance.SetTerritory();
        ProvinceRepository.Instance.SetNeighboringCastles();
    }

    //World world;
    //Persons persons;
    //Forces forces;

    //public SLGCore(int width, int height)
    //{
    //    world = new World(width, height);
    //    persons = new Persons();
    //    forces = new Forces();

    //    persons.SetReference(world);

    //    GenerateDaimyos(3);
    //    GenerateSamurai(200);
    //}

    //void GenerateDaimyos(int maxCastleNum)
    //{
    //    List<Castle> copyCastles = new List<Castle>(world.Castles);
    //    Random rand = new Random();

    //    while(copyCastles.Count > 0)
    //    {
    //        Castle homeCastle = copyCastles[rand.Next(copyCastles.Count)];
    //        copyCastles.Remove(homeCastle);

    //        int castleNum = rand.Next(1, maxCastleNum + 1);
    //        List<Castle> subCastles = new List<Castle>();

    //        foreach (Castle neightboringCastle in homeCastle.GetNeighboringCastles())
    //        {
    //            CheckNieghtboringCastle(neightboringCastle, copyCastles, castleNum, subCastles);
    //        }

    //        House house = new House(homeCastle);
    //        homeCastle.AddPlace(house);

    //        Person person = persons.AddDaimyo(NameGenerator.Instance.Generate(), homeCastle);
    //        person.curPlace = house;
    //        house.AddStayPerson(person);

    //        house.SetOwnerPerson(person);

    //        homeCastle.SetDaimyo(person);
    //        homeCastle.SetJoshu(person);
    //        subCastles.ForEach(x => x.SetDaimyo(person));
    //    }
    //}

    //void CheckNieghtboringCastle(Castle castle, List<Castle> copyCastles, int castleNum, List<Castle> subCastles)
    //{
    //    if (1 + subCastles.Count < castleNum)
    //    {
    //        if (copyCastles.Contains(castle))
    //        {
    //            subCastles.Add(castle);
    //            copyCastles.Remove(castle);

    //            if (1 + subCastles.Count < castleNum)
    //            {
    //                foreach (Castle neightboringCastle in castle.GetNeighboringCastles())
    //                {
    //                    CheckNieghtboringCastle(neightboringCastle, copyCastles, castleNum, subCastles);
    //                }
    //            }
    //        }
    //    }
    //}

    //void GenerateSamurai(int num)
    //{
    //    List<CastleOrTownBase> castleOrTowns = world.Castles.Where(x => x.Joshu != null).Union<CastleOrTownBase>(world.Towns).ToList();
    //    Random rand = new Random();

    //    for(int i=0; i<num; i++)
    //    {
    //        CastleOrTownBase castleOrTown = castleOrTowns[rand.Next(castleOrTowns.Count)];

    //        House house = new House(castleOrTown);
    //        castleOrTown.AddPlace(house);

    //        Person person = persons.AddSamurai(NameGenerator.Instance.Generate(), PersonStatus.AshigaruKumigashira, castleOrTown);
    //        person.curPlace = house;
    //        house.AddStayPerson(person);

    //        house.SetOwnerPerson(person);
    //    }
    //}

    //public World GetWorld()
    //{
    //    return world;
    //}
}