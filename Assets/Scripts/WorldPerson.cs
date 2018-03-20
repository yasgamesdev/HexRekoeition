﻿using System.Collections;
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

            if (person.moveProgress == 0)
            {
                Vector3 center;
                center.x = (province.x + province.z * 0.5f - province.z / 2) * (HexMetrics.innerRadius * 2f);
                center.y = 0.2f;
                center.z = province.z * (HexMetrics.outerRadius * 1.5f);
                transform.localPosition = center;
            }
            else
            {
                Move move = (Move)person.commands.Peek();

                Vector3 from;
                from.x = (province.x + province.z * 0.5f - province.z / 2) * (HexMetrics.innerRadius * 2f);
                from.y = 0.2f;
                from.z = province.z * (HexMetrics.outerRadius * 1.5f);

                Vector3 to;
                to.x = (move.province.x + move.province.z * 0.5f - move.province.z / 2) * (HexMetrics.innerRadius * 2f);
                to.y = 0.2f;
                to.z = move.province.z * (HexMetrics.outerRadius * 1.5f);

                int totalCost = HexPathFinder.GetCost(move.province);
                float percent = (float)person.moveProgress / (float)totalCost;

                Vector3 center = from + (to - from) * percent;
                transform.localPosition = center;
            }
        }
    }
}