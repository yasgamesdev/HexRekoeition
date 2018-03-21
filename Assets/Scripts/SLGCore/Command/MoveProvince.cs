using System;
using System.Collections.Generic;
using System.Linq;

public class MoveProvince : Command
{
    public Province province;
    bool init = false;

    public MoveProvince(Province province)
    {
        this.province = province;
    }
    public override CommandExecResult Execute(Unit unit)
    {
        if(!init)
        {
            init = true;
            unit.SetNextProvince(province);
        }

        unit.IncrementMoveProgress();

        if(unit.NextProvince == null)
        {
            return CommandExecResult.FinishAndBreak;
        }
        else
        {
            return CommandExecResult.Continue;
        }
    }
}