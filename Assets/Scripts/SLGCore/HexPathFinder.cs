using System;
using System.Collections.Generic;
using System.Linq;

public class HexPathFinder
{
    public const int RoadCost = 1;
    public const int LandCost = 3;
    public const int SeaCost = 9;

    static ANode[] nodes;

    public static List<HexCell> GetPath(HexCell fromCell, HexCell toCell, int width, int height, HexCell[] cells)
    {
        if (nodes == null)
        {
            nodes = new ANode[width * height];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    nodes[x + z * width] = new ANode(x, z, cells[x + z * width]);
                }
            }
        }

        foreach (ANode node in nodes)
        {
            node.state = ANodeState.None;
            node.parent = null;
        }

        nodes[fromCell.i].C = 0;
        nodes[fromCell.i].H = HexCell.GetDistance(fromCell, toCell);
        nodes[fromCell.i].state = ANodeState.Open;

        while (true)
        {
            if (nodes[toCell.i].state == ANodeState.Open)
            {
                break;
            }
            else
            {
                int minS = nodes.Where(x => x.state == ANodeState.Open).Min(x => x.S);
                ANode minNode = nodes.Where(x => x.state == ANodeState.Open && x.S == minS).OrderBy(x => x.C).ToArray()[0];
                minNode.state = ANodeState.Close;

                OpenNode(minNode.x + (minNode.z % 2), minNode.z + 1, nodes, minNode, toCell, width, height, cells);
                OpenNode(minNode.x + 1, minNode.z, nodes, minNode, toCell, width, height, cells);
                OpenNode(minNode.x + (minNode.z % 2), minNode.z - 1, nodes, minNode, toCell, width, height, cells);
                OpenNode(minNode.x - ((minNode.z + 1) % 2), minNode.z - 1, nodes, minNode, toCell, width, height, cells);
                OpenNode(minNode.x - 1, minNode.z, nodes, minNode, toCell, width, height, cells);
                OpenNode(minNode.x - ((minNode.z + 1) % 2), minNode.z + 1, nodes, minNode, toCell, width, height, cells);
            }
        }

        return CreatePath(nodes[toCell.i]);
    }

    static void OpenNode(int x, int z, ANode[] nodes, ANode minNode, HexCell toCell, int width, int height, HexCell[] cells)
    {
        if (0 <= x && x < width && 0 <= z && z < height)
        {
            if (nodes[x + width * z].state == ANodeState.None)
            {
                int index = x + width * z;
                if(cells[index].building != Building.None)
                {
                    nodes[index].C = minNode.C + RoadCost;
                }
                else
                {
                    nodes[index].C = minNode.C + (cells[index].terrain == Terrain.Sea ? SeaCost : LandCost);
                }
                nodes[index].H = HexCell.GetDistance(minNode.cell, toCell);
                nodes[index].parent = minNode;
                nodes[index].state = ANodeState.Open;
            }
        }
    }

    static List<HexCell> CreatePath(ANode node)
    {
        List<HexCell> cells = new List<HexCell>();

        ANode targetNode = node;
        while(true)
        {
            if(targetNode != null)
            {
                cells.Add(targetNode.cell);
                targetNode = targetNode.parent;
            }
            else
            {
                break;
            }
        }

        cells.Reverse();

        return cells;
    }
}