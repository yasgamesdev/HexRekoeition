using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HexMesh : MonoBehaviour
{
    SLGCore core;

    public GameObject chunkPrefab;
    GameObject[] chunks;

    public GameObject pathPrefab;
    GameObject start, goal;
    HexCoordinates startCoordinates, goalCoordinates;
    int startIndex, goalIndex;

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

                //CheckDistance();
                startIndex = index;
                SetPath();

                //Debug.Log(_x + ", " + _z);
                //Debug.Log(startCoordinates.X + ", " + startCoordinates.Y + ", " + startCoordinates.Z);
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

                //CheckDistance();
                goalIndex = index;
                SetPath();
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

    const int SeaCost = 30;
    const int LandCost = 3;

    List<GameObject> path = new List<GameObject>();

    void SetPath()
    {
        if (start != null & goal != null)
        {
            ANode[] nodes = new ANode[core.width * core.height];
            for(int z =0; z<core.height; z++)
            {
                for (int x = 0; x < core.width; x++)
                {
                    nodes[x + z * core.width] = new ANode(x, z);
                }
            }

            nodes[startIndex].C = 0;
            nodes[startIndex].H = GetDistance(startIndex, goalIndex);
            nodes[startIndex].state = ANodeState.Open;

            while(true)
            {
                if(nodes[goalIndex].state == ANodeState.Open)
                {
                    break;
                }
                else
                {
                    int minS = nodes.Where(x => x.state == ANodeState.Open).Min(x => x.S);
                    ANode minNode = nodes.Where(x => x.state == ANodeState.Open && x.S == minS).OrderBy(x => x.C).ToArray()[0];
                    minNode.state = ANodeState.Close;

                    OpenNode(minNode.x + (minNode.z % 2), minNode.z + 1, nodes, minNode);
                    OpenNode(minNode.x + 1, minNode.z, nodes, minNode);
                    OpenNode(minNode.x + (minNode.z % 2), minNode.z - 1, nodes, minNode);
                    OpenNode(minNode.x - ((minNode.z + 1) % 2), minNode.z - 1, nodes, minNode);
                    OpenNode(minNode.x - 1, minNode.z, nodes, minNode);
                    OpenNode(minNode.x - ((minNode.z + 1) % 2), minNode.z + 1, nodes, minNode);
                }
            }

            path.ForEach(x => Destroy(x));
            path.Clear();

            ShowPath(nodes[goalIndex]);
        }
    }

    void OpenNode(int x, int z, ANode[] nodes, ANode minNode)
    {
        if (0 <= x && x < core.width && 0 <= z && z < core.height)
        {
            if (nodes[x + core.width * z].state == ANodeState.None)
            {
                int index = x + core.width * z;
                nodes[index].C = minNode.C + (core.GetHexCell(x, z).terrain == Terrain.Sea ? SeaCost : LandCost);
                nodes[index].H = GetDistance(index, goalIndex);
                nodes[index].parent = minNode;
                nodes[index].state = ANodeState.Open;
            }
        }
    }

    void ShowPath(ANode node)
    {
        if(node.parent != null)
        {
            ShowPath(node.parent);
        }

        Vector3 center;
        center.x = (node.x + node.z * 0.5f - node.z / 2) * (HexMetrics.innerRadius * 2f);
        center.y = 1.0f;
        center.z = node.z * (HexMetrics.outerRadius * 1.5f);

        GameObject obj = Instantiate(pathPrefab, transform);
        obj.transform.localPosition = center;
        path.Add(obj);
    }

    int GetDistance(int indexFrom, int indexTo)
    {
        HexCoordinates fromCoordinates = HexCoordinates.FromOffsetCoordinates(indexFrom % core.width, indexFrom / core.width);
        HexCoordinates toCoordinates = HexCoordinates.FromOffsetCoordinates(indexTo % core.width, indexTo / core.width);

        int dX = Mathf.Abs(fromCoordinates.X - toCoordinates.X);
        int dY = Mathf.Abs(fromCoordinates.Y - toCoordinates.Y);
        int dZ = Mathf.Abs(fromCoordinates.Z - toCoordinates.Z);

        return (dX + dY + dZ) / 2;
    }
}
