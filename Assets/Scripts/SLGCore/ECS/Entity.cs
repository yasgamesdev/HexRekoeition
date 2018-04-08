using System;
using System.Collections.Generic;
using System.Linq;

public class Entity
{
    Dictionary<Type, Component> components = new Dictionary<Type, Component>();

    public void AddComponent(Component component)
    {
        component.SetEntity(this);
        components.Add(component.GetType(), component);
    }

    public T GetComponent<T>() where T : Component
    {
        return (T)components[typeof(T)];
    }
}