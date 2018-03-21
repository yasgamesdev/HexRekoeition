using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HexMesh : MonoBehaviour
{
    World world;

    public GameObject chunkPrefab;
    GameObject[] chunks;

    public GameObject castlePrefab, townPrefab, roadPrefab;

    public void SetData(World world)
    {
        this.world = world;

        CreateChunks();

        CreateBuilding();
    }

    void CreateChunks()
    {
        if(world != null)
        {
            chunks = new GameObject[HexMetrics.chunkCountX * HexMetrics.chunkCountZ];

            for (int z = 0, i = 0; z < HexMetrics.chunkCountZ; z++)
            {
                for (int x = 0; x < HexMetrics.chunkCountX; x++)
                {
                    GameObject chunk = chunks[i++] = Instantiate(chunkPrefab, transform);
                    chunk.GetComponent<HexMeshChunk>().Init(x, z, world);
                }
            }
        }
    }

    void CreateBuilding()
    {
        for (int z = 0; z < world.Height; z++)
        {
            for (int x = 0; x < world.Width; x++)
            {
                Province province = world.GetProvince(x, z);

                if (province.ChildPlaces.Count > 0 && province.ChildPlaces[0].Type == PlaceType.Castle)
                {
                    Vector3 center;
                    center.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
                    center.y = 0.1f;
                    center.z = z * (HexMetrics.outerRadius * 1.5f);

                    GameObject obj = Instantiate(castlePrefab, transform);
                    obj.transform.localPosition = center;
                    obj.transform.localEulerAngles = new Vector3(90, 0, 0);
                }
                else if(province.ChildPlaces.Count > 0 && province.ChildPlaces[0].Type == PlaceType.Town)
                {
                    Vector3 center;
                    center.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
                    center.y = 0.1f;
                    center.z = z * (HexMetrics.outerRadius * 1.5f);

                    GameObject obj = Instantiate(townPrefab, transform);
                    obj.transform.localPosition = center;
                    obj.transform.localEulerAngles = new Vector3(90, 0, 0);
                }
                else if(province.IsRoad)
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

    private void Update()
    {
        if (world != null & Input.GetMouseButtonDown(0))
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

                int index = iX + iZ * world.Width + iZ / 2;

                int _x = index % world.Width;
                int _z = index / world.Width;

                Vector3 center;
                center.x = (_x + _z * 0.5f - _z / 2) * (HexMetrics.innerRadius * 2f);
                center.y = 1.0f;
                center.z = _z * (HexMetrics.outerRadius * 1.5f);

                Province province = world.GetProvince(_x, _z);
                Debug.Log(province.x + ", " + province.z);
            }
        }
    }
}
