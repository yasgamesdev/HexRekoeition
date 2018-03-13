using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDrawer : MonoBehaviour
{
    public GameObject landPrefab, seaPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetData(SLGCore core)
    {
        for(int z =0; z<core.height; z++)
        {
            for(int x=0; x<core.width; x++)
            {
                if(core.GetHexCell(x, z).terrain == Terrain.Land)
                {
                    GameObject panel = Instantiate(landPrefab, transform);
                    panel.transform.localPosition = new Vector3(x * 10.0f, 0, z * 10.0f);
                }
                else
                {
                    GameObject panel = Instantiate(seaPrefab, transform);
                    panel.transform.localPosition = new Vector3(x * 10.0f, 0, z * 10.0f);
                }
            }
        }
    }
}
