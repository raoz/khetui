using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KhetObjectType{
    public static List<KhetObjectType> Types = new List<KhetObjectType>();
    public readonly Material RedMaterial;
    public readonly Material SilverMaterial;
    public readonly Object mesh;
    public readonly string name;
    readonly int id;
    static int nextid = 0;
    static int NextId
    {
        get
        {
            nextid++;
            return nextid;
        }
    }
    public KhetObjectType(string name)
    {
        id = NextId;
        this.name = name;
        RedMaterial = (Material)Resources.Load("Material/Red/" + name);
        SilverMaterial = (Material)Resources.Load("Material/Silver/" + name);
        mesh = (Object)Resources.Load("Mesh/" + name);
        Types.Add(this);
    }
}
