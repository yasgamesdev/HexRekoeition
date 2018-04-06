using System;
using System.Collections.Generic;
using System.Linq;

public class HexPathFinder
{
    static ANode[] nodes;

    public static List<Province> GetPath(Province fromProvince, Province toProvince)
    {
        int width = ProvinceRepository.Instance.Width;
        int height = ProvinceRepository.Instance.Height;
        List<Province> provinces = ProvinceRepository.Instance.GetAllProvince();

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
                if(provinces[index].ProvinceType != ProvinceType.None)
                {
                    nodes[index].C = minNode.C + provinces[index].TerrainType == TerrainType.Sea ? MovementCost.SeaRoute : MovementCost.LandRoute;
                }
                else
                {
                    nodes[index].C = minNode.C + (provinces[index].TerrainType == TerrainType.Sea ? MovementCost.Sea : MovementCost.Land);
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

    public static List<Province> GetPath(Faction faction, ref bool result, Province fromProvince, Province toProvince)
    {
        if(fromProvince.ProvinceType == ProvinceType.None || toProvince.ProvinceType == ProvinceType.None)
        {
            result = false;
            return new List<Province>();
        }

        Dictionary<Faction, bool> hostiles = Hostiles.Instance.GetHostiles(faction);

        int width = ProvinceRepository.Instance.Width;
        int height = ProvinceRepository.Instance.Height;
        List<Province> provinces = ProvinceRepository.Instance.GetAllProvince();

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
        //nodes[fromProvince.i].H = Province.GetDistance(fromProvince, toProvince);
        nodes[fromProvince.i].parent = null;
        nodes[fromProvince.i].state = ANodeState.Open;

        while (true)
        {
            List<ANode> openNodes = nodes.Where(x => x.state == ANodeState.Open).ToList();

            if (nodes[toProvince.i].state == ANodeState.Open)
            {
                break;
            }

            int minCost = int.MaxValue;
            ANode minFromNode = null, minToNode = null;
            foreach (ANode openNode in openNodes)
            {
                OpenNode(faction, hostiles, openNode.x + (openNode.z % 2), openNode.z + 1, nodes, openNode, toProvince, width, height, provinces, ref minCost, ref minFromNode, ref minToNode);
                OpenNode(faction, hostiles, openNode.x + 1, openNode.z, nodes, openNode, toProvince, width, height, provinces, ref minCost, ref minFromNode, ref minToNode);
                OpenNode(faction, hostiles, openNode.x + (openNode.z % 2), openNode.z - 1, nodes, openNode, toProvince, width, height, provinces, ref minCost, ref minFromNode, ref minToNode);
                OpenNode(faction, hostiles, openNode.x - ((openNode.z + 1) % 2), openNode.z - 1, nodes, openNode, toProvince, width, height, provinces, ref minCost, ref minFromNode, ref minToNode);
                OpenNode(faction, hostiles, openNode.x - 1, openNode.z, nodes, openNode, toProvince, width, height, provinces, ref minCost, ref minFromNode, ref minToNode);
                OpenNode(faction, hostiles, openNode.x - ((openNode.z + 1) % 2), openNode.z + 1, nodes, openNode, toProvince, width, height, provinces, ref minCost, ref minFromNode, ref minToNode);
            }

            if(minToNode == null)
            {
                result = false;
                return new List<Province>();
            }
            else
            {
                minToNode.C = minFromNode.C + 1;
                minToNode.parent = minFromNode;
                minToNode.state = ANodeState.Open;
            }
        }

        result = true;
        return CreatePath(nodes[toProvince.i]);
    }

    static void OpenNode(Faction faction, Dictionary<Faction, bool> hostiles, int x, int z, ANode[] nodes, ANode minNode, Province toProvince, int width, int height, List<Province> provinces, ref int minCost, ref ANode minFromNode, ref ANode minToNode)
    {
        if (0 <= x && x < width && 0 <= z && z < height && nodes[x + width * z].state == ANodeState.None)
        {
            int index = x + width * z;

            if (IsOpenNode(provinces[index], toProvince, hostiles))
            {
                //nodes[index].C = minNode.C + provinces[index].TerrainType == TerrainType.Sea ? MovementCost.SeaRoute : MovementCost.LandRoute;
                //nodes[index].H = Province.GetDistance(minNode.province, toProvince);
                //nodes[index].parent = minNode;
                //nodes[index].state = ANodeState.Open;
                int cost = minNode.C + 1;
                if(cost < minCost)
                {
                    minCost = cost;
                    minFromNode = minNode;
                    minToNode = nodes[index];
                }
            }
            else
            {
                nodes[index].state = ANodeState.Close;
            }
        }
    }

    static bool IsOpenNode(Province province, Province toProvince, Dictionary<Faction, bool> hostiles)
    {
        if(province == toProvince)
        {
            return true;
        }
        else
        {
            if(province.ProvinceType == ProvinceType.Road || province.ProvinceType == ProvinceType.Town)
            {
                return true;
            }
            else if(province.ProvinceType == ProvinceType.None)
            {
                return false;
            }
            else
            {
                Castle castle = province.GetCastle();
                return !hostiles[castle.GetFaction()];
            }
        }
    }
}