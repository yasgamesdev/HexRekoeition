using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetCurPlaceButton : MonoBehaviour {

    Person person;

	public void Init(Person person, string labelText)
    {
        this.person = person;
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
