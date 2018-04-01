using System;
using System.Collections.Generic;
using System.Linq;

public class Town : RepositoryData
{
    int provinceId;

    List<int> houseIds = new List<int>();

    public Town(Province province, Repository repository) : base(repository)
    {
        provinceId = province.Id;

        province.SetProvinceType(ProvinceType.Town);
        province.SetTownId(Id);
    }

    public void AddHouse(int houseId)
    {
        //関連を変更するときは、Houseオブジェクトだけがこれを使うこと
        houseIds.Add(houseId);
    }

    public void RemoveHouse(int houseId)
    {
        //関連を変更するときは、Houseオブジェクトだけがこれを使うこと
        houseIds.Remove(houseId);
    }
}