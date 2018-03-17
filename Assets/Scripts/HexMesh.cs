using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HexMesh : MonoBehaviour
{
    SLGCore core;

    public GameObject chunkPrefab;
    GameObject[] chunks;

    public GameObject castlePrefab, townPrefab, roadPrefab;

    public void SetData(SLGCore core)
    {
        this.core = core;

        CreateChunks();

        CreateBuilding();
    }

    void CreateChunks()
    {
        if(core != null)
        {
            chunks = new GameObject[HexMetrics.chunkCountX * HexMetrics.chunkCountZ];

            for (int z = 0, i = 0; z < HexMetrics.chunkCountZ; z++)
            {
                for (int x = 0; x < HexMetrics.chunkCountX; x++)
                {
                    GameObject chunk = chunks[i++] = Instantiate(chunkPrefab, transform);
                    chunk.GetComponent<HexMeshChunk>().Init(x, z, core);
                }
            }
        }
    }

    void CreateBuilding()
    {
        for (int z = 0; z < core.height; z++)
        {
            for (int x = 0; x < core.width; x++)
            {
                HexCell cell = core.GetHexCell(x, z);

                if (cell.building == Building.Castle)
                {
                    Vector3 center;
                    center.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
                    center.y = 0.1f;
                    center.z = z * (HexMetrics.outerRadius * 1.5f);

                    GameObject obj = Instantiate(castlePrefab, transform);
                    obj.transform.localPosition = center;
                    obj.transform.localEulerAngles = new Vector3(90, 0, 0);
                }
                else if(cell.building == Building.Town)
                {
                    Vector3 center;
                    center.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
                    center.y = 0.1f;
                    center.z = z * (HexMetrics.outerRadius * 1.5f);

                    GameObject obj = Instantiate(townPrefab, transform);
                    obj.transform.localPosition = center;
                    obj.transform.localEulerAngles = new Vector3(90, 0, 0);
                }
                else if(cell.building == Building.Road)
                {
                    Vector3 center;
                    center.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
                    center.y = 0.1f;
                    center.z = z * (HexMetrics.outerRadius * 1.5f);

                    GameObject obj = Instantiate(roadPrefab, transform);
                    obj.transform.localPosition = center;
                    obj.transform.localEulerAngles = new Vector3(90, 0, 0);
                }
            }
        }
    }

    bool IsSeaTerrain(int x, int z)
    {
        if (0 <= x && x < core.width && 0 <= z && z < core.height)
        {
            return core.GetHexCell(x, z).terrain == Terrain.Sea;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        if (core != null & Input.GetMouseButtonDown(0))
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                Vector3 position = position = transform.InverseTransformPoint(hit.point);

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

                int index = iX + iZ * core.width + iZ / 2;
                //Debug.Log(index + ", " + iX + ", " + iZ);

                int _x = index % core.width;
                int _z = index / core.width;

                Vector3 center;
                center.x = (_x + _z * 0.5f - _z / 2) * (HexMetrics.innerRadius * 2f);
                center.y = 1.0f;
                center.z = _z * (HexMetrics.outerRadius * 1.5f);

                HexCell cell = core.GetHexCell(_x, _z);
                Debug.Log(cell.x + ", " + cell.z);
            }
        }
    }
}
