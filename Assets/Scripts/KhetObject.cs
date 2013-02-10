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
    public readonly KhetColor color;
    public int x;
    public int y;
    public short direction;
    public KhetObject(int x, int y, KhetObjectType type, KhetColor color, short direction)
    {
        this.x = x;
        this.y = y;
        this.color = color;
        if (GameArea.area[x, y].obj != null)
        {
            return;
        }
        if ((GameArea.area[x, y].mod == KhetModifier.RedBlock && color == KhetColor.Silver) || (GameArea.area[x, y].mod == KhetModifier.SilverBlock && color == KhetColor.Red)) return;
        GameArea.area[x, y].obj = this;
        this.type = type;
        obj = (GameObject)GameObject.Instantiate(type.mesh, new Vector3(x * 10, 0, y * 10), Quaternion.Euler(type.name == "Anubis" ? 270 : 0, 90 * direction, 0));
        obj.transform.tag = "KhetObject";
        obj.transform.name = type.name;
        this.direction = direction;
        if (color == KhetColor.Red)
        {
            obj.GetComponent<KhetGameObject>().SetMaterial(type.RedMaterial);
        }
        else
        {
            obj.GetComponent<KhetGameObject>().SetMaterial(type.SilverMaterial);
        }
    }
    public bool HasMirror(int dir)
    {
        return (type.name == "Pyramid" && ((dir + 1) % 4 == direction || (dir + 2) % 4 == direction)) || type.name == "Scarab";
    }
    public bool IsShielded(int dir)
    {
        return type.name == "Anubis" && (dir + 2) % 4 == direction;
    }
    public int GetMirroredLaser(int dir)
    {
        if (direction == 0 || direction == 2)
            return dir == 0 ? 3 : dir == 1 ? 2 : dir == 2 ? 1 : dir == 3 ? 0 : -1;
        if (direction == 1 || direction == 3)
            return dir == 0 ? 1 : dir == 1 ? 0 : dir == 2 ? 3 : 0;
        return -1;
    }
}
