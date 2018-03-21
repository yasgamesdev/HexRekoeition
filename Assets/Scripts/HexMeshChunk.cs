using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMeshChunk : MonoBehaviour
{
    Mesh hexMesh;
    List<Vector3> vertices;
    List<Color> colors;
    List<int> triangles;

    MeshCollider meshCollider;

    int chunkIndexX, chunkIndexZ;
    World world;

    public void Init(int chunkIndexX, int chunkIndexZ, World world)
    {
        this.chunkIndexX = chunkIndexX;
        this.chunkIndexZ = chunkIndexZ;
        this.world = world;

        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();

        Triangulate();
    }

    public void Triangulate()
    {
        if (world != null)
        {
            hexMesh.Clear();
            vertices.Clear();
            colors.Clear();
            triangles.Clear();

            for (int z = chunkIndexZ * HexMetrics.chunkSizeZ; z < (chunkIndexZ + 1) * HexMetrics.chunkSizeZ; z++)
            {
                for (int x = chunkIndexX * HexMetrics.chunkSizeX; x < (chunkIndexX + 1) * HexMetrics.chunkSizeX; x++)
                {
                    Vector3 center;
                    center.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
                    center.y = 0f;
                    center.z = z * (HexMetrics.outerRadius * 1.5f);

                    for (int i = 0; i < 6; i++)
                    {
                        AddTriangle(
                            center,
                            center + HexMetrics.corners[i],
                            center + HexMetrics.corners[i + 1]
                        );
                        AddTriangleColor(world.GetProvince(x, z).Terrain == TerrainType.Sea ? Color.blue : Color.green);
                    }
                }
            }

            hexMesh.vertices = vertices.ToArray();
            hexMesh.colors = colors.ToArray();
            hexMesh.triangles = triangles.ToArray();
            hexMesh.RecalculateNormals();
            meshCollider.sharedMesh = hexMesh;
        }
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    //private void Update()
    //{
    //    if(core != null & Input.GetMouseButtonDown(0))
    //    {
    //        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(inputRay, out hit))
    //        {
    //            Vector3 position = position = transform.InverseTransformPoint(hit.point);

    //            float x = position.x / (HexMetrics.innerRadius * 2f);
    //            float y = -x;

    //            float offset = position.z / (HexMetrics.outerRadius * 3f);
    //            x -= offset;
    //            y -= offset;

    //            int iX = Mathf.RoundToInt(x);
    //            int iY = Mathf.RoundToInt(y);
    //            int iZ = Mathf.RoundToInt(-x - y);

    //            if (iX + iY + iZ != 0)
    //            {
    //                float dX = Mathf.Abs(x - iX);
    //                float dY = Mathf.Abs(y - iY);
    //                float dZ = Mathf.Abs(-x - y - iZ);

    //                if (dX > dY && dX > dZ)
    //                {
    //                    iX = -iY - iZ;
    //                }
    //                else if (dZ > dY)
    //                {
    //                    iZ = -iX - iY;
    //                }
    //            }

    //            int index = iX + iZ * core.width + iZ / 2;
    //            Debug.Log(index + ", " + iX + ", " + iZ);
    //        }
    //    }
    //}
}
