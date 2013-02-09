using UnityEngine;
using System.Collections;

public enum KhetColor
{
    Red,
    Silver
}

public enum KhetModifier
{
    None,
    SilverBlock,
    RedBlock
}

public class KhetObject {
    public readonly KhetObjectType type;
    public readonly GameObject obj;
    public KhetObject(int x, int y, KhetObjectType type, KhetColor color, Quaternion rotation)
    {
        if (GameArea.area[x, y].obj != null)
        {
            return;
        }
        if ((GameArea.area[x, y].mod == KhetModifier.RedBlock && color == KhetColor.Silver) || (GameArea.area[x, y].mod == KhetModifier.SilverBlock && color == KhetColor.Red)) return;
        GameArea.area[x, y].obj = this;
        this.type = type;
        obj = (GameObject)GameObject.Instantiate(type.mesh, new Vector3(x * 10, 0, y * 10), rotation);
        if (color == KhetColor.Red)
        {
            obj.GetComponent<KhetGameObject>().SetMaterial(type.RedMaterial);
        }
        else
        {
            obj.GetComponent<KhetGameObject>().SetMaterial(type.SilverMaterial);
        }
    }
}
