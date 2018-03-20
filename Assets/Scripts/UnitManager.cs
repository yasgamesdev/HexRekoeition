using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    SLGCore core;

    public GameObject personPrefab;
    List<GameObject> units = new List<GameObject>();

    public WorldPerson playerPerson;

    public void SetData(SLGCore core)
    {
        this.core = core;

        foreach (Province province in core.GetWorld().children.Where(x => x.units.Count > 0))
        {
            foreach (Unit unit in province.units)
            {
                GameObject obj = Instantiate(personPrefab, transform);
                obj.GetComponent<WorldPerson>().Init((Person)unit);
                units.Add(obj);

                if(unit.isPlayer)
                {
                    playerPerson = obj.GetComponent<WorldPerson>();
                }
            }
        }
    }

    // Update is called once per frame
    const int updateSpeed = 1;
    int updateCounter = 0;
    
    void Update()
    {
        SetPlayerPath();

        if(playerPerson.person.commands.Count > 0)
        {
            updateCounter++;
            if(updateCounter >= updateSpeed)
            {
                updateCounter = 0;

                bool finish = playerPerson.person.commands.Peek().Update(playerPerson.person);

                if (finish)
                {
                    playerPerson.person.commands.Dequeue();
                }
            }
        }
    }

    void SetPlayerPath()
    {
        if (core != null && playerPerson != null && playerPerson.person.commands.Count == 0 && Input.GetMouseButtonDown(1))
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

                int index = iX + iZ * core.world.width + iZ / 2;

                int _x = index % core.world.width;
                int _z = index / core.world.width;

                Vector3 center;
                center.x = (_x + _z * 0.5f - _z / 2) * (HexMetrics.innerRadius * 2f);
                center.y = 1.0f;
                center.z = _z * (HexMetrics.outerRadius * 1.5f);

                Province province = core.world.GetProvince(_x, _z);

                Province fromProvince = (Province)playerPerson.person.place;
                List<Province> path = HexPathFinder.GetPath(fromProvince, province, core.world.width, core.world.height, core.world.children.Cast<Province>().ToList());
                if (path.Count > 0)
                {
                    for (int i = 1; i < path.Count; i++)
                    {
                        playerPerson.person.commands.Enqueue(new Move(path[i]));
                    }
                }
            }
        }
    }
}
