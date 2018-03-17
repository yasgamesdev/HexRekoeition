using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    SLGCore core;
	// Use this for initialization
	void Start () {
        core = new SLGCore(HexMetrics.chunkCountX * HexMetrics.chunkSizeX, HexMetrics.chunkCountZ * HexMetrics.chunkSizeZ);

        GameObject drawer = GameObject.Find("HexMesh");
        if(drawer != null)
        {
            drawer.GetComponent<HexMesh>().SetData(core);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
