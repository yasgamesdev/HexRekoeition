using System;
using System.Collections.Generic;
using System.Linq;

public class Repository
{
    List<RepositoryData> data;

    public Repository()
    {
        data = new List<RepositoryData>();
    }

    public void RegisterData(RepositoryData repoData)
    {
        data.Add(repoData);
        repoData.SetId(data.Count - 1);
    }

    protected RepositoryData GetRepositoryData(int Id)
    {
        return data[Id];
    }

    protected List<RepositoryData> GetAllRepositoryData()
    {
        return new List<RepositoryData>(data);
    }
}