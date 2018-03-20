using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPerson : MonoBehaviour
{
    public Person person { get; private set; }

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
            center.y = 0.2f;
            center.z = province.z * (HexMetrics.outerRadius * 1.5f);
            transform.localPosition = center;
        }
    }
}
