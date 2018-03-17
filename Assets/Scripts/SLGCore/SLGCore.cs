using System.Collections;
using System.Collections.Generic;

public class SLGCore
{
    public int width { get; private set; }
    public int height { get; private set; }

    HexGrid grid;

    public SLGCore(int width, int height)
    {
        this.width = width;
        this.height = height;

        grid = new HexGrid(width, height);
    }

    public HexCell GetHexCell(int x, int z)
    {
        return grid.GetHexCell(x + z * width);
    }
}