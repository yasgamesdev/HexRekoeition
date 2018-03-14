using System.Collections;
using System.Collections.Generic;

public class SLGCore
{
    public int chunkCountX { get; private set; }
    public int chunkCountZ { get; private set; }

    public int width { get; private set; }
    public int height { get; private set; }

    HexGrid grid;

    public SLGCore(int chunkCountX, int chunkCountZ, int chunkSizeX, int chunkSizeZ)
    {
        this.chunkCountX = chunkCountX;
        this.chunkCountZ = chunkCountZ;

        width = chunkCountX * chunkSizeX;
        height = chunkCountZ * chunkSizeZ;

        grid = new HexGrid(width, height);
    }

    public HexCell GetHexCell(int x, int y)
    {
        return grid.GetHexCell(x + y * width);
    }
}