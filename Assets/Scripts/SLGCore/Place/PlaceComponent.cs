using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlaceComponent : Component
{
    public PlaceType CurPlaceType { get; private set; }
    int curPlaceId;

    List<int> pathProvinceIds = new List<int>();
    int curPathIndex;

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

    public void Update()
    {
        if(pathProvinceIds.Count > 0)
        {
            curPathIndex++;
            Province nextProvince = ProvinceRepository.Instance.GetProvince(pathProvinceIds[curPathIndex]);
            SetCurPlace(nextProvince);

            if(curPathIndex >= pathProvinceIds.Count - 1)
            {
                pathProvinceIds.Clear();
                curPathIndex = 0;
            }
        }
    }

    public void SetPath(List<Province> path)
    {
        pathProvinceIds = path.ConvertAll(x => x.Id);
    }

    public bool HavePath()
    {
        return pathProvinceIds.Count > 0;
    }

    public Province GetNextProvince()
    {
        return ProvinceRepository.Instance.GetProvince(pathProvinceIds[curPathIndex + 1]);
    }
}