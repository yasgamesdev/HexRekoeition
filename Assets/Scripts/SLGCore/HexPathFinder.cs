using System;
using System.Collections.Generic;
using System.Linq;

public class HexPathFinder
{
    static ANode[] nodes;

    public static List<Province> GetPath(Province fromProvince, Province toProvince, int width, int height, List<Province> provinces)
    {
        if (nodes == null)
        {
            nodes = new ANode[width * height];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    nodes[x + z * width] = new ANode(x, z, provinces[x + z * width]);
                }
            }
        }

        foreach (ANode node in nodes)
        {
            node.state = ANodeState.None;
            node.parent = null;
        }

        nodes[fromProvince.i].C = 0;
        nodes[fromProvince.i].H = Province.GetDistance(fromProvince, toProvince);
        nodes[fromProvince.i].state = ANodeState.Open;

        while (true)
        {
            if (nodes[toProvince.i].state == ANodeState.Open)
            {
                break;
            }
            else
            {
                int minS = nodes.Where(x => x.state == ANodeState.Open).Min(x => x.S);
                ANode minNode = nodes.Where(x => x.state == ANodeState.Open && x.S == minS).OrderBy(x => x.C).ToArray()[0];
                minNode.state = ANodeState.Close;

                OpenNode(minNode.x + (minNode.z % 2), minNode.z + 1, nodes, minNode, toProvince, width, height, provinces);
                OpenNode(minNode.x + 1, minNode.z, nodes, minNode, toProvince, width, height, provinces);
                OpenNode(minNode.x + (minNode.z % 2), minNode.z - 1, nodes, minNode, toProvince, width, height, provinces);
                OpenNode(minNode.x - ((minNode.z + 1) % 2), minNode.z - 1, nodes, minNode, toProvince, width, height, provinces);
                OpenNode(minNode.x - 1, minNode.z, nodes, minNode, toProvince, width, height, provinces);
                OpenNode(minNode.x - ((minNode.z + 1) % 2), minNode.z + 1, nodes, minNode, toProvince, width, height, provinces);
            }
        }

        return CreatePath(nodes[toProvince.i]);
    }

    static void OpenNode(int x, int z, ANode[] nodes, ANode minNode, Province toProvince, int width, int height, List<Province> provinces)
    {
        if (0 <= x && x < width && 0 <= z && z < height)
        {
            if (nodes[x + width * z].state == ANodeState.None)
            {
                int index = x + width * z;
                if(provinces[index].IsRoad)
                {
                    nodes[index].C = minNode.C + provinces[index].Terrain == TerrainType.Sea ? MovementCost.RoadSea : MovementCost.RoadLand;
                }
                else
                {
                    nodes[index].C = minNode.C + (provinces[index].Terrain == TerrainType.Sea ? MovementCost.Sea : MovementCost.Land);
                }
                nodes[index].H = Province.GetDistance(minNode.province, toProvince);
                nodes[index].parent = minNode;
                nodes[index].state = ANodeState.Open;
            }
        }
    }

    static List<Province> CreatePath(ANode node)
    {
        List<Province> provinces = new List<Province>();

        ANode targetNode = node;
        while(true)
        {
            if(targetNode != null)
            {
                provinces.Add(targetNode.province);
                targetNode = targetNode.parent;
            }
            else
            {
                break;
            }
        }

        provinces.Reverse();

        return provinces;
    }

    public static int GetMovementCost(List<Province> path)
    {
        return path.Sum(x => GetMovementCost(x));
    }

    public static int GetMovementCost(Province province)
    {
        if(province.IsRoad)
        {
            return province.Terrain == TerrainType.Sea ? MovementCost.RoadLand : MovementCost.RoadLand;
        }
        else
        {
            return province.Terrain == TerrainType.Sea ? MovementCost.Sea : MovementCost.Land;
        }
    }
}