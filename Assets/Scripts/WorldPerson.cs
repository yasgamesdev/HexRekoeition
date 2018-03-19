using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPerson : MonoBehaviour
{
    Person person;

    public void Init(Person person)
    {
        this.person = person;
    }

    // Update is called once per frame
    void Update()
    {
        if (person.place.type == PlaceType.Province)
        {
            Province province = (Province)person.place;

            Vector3 center;
            center.x = (province.x + province.z * 0.5f - province.z / 2) * (HexMetrics.innerRadius * 2f);
            center.y = 0.0f;
            center.z = province.z * (HexMetrics.outerRadius * 1.5f);
            transform.localPosition = center;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Province province = (Province)person.place;
            province.RemoveUnit(person);
            Province nextProvicne = (Province)province.parent.children[province.i + 1];
            nextProvicne.AddUnit(person);
            person.SetPlace(nextProvicne);
        }
    }
}
