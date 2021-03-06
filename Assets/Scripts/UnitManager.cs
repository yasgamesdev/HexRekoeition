﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public GameObject personPrefab;
    List<GameObject> units = new List<GameObject>();

    WorldPerson playerPerson;

    private void Start()
    {
        Person playerPerson = PersonRepository.Instance.GetPlayerPerson();
        GameObject playerWorldPerson = Instantiate(personPrefab, transform);
        playerWorldPerson.GetComponent<WorldPerson>().Init(playerPerson);
        units.Add(playerWorldPerson);
    }

    float timer = 0.0f;
    const float speed = 0.1f;

    void Update()
    {
        SetPlayerPath();

        //if(PersonRepository.Instance.GetPlayerPerson().GetComponent<CommandComponent>().HaveCommand())
        //{
        //    timer += Time.deltaTime;
        //    if(timer < speed)
        //    {

        //    }
        //    else
        //    {
        //        timer = 0.0f;

        //        units.ForEach(x => x.GetComponent<WorldPerson>().person.GetComponent<CommandComponent>().Exec());
        //    }
        //}

        //if(timer == 0.0f)
        //{
        //    units.ForEach(x => x.GetComponent<WorldPerson>().UpdatePosition());
        //}
        //else
        //{
        //    units.ForEach(x => x.GetComponent<WorldPerson>().UpdateRatePosition(timer / speed));
        //}
    }

    Castle fromCastle, toCastle;

    void SetPlayerPath()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                Vector3 position = transform.InverseTransformPoint(hit.point);

                float x = position.x / (HexMetrics.innerRadius * 2f);
                float y = -x;

                float offset = position.z / (HexMetrics.outerRadius * 3f);
                x -= offset;
                y -= offset;

                int iX = Mathf.RoundToInt(x);
                int iY = Mathf.RoundToInt(y);
                int iZ = Mathf.RoundToInt(-x - y);

                if (iX + iY + iZ != 0)
                {
                    float dX = Mathf.Abs(x - iX);
                    float dY = Mathf.Abs(y - iY);
                    float dZ = Mathf.Abs(-x - y - iZ);

                    if (dX > dY && dX > dZ)
                    {
                        iX = -iY - iZ;
                    }
                    else if (dZ > dY)
                    {
                        iZ = -iX - iY;
                    }
                }

                int index = iX + iZ * ProvinceRepository.Instance.Width + iZ / 2;

                int _x = index % ProvinceRepository.Instance.Width;
                int _z = index / ProvinceRepository.Instance.Width;

                Vector3 center;
                center.x = (_x + _z * 0.5f - _z / 2) * (HexMetrics.innerRadius * 2f);
                center.y = 1.0f;
                center.z = _z * (HexMetrics.outerRadius * 1.5f);

                Province province = ProvinceRepository.Instance.GetProvince(_x, _z);

                if (province.ProvinceType == ProvinceType.Castle)
                {
                    //Person playerPerson = PersonRepository.Instance.GetPlayerPerson();
                    //if(playerPerson.GetFaction() != province.GetCastle().GetFaction())
                    //{
                    //    Hostiles.Instance.SetHostile(playerPerson.GetFaction(), province.GetCastle().GetFaction(), true);
                    //}
                    Debug.Log(province.GetCastle().GetFaction().GetSoldiersNum());
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                Vector3 position = transform.InverseTransformPoint(hit.point);

                float x = position.x / (HexMetrics.innerRadius * 2f);
                float y = -x;

                float offset = position.z / (HexMetrics.outerRadius * 3f);
                x -= offset;
                y -= offset;

                int iX = Mathf.RoundToInt(x);
                int iY = Mathf.RoundToInt(y);
                int iZ = Mathf.RoundToInt(-x - y);

                if (iX + iY + iZ != 0)
                {
                    float dX = Mathf.Abs(x - iX);
                    float dY = Mathf.Abs(y - iY);
                    float dZ = Mathf.Abs(-x - y - iZ);

                    if (dX > dY && dX > dZ)
                    {
                        iX = -iY - iZ;
                    }
                    else if (dZ > dY)
                    {
                        iZ = -iX - iY;
                    }
                }

                int index = iX + iZ * ProvinceRepository.Instance.Width + iZ / 2;

                int _x = index % ProvinceRepository.Instance.Width;
                int _z = index / ProvinceRepository.Instance.Width;

                Vector3 center;
                center.x = (_x + _z * 0.5f - _z / 2) * (HexMetrics.innerRadius * 2f);
                center.y = 1.0f;
                center.z = _z * (HexMetrics.outerRadius * 1.5f);

                Province province = ProvinceRepository.Instance.GetProvince(_x, _z);

                Person person = PersonRepository.Instance.GetPlayerPerson();
                if (!person.GetComponent<CommandComponent>().HaveCommand() && province.ProvinceType != ProvinceType.None)
                {
                    //var path = DijkstraPathFinder.GetPath(person.GetPlaceComponent().GetCurProvince().GetCastle(), province.GetCastle());
                    //PersonRepository.Instance.GetPlayerPerson().GetPlaceComponent().SetPath(path);

                    //toCastle = province.GetCastle();
                    //FindPath();

                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    bool result = false;
                    //var path = DijkstraPathFinder.GetPath(person.GetFaction(), ref result, person.GetPlaceComponent().GetCurProvince().GetCastle(), province.GetCastle());
                    var path = HexPathFinder.GetPath(person.GetFaction(), true, person.GetComponent<PlaceComponent>().GetCurProvince(), province, ref result);
                    sw.Stop();

                    Debug.Log(result);
                    Debug.Log(sw.Elapsed);
                    PersonRepository.Instance.GetPlayerPerson().GetComponent<CommandComponent>().AddCommand(new Move(path));
                }
            }
        }
    }

    void FindPath()
    {
        if(fromCastle != null & toCastle != null)
        {
            var path = DijkstraPathFinder.GetPath(fromCastle, toCastle);
            path.ForEach(x => UnityEngine.Debug.Log(x.x + ", " + x.z));
        }
    }
}
