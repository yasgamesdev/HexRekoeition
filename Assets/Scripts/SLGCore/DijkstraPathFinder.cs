using System;
using System.Collections.Generic;
using System.Linq;

public class DijkstraPathFinder
{
    static ANode[] nodes;
    static Dictionary<int, List<Edge>> edges = new Dictionary<int, List<Edge>>();

    public static void CreateEdges()
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

        List<Castle> castles = CastleRepository.Instance.GetAllCastle();
        foreach(Castle castle in castles)
        {
            foreach (ANode node in nodes)
            {
                node.state = ANodeState.None;
                node.parent = null;
            }

            edges.Add(castle.Id, new List<Edge>());

            nodes[castle.GetProvince().i].C = 0;
            nodes[castle.GetProvince().i].state = ANodeState.Open;

            while(true)
            {
                List<ANode> openNodes = nodes.Where(x => x.state == ANodeState.Open).ToList();

                if(openNodes.Count == 0)
                {
                    break;
                }

                int minCost = openNodes.Min(x => x.C);
                ANode minNode = openNodes.First(x => x.C == minCost);
                Province minProvince = minNode.province;

                CheckNeighboringProvince(minNode, minProvince.x + (minProvince.z % 2), minProvince.z + 1);
                CheckNeighboringProvince(minNode, minProvince.x + 1, minProvince.z);
                CheckNeighboringProvince(minNode, minProvince.x + (minProvince.z % 2), minProvince.z - 1);
                CheckNeighboringProvince(minNode, minProvince.x - ((minProvince.z + 1) % 2), minProvince.z - 1);
                CheckNeighboringProvince(minNode, minProvince.x - 1, minProvince.z);
                CheckNeighboringProvince(minNode, minProvince.x - ((minProvince.z + 1) % 2), minProvince.z + 1);

                minNode.state = ANodeState.Close;
            }

            List<ANode> neightboringCastleNodes = nodes.Where(x => x.state == ANodeState.Close && x.province.ProvinceType == ProvinceType.Castle && x.province.GetCastle() != castle).ToList();
            foreach(ANode neightboringCastleNode in neightboringCastleNodes)
            {
                var path = CreatePath(neightboringCastleNode);
                Edge edge = new Edge(castle, neightboringCastleNode.province.GetCastle(), path);
                edges[castle.Id].Add(edge);
                //UnityEngine.Debug.Log("(" + edge.GetFromCastle().GetProvince().x + ", " + edge.GetFromCastle().GetProvince().z + ") > (" + edge.GetToCastle().GetProvince().x + ", " + edge.GetToCastle().GetProvince().z + ")" + " " + edge.GetPathProvinces().Count);
            }
        }
    }

    static void CheckNeighboringProvince(ANode minNode, int x, int z)
    {
        if (0 <= x && x < ProvinceRepository.Instance.Width && 0 <= z && z < ProvinceRepository.Instance.Height)
        {
            int index = x + z * ProvinceRepository.Instance.Width;

            if(nodes[index].state == ANodeState.None)
            {
                Province roadProvince = ProvinceRepository.Instance.GetProvince(x, z);
                if(roadProvince.ProvinceType != ProvinceType.None)
                {
                    nodes[index].C = minNode.C + 1;
                    nodes[index].parent = minNode;
                    nodes[index].state = roadProvince.ProvinceType == ProvinceType.Castle ? ANodeState.Close : ANodeState.Open;
                }
            }
        }
    }

    static List<Province> CreatePath(ANode node)
    {
        List<Province> provinces = new List<Province>();

        ANode targetNode = node;
        while (true)
        {
            if (targetNode != null)
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

    public static void GetPath(Castle fromCastle, Castle toCastle)
    {
        List<Castle> castles = CastleRepository.Instance.GetAllCastle();

        ANode[] castleNodes = new ANode[castles.Count];
        for(int i=0; i<castleNodes.Length; i++)
        {
            Province castleProvince = castles[i].GetProvince();
            castleNodes[i] = new ANode(castleProvince.x, castleProvince.z, castleProvince);
        }

        ANode startNode = GetANode(castleNodes, fromCastle.GetProvince().x, fromCastle.GetProvince().z);
        startNode.C = 0;
        startNode.state = ANodeState.Close;

        ANode goalNode = GetANode(castleNodes, toCastle.GetProvince().x, toCastle.GetProvince().z);

        while (true)
        {
            if(goalNode.state == ANodeState.Close)
            {
                break;
            }

            int minCost = int.MaxValue;
            Edge minEdge = null;
            foreach(ANode closeNode in castleNodes.Where(x => x.state == ANodeState.Close))
            {
                Province closeCastleProvince = closeNode.province;
                foreach(Edge edge in edges[closeCastleProvince.GetCastle().Id])
                {
                    Province toProvince = edge.GetToCastle().GetProvince();
                    ANode toNode = GetANode(castleNodes, toProvince.x, toProvince.z);
                    if(toNode.state == ANodeState.None)
                    {
                        int newCost = closeNode.C + edge.GetPathProvinces().Count - 1;
                        if(newCost < minCost)
                        {
                            minCost = newCost;
                            minEdge = edge;
                        }
                    }
                }
            }

            ANode fromMinNode = GetANode(castleNodes, minEdge.GetFromCastle().GetProvince().x, minEdge.GetFromCastle().GetProvince().z);
            ANode toMinNode = GetANode(castleNodes, minEdge.GetToCastle().GetProvince().x, minEdge.GetToCastle().GetProvince().z);

            toMinNode.C = minCost;
            toMinNode.parent = fromMinNode;
            toMinNode.state = ANodeState.Close;
        }

        var path = CreatePath(goalNode);
        path.ForEach(x => UnityEngine.Debug.Log(x.x + ", " + x.z));
    }

    static ANode GetANode(ANode[] nodes, int x, int z)
    {
        return nodes.First(y => y.x == x && y.z == z);
    }
}