using System;
using System.Collections.Generic;
using System.Linq;

public class Component
{
    Entity parent;

    public void SetEntity(Entity parent)
    {
        this.parent = parent;
    }
}