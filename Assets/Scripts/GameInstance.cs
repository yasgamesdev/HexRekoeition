using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    SLGCore core;
	// Use this for initialization
	void Start () {
        core = new SLGCore(HexMetrics.chunkCountX * HexMetrics.chunkSizeX, HexMetrics.chunkCountZ * HexMetrics.chunkSizeZ);

        GameObject hexMesh = GameObject.Find("HexMesh");
        if(hexMesh != null)
        {
            hexMesh.GetComponent<HexMesh>().SetData(core.GetWorld());
        }

        GameObject unitManager = GameObject.Find("UnitManager");
        if (unitManager != null)
        {
            unitManager.GetComponent<UnitManager>().SetData(core);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
