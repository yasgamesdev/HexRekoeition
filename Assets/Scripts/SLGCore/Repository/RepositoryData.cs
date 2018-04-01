using System;
using System.Collections.Generic;
using System.Linq;

public class RepositoryData
{
    public int Id { get; private set; }

    public RepositoryData(Repository repository)
    {
        repository.RegisterData(this);
    }

    public void SetId(int id)
    {
        Id = id;
    }
}