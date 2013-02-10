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
        if (turn == KhetColor.Red)
        {
            turn = KhetColor.Silver;
            red.StopInput();
            silver.StartInput();
            GameMaster.ToInvoke.Add(new WaitedCall(FireRedLaser, 1.2f));
        }
        else
        {
            turn = KhetColor.Red;
            silver.StopInput();
            red.StartInput();
            GameMaster.ToInvoke.Add(new WaitedCall(FireSilverLaser, 1.2f));
        }
    }   

    void FireRedLaser()
    {
        int direction = GameArea.area[0, 7].obj.direction;
        Laser(new Ray(new Vector3(0, 4, 70), new Vector3(direction % 2, 0, -((direction + 1) % 2))));
    }
    void FireSilverLaser()
    {
        int direction = GameArea.area[9, 0].obj.direction;
        Laser(new Ray(new Vector3(90, 4, 0), new Vector3(-direction, 0, (direction + 1) % 2)));
    }
    void Laser(Ray original)
    {        
        List<Vector3> laserpath = new List<Vector3>();
        laserpath.Add(original.origin);
        RaycastHit hit;
        Ray ray = original;
        Physics.Raycast(ray, out hit, 100);
        while (hit.collider != null)
        {            
            laserpath.Add(hit.point);
            if (hit.collider.tag != "Mirror")
                break;
            ray.direction = Vector3.Reflect(ray.direction, hit.normal);           
            ray.origin = hit.point;
            Physics.Raycast(ray, out hit, 100);
        }
        if (hit.collider == null)
        {
            laserpath.Add(ray.origin + (ray.direction * 1000));
        }
        else
        {
            if (hit.transform.name == "Sphinx")
            {
                ShowLaser(laserpath);
                return;
            }
            if (hit.transform.name == "Shield")
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
            int x = (int)Mathf.Round(hit.point.x / 10);
            int y = (int)Mathf.Round(hit.point.z / 10);
            Debug.Log(x + ":" + y);
            iTween.MoveTo(GameArea.area[x, y].obj.obj, new Vector3(100, 1000, 100), 150);
            Object.Destroy(GameArea.area[x, y].obj.obj, 10);
            GameArea.area[x, y].obj = null;
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