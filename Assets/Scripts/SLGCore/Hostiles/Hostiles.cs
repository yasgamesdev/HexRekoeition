using System;
using System.Collections.Generic;
using System.Linq;

public class Hostiles
{
    private static Hostiles instance = new Hostiles();

    public static Hostiles Instance
    {
        get
        {
            return instance;
        }
    }

    private Hostiles()
    {
    }

    Dictionary<int, Dictionary<int, bool>> hostiles = new Dictionary<int, Dictionary<int, bool>>();

    public void AddFaction(Faction faction)
    {
        hostiles.Values.ToList().ForEach(x => x.Add(faction.Id, false));

        Dictionary<int, bool> hostile = new Dictionary<int, bool>();
        hostile.Add(faction.Id, false);
        hostiles.Keys.ToList().ForEach(x => hostile.Add(x, false));
        hostiles.Add(faction.Id, hostile);
    }

    public void RemoveFaction(Faction faction)
    {
        hostiles.Values.ToList().ForEach(x => x.Remove(faction.Id));

        hostiles.Remove(faction.Id);
    }

    public void SetHostile(Faction faction0, Faction faction1, bool isHostile)
    {
        hostiles[faction0.Id][faction1.Id] = isHostile;
        hostiles[faction1.Id][faction0.Id] = isHostile;
    }

    public Dictionary<Faction, bool> GetHostiles(Faction faction)
    {
        Dictionary<Faction, bool> ret = new Dictionary<Faction, bool>();

        foreach(var item in hostiles[faction.Id])
        {
            ret.Add(FactionRepository.Instance.GetFaction(item.Key), item.Value);
        }

        return ret;
    }
}