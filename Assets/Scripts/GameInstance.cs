using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    World world;
	// Use this for initialization
	void Start () {
        world = new World(HexMetrics.chunkCountX * HexMetrics.chunkSizeX, HexMetrics.chunkCountZ * HexMetrics.chunkSizeZ);

        GameObject hexMesh = GameObject.Find("HexMesh");
        if(hexMesh != null)
        {
            hexMesh.GetComponent<HexMesh>().SetData(world);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
