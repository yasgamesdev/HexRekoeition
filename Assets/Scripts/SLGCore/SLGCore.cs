using System;
using System.Collections.Generic;
using System.Linq;

public class SLGCore
{
    World world;
    Persons persons;
    Forces forces;

    public SLGCore(int width, int height)
    {
        world = new World(width, height);
        persons = new Persons();
        forces = new Forces();
        
        persons.SetReference(world);

        GenerateDaimyos(3);
    }

    void GenerateDaimyos(int maxCastleNum)
    {
        List<Castle> copyCastles = new List<Castle>(world.Castles);
        Random rand = new Random();

        while(copyCastles.Count > 0)
        {
            Castle homeCastle = copyCastles[rand.Next(copyCastles.Count)];
            copyCastles.Remove(homeCastle);

            int castleNum = rand.Next(1, maxCastleNum + 1);
            List<Castle> subCastles = new List<Castle>();

            foreach (Castle neightboringCastle in homeCastle.GetNeighboringCastles())
            {
                CheckNieghtboringCastle(neightboringCastle, copyCastles, castleNum, subCastles);
            }

            House house = new House(homeCastle);
            homeCastle.AddPlace(house);

            Person person = persons.AddDaimyo(NameGenerator.Instance.Generate(), homeCastle);
            person.curPlace = house;
            house.AddStayPerson(person);

            house.SetOwnerPerson(person);

            homeCastle.SetDaimyo(person);
            subCastles.ForEach(x => x.SetDaimyo(person));
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

    public World GetWorld()
    {
        return world;
    }
}