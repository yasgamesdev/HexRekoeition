using UnityEngine;
using System.Collections.Generic;

public class HexMesh : MonoBehaviour
{
    SLGCore core;

    public GameObject chunkPrefab;
    GameObject[] chunks;

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
        if(core != null & Input.GetMouseButtonDown(0))
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
                Debug.Log(index + ", " + iX + ", " + iZ);
            }
        }
    }
}
