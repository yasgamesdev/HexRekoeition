using System;
using System.Collections.Generic;
using System.Linq;

public class Command
{
    public virtual bool Update(Unit unit) { return true; }
}