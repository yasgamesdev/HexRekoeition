using System;
using System.Collections.Generic;
using System.Linq;

public class Place : Component
{
    public PlaceType CurPlaceType { get; private set; }
    int curPlaceId;

    public void SetCurPlace(RepositoryData curPlace)
    {
        if(curPlace is Province)
        {
            CurPlaceType = PlaceType.Province;
        }
        else if(curPlace is Castle)
        {
            CurPlaceType = PlaceType.Castle;
        }
        else if (curPlace is Town)
        {
            CurPlaceType = PlaceType.Town;
        }
        else if (curPlace is House)
        {
            CurPlaceType = PlaceType.House;
        }

        curPlaceId = curPlace.Id;
    }

    public Province GetCurProvince()
    {
        return ProvinceRepository.Instance.GetProvince(curPlaceId);
    }

    public Castle GetCurCastle()
    {
        return CastleRepository.Instance.GetCastle(curPlaceId);
    }

    public Town GetCurTown()
    {
        return TownRepository.Instance.GetTown(curPlaceId);
    }

    public House GetCurHouse()
    {
        return HouseRepository.Instance.GetHouse(curPlaceId);
    }
}