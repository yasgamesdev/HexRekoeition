using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMainAction : MonoBehaviour {
    public GameObject setCurPlaceButton;

    SLGCore core;
    GameObject buttonRoot;

    List<GameObject> buttons = new List<GameObject>();

    Place curShowPlace;
    // Use this for initialization
    void Start()
    {
        //core = GameObject.Find("GameInstance").GetComponent<GameInstance>().GetSLGCore();
        //buttonRoot = transform.Find("ButtonRoot").gameObject;
    }

    void Update()
    {
        //if(!core.GetPlayerPerson().HaveCommand() && curShowPlace != core.GetPlayerPerson().CurPlace)
        //{
        //    curShowPlace = core.GetPlayerPerson().CurPlace;

        //    buttons.ForEach(x => Destroy(x));

        //    Person playerPerson = core.GetPlayerPerson();
        //    if (playerPerson.CurPlace.ContainPlaceType(PlaceType.Castle))
        //    {
        //        GameObject button = Instantiate(setCurPlaceButton, buttonRoot.transform);
        //        button.GetComponent<SetCurPlaceButton>().Init(playerPerson, playerPerson.CurPlace.ChildPlaces[0], @"城に入る");
        //        buttons.Add(button);
        //    }
        //    else if (playerPerson.CurPlace.ContainPlaceType(PlaceType.Town))
        //    {
        //        GameObject button = Instantiate(setCurPlaceButton, buttonRoot.transform);
        //        button.GetComponent<SetCurPlaceButton>().Init(playerPerson, playerPerson.CurPlace.ChildPlaces[0], @"町に入る");
        //        buttons.Add(button);
        //    }
        //}
        //else if(core.GetPlayerPerson().HaveCommand())
        //{
        //    curShowPlace = null;

        //    buttons.ForEach(x => Destroy(x));
        //}
    }
}
