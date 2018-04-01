using System;
using System.Collections.Generic;
using System.Linq;

public class Town : RepositoryData
{
    int provinceId;

    public Town(Province province, Repository repository) : base(repository)
    {
        provinceId = province.Id;

        province.SetProvinceType(ProvinceType.Town);
        province.SetTownId(Id);
    }
}