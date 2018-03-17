using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ANode
{
    public ANodeState state;
    public int x, z;
    public int C, H;
    public int S
    {
        get
        {
            return C + H;
        }
    }
    public ANode parent;
    public HexCell cell;

    public ANode(int x, int z, HexCell cell)
    {
        state = ANodeState.None;
        this.x = x;
        this.z = z;
        this.cell = cell;
    }
}

public enum ANodeState
{
    None,
    Open,
    Close
}