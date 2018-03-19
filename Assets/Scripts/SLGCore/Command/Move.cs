using System;
using System.Collections.Generic;
using System.Linq;

public class Move : Command
{
    public Province province;

    public Move(Province province)
    {
        this.province = province;
    }
}