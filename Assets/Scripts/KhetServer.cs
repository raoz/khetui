using UnityEngine;
using System.Collections;
/// <summary>
/// A singelton (Yes, I know it's bad, but I'm in a hurry) for game mechanics server
/// </summary>
/// 
public delegate bool Move(int startX, int startY, int endX, int EndY);
public delegate bool Rotate(int x, int y, bool direction);
public delegate void Action();

public class KhetServer
{
    #region Singelton
    static KhetServer instance;
    public static KhetServer Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = new KhetServer();
            instance.turn = (KhetColor)Random.Range(0, 1);
            return instance;
        }
    }
    public static void DestroyInstance(int key)
    {
        if (key == 42)
        {
            instance = null;
            return;
        }
        throw new System.Security.SecurityException("Tried to destroy Khet instance with wrong key!!!");
    }
    #endregion
    KhetColor turn;
    public bool OnMove(int startX, int startY, int endX, int endY) // HACK remove public tag
    {
        if (GameArea.area[startX, startY] == null)
            return false;
        if (GameArea.area[startX, startY] == null)
            return false;
        KhetObject obj = GameArea.area[startX, startY].obj;
        GameArea.area[startX, startY].obj = null;
        GameArea.area[endX, endY].obj = obj;
        obj.x = endX;
        obj.y = endY;
        iTween.MoveTo(obj.obj, new Vector3(endX * 10, 0, endY * 10), 1.1f);
        EndTurn();
        return true;
    }
    bool OnTurn(int x, int y, bool direction)
    {
        if (GameArea.area[x, y] == null)
        {
            return false;
        }
        EndTurn();
        return true;
    }
    void EndTurn()
    {
        if (turn == KhetColor.Red) turn = KhetColor.Silver;
        else turn = KhetColor.Red;
    }
}