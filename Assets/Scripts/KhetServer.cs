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
    public int turnid = 0;
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
        KhetObject obj = GameArea.area[startX, startY].obj;
        if (GameArea.area[endX, endY].mod == KhetModifier.RedBlock && turn == KhetColor.Silver)
            return false;
        if (GameArea.area[endX, endY].mod == KhetModifier.SilverBlock && turn == KhetColor.Red)
            return false;
        if (GameArea.area[endX, endY].obj != null && obj.type.name != "Scarab")
            return false;
        if (Mathf.Abs(startX - endX) > 1 || Mathf.Abs(startY - endY) > 1 || (Mathf.Abs(startX - endX) < 1 && Mathf.Abs(startY - endY) < 1))
            return false;
        if (obj.color != turn)
            return false;
        if (obj.type.name == "Sphinx")
            return false;
        if (obj.type.name == "Scarab" && GameArea.area[endX, endY].obj != null)
        {
            KhetColor toswitch = GameArea.area[endX, endY].obj.color;
            if (toswitch == KhetColor.Red && GameArea.area[startX, startY].mod == KhetModifier.SilverBlock)
                return false;
            if (toswitch == KhetColor.Silver && GameArea.area[startX, startY].mod == KhetModifier.RedBlock)
                return false;
            iTween.MoveTo(obj.obj, new Vector3(endX * 10, 0, endY * 10), 1.1f);
            obj.x = endX;
            obj.y = endY;
            iTween.MoveTo(GameArea.area[endX, endY].obj.obj, new Vector3(startX * 10, 0, startY * 10), 1.1f);
            GameArea.area[endX, endY].obj.x = startX;
            GameArea.area[endX, endY].obj.y = startY;
            KhetObject temp = GameArea.area[startX, startY].obj;
            GameArea.area[startX, startY].obj = GameArea.area[endX, endY].obj;
            GameArea.area[endX, endY].obj = temp;
            Debug.Log(GameArea.area[startX, startY].obj.type.name);
            EndTurn();
            return true;
        }        
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
        Debug.Log(clockwise);
        KhetObject obj = GameArea.area[x, y].obj;
        if (obj.color != turn)
            return false;
        if (obj.type.name == "Sphinx")
        {
            short newdirection;
            //Debug.Log(obj.direction);
            if (clockwise == true) 
                newdirection = (short)((obj.direction + 1) % 4);
            else 
                newdirection = (short)((obj.direction - 1 + 4) % 4);
            //Debug.Log(newdirection);
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
        if (clockwise == true) 
            obj.direction = (short)((obj.direction + 1) % 4);
        else 
            obj.direction = (short)((obj.direction - 1 + 4) % 4);
        iTween.RotateTo(obj.obj, new Vector3(obj.obj.transform.rotation.eulerAngles.x, obj.direction * 90, 0), 1.1f);
        EndTurn();
        return true;
    }
    void EndTurn()
    {
        turnid++;
        if (turnid > CurrentGameSettings.Instance.turnslimit && CurrentGameSettings.Instance.limitturns)
        {
            Result result = Result.Draw;
            Debug.Log(result);
            silver.GameOver(result);
            red.GameOver(result);
            silver.StopInput();
            red.StopInput();
        }
        if (turn == KhetColor.Red)
        {
            turn = KhetColor.Silver;
            red.StopInput();
            GameMaster.ToInvoke.Add(new WaitedCall(silver.StartInput,2f));
            GameMaster.ToInvoke.Add(new WaitedCall(FireRedLaser, 1.2f));
        }
        else
        {
            turn = KhetColor.Red;
            silver.StopInput();
            GameMaster.ToInvoke.Add(new WaitedCall(red.StartInput, 2f));
            GameMaster.ToInvoke.Add(new WaitedCall(FireSilverLaser, 1.2f));
        }
    }   

    void FireRedLaser()
    {
        int direction = GameArea.area[0, 7].obj.direction;
        Laser(0, 7, direction);
    }
    void FireSilverLaser()
    {
        int direction = GameArea.area[9, 0].obj.direction;
        Laser(9, 0, direction);
    }
    void Laser(int x, int y, int direction)
    {        
        List<Vector3> laserpath = new List<Vector3>();
        laserpath.Add(new Vector3(x * 10, 4, y * 10));
        KhetObject khethit;
        GameObject hit = null;
        try
        {
            Debug.Log("----");
            x += direction == 1 ? 1 : direction == 3 ? -1 : 0;
            y += direction == 0 ? 1 : direction == 2 ? -1 : 0;
            while (GameArea.area[x, y].obj == null || GameArea.area[x, y].obj.HasMirror(direction))
            {
                Debug.Log(x + ":" + y + " towards " + direction);
                if (GameArea.area[x, y].obj != null  && GameArea.area[x, y].obj.HasMirror(direction))
                {
                    laserpath.Add(new Vector3(x  * 10, 4, y * 10));
                    direction = GameArea.area[x, y].obj.GetMirroredLaser(direction);
                }
                x += direction == 1 ? 1 : direction == 3 ? -1 : 0;
                y += direction == 0 ? 1 : direction == 2 ? -1 : 0;
            }
            Debug.Log(x + ":" + y);
            Debug.Log("----");
            khethit = GameArea.area[x, y].obj;
            laserpath.Add(new Vector3(x * 10, 4, y * 10));
            hit = khethit.obj;
        }
        catch ( System.Exception e)
        {
            Debug.Log(e.ToString());
            x += direction == 1 ? 1000 : direction == 3 ? -1000 : 0;
            y += direction == 0 ? 1000 : direction == 2 ? -1000: 0;
            laserpath.Add(new Vector3(x * 10, 4, y * 10));
            Debug.Log(x + ":" + y);
            Debug.Log("----");
        }
        if (hit != null)
        {
            if (hit.transform.name == "Sphinx")
            {
                ShowLaser(laserpath);
                return;
            }
            if (hit.transform.name == "Board")
            {
                ShowLaser(laserpath);
                Debug.Log("Hit board");
                return;
            }
            if (hit.transform.name == "Pharaoh")
            {
                Result result;
                if (GameArea.area[x, y].obj.color == KhetColor.Red)
                    result = Result.SilverWon;
                else
                    result = Result.RedWon;
                Debug.Log(result);
                silver.GameOver(result);
                red.GameOver(result);
                silver.StopInput();
                red.StopInput();
            }
            if (!GameArea.area[x, y].obj.IsShielded(direction))
            {
                GameObject go = (GameObject)GameObject.Instantiate((Object)Resources.Load("Death"));
                go.transform.position = GameArea.area[x, y].obj.obj.transform.position;
                Object.Destroy(GameArea.area[x, y].obj.obj);
                GameArea.area[x, y].obj = null;
            }
        }
        ShowLaser(laserpath);
    }
    void ShowLaser(List<Vector3> path)
    {
        GameObject laser = new GameObject("Laser");
        LineRenderer laserrenderer = laser.AddComponent<LineRenderer>();
        laserrenderer.material.color = Color.red;
        laserrenderer.SetVertexCount(path.Count);
        for (int i = 0; i < path.Count; i++)
        {
            laserrenderer.SetPosition(i, path[i]);
        }
        Object.Destroy(laser, 0.6f);
    }
}