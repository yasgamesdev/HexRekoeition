using System;
using System.Collections.Generic;
using System.Linq;

public class Command
{
    public virtual CommandExecResult Execute(Unit unit) { return CommandExecResult.Finish; }
}