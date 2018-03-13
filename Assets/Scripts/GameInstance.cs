using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    SLGCore core;
	// Use this for initialization
	void Start () {
        core = new SLGCore(64, 64);

        GameObject drawer = GameObject.Find("HexDrawer");
        if(drawer != null)
        {
            drawer.GetComponent<HexDrawer>().SetData(core);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
