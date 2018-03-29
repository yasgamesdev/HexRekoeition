using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetCurPlaceButton : MonoBehaviour {

    Person person;
    Place place;

	public void Init(Person person, Place place, string labelText)
    {
        this.person = person;
        this.place = place;
        GetComponentInChildren<Text>().text = labelText;
    }

    public void Clicked()
    {
        //person.SetCurPlace(place);

        //if(person.CurPlace.Type == PlaceType.Castle)
        //{
        //    SceneManager.LoadScene("Castle");
        //}
        //else
        //{
        //    SceneManager.LoadScene("Town");
        //}
    }
}
