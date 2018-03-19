using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    public GameObject personPrefab;
    SLGCore core;
	// Use this for initialization
	void Start () {
        core = new SLGCore(HexMetrics.chunkCountX * HexMetrics.chunkSizeX, HexMetrics.chunkCountZ * HexMetrics.chunkSizeZ);

        GameObject hexMesh = GameObject.Find("HexMesh");
        if(hexMesh != null)
        {
            hexMesh.GetComponent<HexMesh>().SetData(core.GetWorld());
        }

        foreach(Province province in core.GetWorld().children.Where(x => x.units.Count > 0))
        {
            foreach(Unit unit in province.units)
            {
                GameObject obj = Instantiate(personPrefab);
                obj.GetComponent<WorldPerson>().Init((Person)unit);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
