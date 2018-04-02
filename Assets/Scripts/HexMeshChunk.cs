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

    public void Init(int chunkIndexX, int chunkIndexZ)
    {
        this.chunkIndexX = chunkIndexX;
        this.chunkIndexZ = chunkIndexZ;

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
                    //AddTriangleColor(ProvinceRepository.Instance.GetProvince(x, z).TerrainType == TerrainType.Sea ? Color.blue : Color.green);

                    //Castle castle = ProvinceRepository.Instance.GetProvince(x, z).GetTerritoryCastle();
                    //Province castleProvince = castle.GetProvince();
                    //Random.InitState(castleProvince.i);
                    //float r = Random.Range(0.0f, 1.0f);
                    //float g = Random.Range(0.0f, 1.0f);
                    //float b = Random.Range(0.0f, 1.0f);
                    //AddTriangleColor(ProvinceRepository.Instance.GetProvince(x, z).TerrainType == TerrainType.Sea ? Color.blue : new Color(r, g, b));

                    Castle castle = ProvinceRepository.Instance.GetProvince(x, z).GetTerritoryCastle();
                    Person person = castle.GetJoshu();
                    var md5Hasher = System.Security.Cryptography.MD5.Create();
                    var hashed = md5Hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(person.Name));
                    var integer = System.BitConverter.ToInt32(hashed, 0);
                    Random.InitState(integer);
                    float r = Random.Range(0.0f, 1.0f);
                    float g = Random.Range(0.0f, 1.0f);
                    float b = Random.Range(0.0f, 1.0f);
                    AddTriangleColor(ProvinceRepository.Instance.GetProvince(x, z).TerrainType == TerrainType.Sea ? Color.blue : new Color(r, g, b));
                }
            }
        }

        hexMesh.vertices = vertices.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
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
