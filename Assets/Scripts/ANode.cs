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

    public ANode(int x, int z)
    {
        state = ANodeState.None;
        this.x = x;
        this.z = z;
    }
}

public enum ANodeState
{
    None,
    Open,
    Close
}