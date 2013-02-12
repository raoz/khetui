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
    public KhetObjectType(string name, bool twodimensional, string matname = "Main")
    {
        id = NextId;
        this.name = name;
        if (!twodimensional)
        {
            RedMaterial = (Material)Resources.Load("Material/Red/" + matname);
            SilverMaterial = (Material)Resources.Load("Material/Silver/" + matname);
            mesh = (Object)Resources.Load("Mesh/" + name);
        }
        else
        {
            RedMaterial = (Material)Resources.Load("Material/2d/Red/" + name);
            SilverMaterial = (Material)Resources.Load("Material/2d/Silver/" + name);
            mesh = (Object)Resources.Load("Mesh/2d/" + name);
        }
        Types.Add(this);
    }
    public static void ClearTypes(int key)
    {
        foreach (AreaSquare sq in GameArea.area)
        {
            sq.obj = null;
        }
        if (key != 42)
            throw new System.Security.SecurityException("Trying to clear KhetObjectTypes without correct key");
        Types = new List<KhetObjectType>();
    }
}
