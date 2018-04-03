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

    }

    const int updateSpeed = 1;
    int updateCounter = 0;

    void Update()
    {
        SetPlayerPath();

        //updateCounter++;
        //if (updateCounter >= updateSpeed)
        //{
        //    updateCounter = 0;

        //    core.ProgressQuarterDay();
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
                    fromCastle = province.GetCastle();
                    FindPath();
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

                if (province.ProvinceType == ProvinceType.Castle)
                {
                    toCastle = province.GetCastle();
                    FindPath();
                }
            }
        }
    }

    void FindPath()
    {
        if(fromCastle != null & toCastle != null)
        {
            DijkstraPathFinder.GetPath(fromCastle, toCastle);
        }
    }
}
