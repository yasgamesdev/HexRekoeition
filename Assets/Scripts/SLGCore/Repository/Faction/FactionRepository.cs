using System;
using System.Collections.Generic;
using System.Linq;

public class FactionRepository : Repository
{
    private static FactionRepository instance = new FactionRepository();

    public static FactionRepository Instance
    {
        get
        {
            return instance;
        }
    }

    private FactionRepository()
    {
    }

    public Faction GetFaction(int factionId)
    {
        return (Faction)GetRepositoryData(factionId);
    }

    public Faction CreateFaction(string name)
    {
        return new Faction(name, this);
    }
}