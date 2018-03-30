using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    SLGCore core;

    public GameObject personPrefab;
    List<GameObject> units = new List<GameObject>();

    WorldPerson playerPerson;

    private void Start()
    {
        core = GameObject.Find("GameInstance").GetComponent<GameInstance>().GetSLGCore();

        //GameObject obj = Instantiate(personPrefab, transform);
        //obj.GetComponent<WorldPerson>().Init(core.GetPlayerPerson());
        //units.Add(obj);
        //playerPerson = obj.GetComponent<WorldPerson>();
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

    void SetPlayerPath()
    {
        if (core != null && Input.GetMouseButtonDown(1))
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

                int index = iX + iZ * core.GetWorld().Width + iZ / 2;

                int _x = index % core.GetWorld().Width;
                int _z = index / core.GetWorld().Width;

                Vector3 center;
                center.x = (_x + _z * 0.5f - _z / 2) * (HexMetrics.innerRadius * 2f);
                center.y = 1.0f;
                center.z = _z * (HexMetrics.outerRadius * 1.5f);

                Province province = core.GetWorld().GetProvince(_x, _z);

                if (province.ChildPlaces.Count > 0 && province.ChildPlaces[0] is Castle)
                {
                    Castle castle = (Castle)province.ChildPlaces[0];
                    foreach (Castle neighbor in castle.GetNeighboringCastles())
                    {
                        Province parentProvince = (Province)neighbor.ParentPlace;
                        Debug.Log(parentProvince.x + ", " + parentProvince.z);
                    }
                }

                //Province fromProvince = (Province)playerPerson.person.CurPlace;
                //List<Province> path = HexPathFinder.GetPath(fromProvince, province, core.world.Width, core.world.Height, core.world.ChildPlaces.Cast<Province>().ToList());
                //if (path.Count > 0)
                //{
                //    for (int i = 1; i < path.Count; i++)
                //    {
                //        playerPerson.person.EnqueueCommand(new MoveProvince(path[i]));
                //    }
                //}
            }
        }
    }
}
