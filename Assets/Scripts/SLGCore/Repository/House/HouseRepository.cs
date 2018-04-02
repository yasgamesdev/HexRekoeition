using System;
using System.Collections.Generic;
using System.Linq;

public class HouseRepository : Repository
{
    private static HouseRepository instance = new HouseRepository();

    public static HouseRepository Instance
    {
        get
        {
            return instance;
        }
    }

    private HouseRepository()
    {
    }

    public House GetHouse(int houseId)
    {
        return (House)GetRepositoryData(houseId);
    }
}