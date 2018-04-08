using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Command
{
    public abstract CommandExecResult Exec(Component component);
}