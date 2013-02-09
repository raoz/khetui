using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool Move(int startX, int startY, int endX, int EndY);
public delegate bool Rotate(int x, int y, bool clockwise);
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
            instance.turn = KhetColor.Silver;
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
    public void Start()
    {
        silver.StartInput();
    }
    KhetColor turn;
    KhetInput silver;
    KhetInput red;
    #region ColorSpecific
    public void SetupSilverInput(KhetInput input)
    {
        silver = input;
        silver.OnMove = OnSilverMove;
        silver.OnRotate = OnSilverRotate;
        silver.OnEndTurn = OnSilverEndTurn;
    }
    public void SetupRedInput(KhetInput input)
    {
        red = input;
        red.OnMove = OnRedMove;
        red.OnRotate = OnRedRotate;
        red.OnEndTurn = OnRedEndTurn;
    }
   
    private bool OnSilverMove(int startX, int startY, int endX, int endY)
    {
        if(turn == KhetColor.Silver)
            return OnMove(startX, startY, endX, endY);
        return false;
    }
    private bool OnRedMove(int startX, int startY, int endX, int endY)
    {
        if (turn == KhetColor.Red)
            return OnMove(startX, startY, endX, endY);
        return false;
    }
    private bool OnRedRotate(int x, int y, bool direction)
    {
        if (turn == KhetColor.Red)
            return OnRotate(x, y, direction);
        return false;
    }
    private bool OnSilverRotate(int x, int y, bool direction)
    {
        if (turn == KhetColor.Silver)
            return OnRotate(x, y, direction);
        return false;
    }
    private void OnRedEndTurn()
    {
        if (turn == KhetColor.Red)
            EndTurn();
    }
    private void OnSilverEndTurn()
    {
        if (turn == KhetColor.Silver)
            EndTurn();
    }
    #endregion
    private bool OnMove(int startX, int startY, int endX, int endY)
    {
        if (GameArea.area[startX, startY].obj == null)
            return false;
        if (GameArea.area[endX, endY].obj != null)
            return false;
        if (Mathf.Abs(startX - endX) > 1 || Mathf.Abs(startY - endY) > 1)
            return false;
        KhetObject obj = GameArea.area[startX, startY].obj;
        if (obj.color != turn)
            return false;
        if (obj.type.name == "Sphinx")
            return false;
        GameArea.area[startX, startY].obj = null;
        GameArea.area[endX, endY].obj = obj;
        obj.x = endX;
        obj.y = endY;
        iTween.MoveTo(obj.obj, new Vector3(endX * 10, 0, endY * 10), 1.1f);
        EndTurn();
        return true;
    }
    
    bool OnRotate(int x, int y, bool clockwise)
    {
        if (GameArea.area[x, y] == null)
        {
            return false;
        }
        KhetObject obj = GameArea.area[x, y].obj;
        if (obj.color != turn)
            return false;
        if (obj.type.name == "Sphinx")
        {
            short newdirection;
            if (clockwise) newdirection = (short)((obj.direction + 1) % 4);
            else newdirection = (short)((obj.direction - 1 + 4) % 4);
            Debug.Log(newdirection);
            if (obj.color == KhetColor.Silver)
            {
                if (newdirection != 0 && newdirection != 3)
                    return false;
            }
            if (obj.color == KhetColor.Red)
            {
                if (newdirection != 1 && newdirection != 2)
                    return false;
            }
        }
        if (clockwise) obj.direction = (short)((obj.direction + 1) % 4);
        else obj.direction = (short)((obj.direction - 1 + 4) % 4);
        iTween.RotateTo(obj.obj, new Vector3(0, obj.direction * 90 + 180, 0), 1.1f);
        EndTurn();
        return true;
    }
    void EndTurn()
    {
        if (turn == KhetColor.Red)
        {
            turn = KhetColor.Silver;
            red.StopInput();
            silver.StartInput();            
        }
        else
        {
            turn = KhetColor.Red;
            silver.StopInput();
            red.StartInput();            
        }
    }
    void FireRedLaser()
    {
        int direction = GameArea.area[0,9].obj.direction;
        Laser(new Ray(new Vector3(0, 3, 90), new Vector3(direction % 2, 0, -((direction + 1) % 2))));
    }
    void FireSilverLaser()
    {
        int direction = GameArea.area[7, 0].obj.direction;
        Laser(new Ray(new Vector3(70, 3, 0), new Vector3(-direction, 0, (direction + 1) % 2)));
    }
    void Laser(Ray original)
    {
        LineRenderer laser = GameObject.Find("Laser").GetComponent<LineRenderer>();
        
        RaycastHit hit;
        Physics.Raycast(original, out hit, 100);
        
    }
}