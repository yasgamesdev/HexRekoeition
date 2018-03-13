using System.Collections;
using System.Collections.Generic;

public class HexGrid
{
    HexCell[] cells;

    public HexGrid(int width, int height)
    {
        float[,] noise = NoiseGenerator.Generate(width, height, 4.0f, 4.0f, 2.0f, 1.0f);

        cells = new HexCell[width * height];

        for (int y = 0, i=0; y < noise.GetLength(1); y++)
        {
            for (int x = 0; x < noise.GetLength(0); x++)
            {
                cells[i++] = new HexCell(noise[x, y] >= 0 ? Terrain.Land : Terrain.Sea);
            }
        }
    }

    public HexCell GetHexCell(int i)
    {
        return cells[i];
    }
}
