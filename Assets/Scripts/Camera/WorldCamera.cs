using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCamera : MonoBehaviour {
    SLGCore core;
    GameObject playerWorldPerson;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(playerWorldPerson == null)
        {
            playerWorldPerson = GameObject.Find("Person(Clone)");
        }

        if(playerWorldPerson != null)
        {
            transform.localPosition = new Vector3(playerWorldPerson.transform.localPosition.x, 10.0f, playerWorldPerson.transform.localPosition.z);
        }
	}
}
