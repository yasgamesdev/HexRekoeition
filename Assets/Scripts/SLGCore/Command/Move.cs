using System;
using System.Collections.Generic;
using System.Linq;

public class Move : Command
{
    List<int> pathProvinceIds = new List<int>();
    int curPathIndex;

    public Move(List<Province> path)
    {
        pathProvinceIds = path.ConvertAll(x => x.Id);
        curPathIndex = 0;
    }

    public override CommandExecResult Exec(Component component)
    {
        curPathIndex++;
        Province nextProvince = ProvinceRepository.Instance.GetProvince(pathProvinceIds[curPathIndex]);
        component.GetComponent<PlaceComponent>().SetCurPlace(nextProvince);

        if (curPathIndex >= pathProvinceIds.Count - 1)
        {
            return CommandExecResult.FinishAndBreak;
        }
        else
        {
            return CommandExecResult.Continue;
        }
    }

    public Province GetNextProvince()
    {
        return ProvinceRepository.Instance.GetProvince(pathProvinceIds[curPathIndex + 1]);
    }
}