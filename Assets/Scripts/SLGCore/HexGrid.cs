using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HexGrid
{
    int width;
    int height;

    HexCell[] cells;

    public HexGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        float[,] noise = NoiseGenerator.Generate(width, height, 4.0f, 4.0f * 0.75f, 2.0f, 1.0f);

        cells = new HexCell[width * height];

        for (int z = 0, i=0; z < noise.GetLength(1); z++)
        {
            for (int x = 0; x < noise.GetLength(0); x++)
            {
                HexCell cell = new HexCell(x, z, width);
                cell.SetTerrain(noise[x, z] >= 0 ? Terrain.Land : Terrain.Sea);
                cells[i++] = cell;
            }
        }

        SetBuilding();
    }

    public HexCell GetHexCell(int i)
    {
        return cells[i];
    }

    void SetBuilding()
    {
        SetCastle(30, 4);

        SetTown(40, 3);

        SetRoad();
    }

    void SetCastle(int castleCount, int minDistance)
    {
        List<HexCell> possibleCells = cells.Where(x => x.terrain == Terrain.Land && x.building == Building.None).ToList();

        Random rand = new Random();

        for(int i=0; i<castleCount; i++)
        {
            if(possibleCells.Count == 0)
            {
                break;
            }
            else
            {
                int index = rand.Next(possibleCells.Count);
                HexCell cell = possibleCells[index];
                cell.SetBuilding(Building.Castle);
                possibleCells.RemoveAll(x => HexCell.GetDistance(cell, x) <= minDistance);
            }
        }
    }

    void SetTown(int townCount, int minDistance)
    {
        List<HexCell> possibleCells = cells.Where(x => x.terrain == Terrain.Land && x.building == Building.None).ToList();

        Random rand = new Random();

        for (int i = 0; i < townCount; i++)
        {
            if (possibleCells.Count == 0)
            {
                break;
            }
            else
            {
                int index = rand.Next(possibleCells.Count);
                HexCell cell = possibleCells[index];
                cell.SetBuilding(Building.Town);
                possibleCells.RemoveAll(x => HexCell.GetDistance(cell, x) <= minDistance);
            }
        }
    }

    void SetRoad()
    {
        List<HexCell> castleOrTownCells = cells.Where(x => x.building == Building.Castle || x.building == Building.Town).ToList();

        List<HexCell> setRoadCells = new List<HexCell>();
        setRoadCells.Add(castleOrTownCells[0]);

        while (true)
        {
            if (setRoadCells.Count == castleOrTownCells.Count)
            {
                break;
            }
            else
            {
                List<HexCell> notSetRoadCells = new List<HexCell>();
                for (int i = 0; i < castleOrTownCells.Count; i++)
                {
                    if (!setRoadCells.Contains(castleOrTownCells[i]))
                    {
                        notSetRoadCells.Add(castleOrTownCells[i]);
                    }
                }

                int minDistance = int.MaxValue;
                HexCell minSetRoadCell = null, minNotSetRoadCell = null;

                foreach(HexCell fromCell in notSetRoadCells)
                {
                    foreach(HexCell toCell in setRoadCells)
                    {
                        int distance = HexCell.GetDistance(fromCell, toCell);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            minNotSetRoadCell = fromCell;
                            minSetRoadCell = toCell;
                        }
                    }
                }

                setRoadCells.Add(minNotSetRoadCell);

                HexPathFinder.GetPath(minNotSetRoadCell, minSetRoadCell, width, height, cells).ForEach(x => x.SetBuilding((x.building == Building.None) ? Building.Road : x.building));
            }
        }
    }
}
