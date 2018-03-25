using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NameGenerator
{
    private static NameGenerator instance = null;

    public static NameGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NameGenerator();
            }

            return instance;
        }
    }

    List<string> names = new List<string>();
    int index = 0;

    public NameGenerator()
    {
        TextAsset text = Resources.Load<TextAsset>("Names");

        foreach(string name in text.text.Split('\n'))
        {
            if(name.Length != 0)
            {
                names.Add(name);
            }
        }

        names = names.OrderBy(i => Guid.NewGuid()).ToList();
    }

    public string Generate()
    {
        string name = names[index++];

        if(index >= names.Count)
        {
            index = 0;
        }

        return name;
    }
}
