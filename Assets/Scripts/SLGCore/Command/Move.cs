using System;
using System.Collections.Generic;
using System.Linq;

public class Move : Command
{
    public Province province;

    public Move(Province province)
    {
        this.province = province;
    }

    public override bool Update(Unit unit)
    {
        unit.moveProgress++;
        int cost = province.isRoad ? 1 : (province.terrain == TerrainType.Land ? HexPathFinder.LandCost : HexPathFinder.SeaCost);
        if(unit.moveProgress == cost)
        {
            unit.moveProgress = 0;

            unit.place.RemoveUnit(unit);
            province.AddUnit(unit);
            unit.SetPlace(province);
            return true;
        }

        return false;
    }
}