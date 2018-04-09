using System;
using System.Collections.Generic;
using System.Linq;

public class Faction : RepositoryData
{
    public string Name { get; private set; }

    public Faction(string name, Repository repository) :base(repository)
    {
        Name = name;

        Hostiles.Instance.AddFaction(this);
    }

    public void DeclareWar()
    {
        Person playerPerson = PersonRepository.Instance.GetPlayerPerson();
        if (playerPerson.Status == PersonStatus.Daimyo && playerPerson.GetFaction() == this)
        {
            return;
        }

        var hostiles = Hostiles.Instance.GetHostiles(this);
        if(hostiles.Values.Any(x => x == true))
        {
            return;
        }

        List<Castle> castles = CastleRepository.Instance.GetCastles(this);
        List<Faction> neightborFactions = new List<Faction>();
        castles.ForEach(x => x.GetNeighboringCastles().ForEach(y => neightborFactions.Add(y.GetFaction())));
        neightborFactions = neightborFactions.Distinct().ToList();
        neightborFactions.Remove(this);

        if(neightborFactions.Count == 0)
        {
            return;
        }

        int mySoldiersNum = GetSoldiersNum();
        int minSoldiersNum = neightborFactions.Min(x => x.GetSoldiersNum());
        int maxSoldiersNum = neightborFactions.Max(x => x.GetSoldiersNum());

        if(mySoldiersNum >= 10000 && mySoldiersNum >= minSoldiersNum * 2 && mySoldiersNum - minSoldiersNum >= maxSoldiersNum)
        {
            Faction minFaction = neightborFactions.First(x => x.GetSoldiersNum() == minSoldiersNum);
            Hostiles.Instance.SetHostile(this, minFaction, true);
            UnityEngine.Debug.Log(Name + "(" + GetCapitalCastle().GetProvince().x + ", " + GetCapitalCastle().GetProvince().z + ")>" + minFaction.Name + "(" + minFaction.GetCapitalCastle().GetProvince().x + ", " + minFaction.GetCapitalCastle().GetProvince().z + ")");
        }
    }

    public int GetSoldiersNum()
    {
        return CastleRepository.Instance.GetCastles(this).Sum(x => x.SoldiersNum);
    }

    public Castle GetCapitalCastle()
    {
        return CastleRepository.Instance.GetCastles(this).First(x => x.GetHouses().Any(y => y.GetOwnerPerson() == x.GetJoshu()));
    }
}