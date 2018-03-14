using UnityEngine;
using System.Collections.Generic;

public class HexMesh : MonoBehaviour
{
    SLGCore core;

    public GameObject chunkPrefab;
    GameObject[] chunks;

    public GameObject pathPrefab;
    GameObject start, goal;
    HexCoordinates startCoordinates, goalCoordinates;

    void CreateChunks()
    {
        if(core != null)
        {
            chunks = new GameObject[core.chunkCountX * core.chunkCountZ];

            for (int z = 0, i = 0; z < core.chunkCountZ; z++)
            {
                for (int x = 0; x < core.chunkCountX; x++)
                {
                    GameObject chunk = chunks[i++] = Instantiate(chunkPrefab, transform);
                    chunk.GetComponent<HexMeshChunk>().Init(x, z, core);
                }
            }
        }
    }

    public void SetData(SLGCore core)
    {
        this.core = core;

        //Triangulate();

        CreateChunks();
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

                if (start != null)
                {
                    Destroy(start);
                }
                start = Instantiate(pathPrefab, transform);
                start.transform.localPosition = center;

                startCoordinates = HexCoordinates.FromOffsetCoordinates(_x, _z);

                CheckDistance();
            }
        }
        else if (core != null & Input.GetMouseButtonDown(1))
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

                if (goal != null)
                {
                    Destroy(goal);
                }
                goal = Instantiate(pathPrefab, transform);
                goal.transform.localPosition = center;

                goalCoordinates = HexCoordinates.FromOffsetCoordinates(_x, _z);

                CheckDistance();
            }
        }
    }

    void CheckDistance()
    {
        if(start != null & goal != null)
        {
            //Debug.Log(startCoordinates.X + ", " + startCoordinates.Y + ", " + startCoordinates.Z);

            int dX = Mathf.Abs(startCoordinates.X - goalCoordinates.X);
            int dY = Mathf.Abs(startCoordinates.Y - goalCoordinates.Y);
            int dZ = Mathf.Abs(startCoordinates.Z - goalCoordinates.Z);

            int distance = (dX + dY + dZ) / 2;

            Debug.Log(distance);
        }
    }
}
