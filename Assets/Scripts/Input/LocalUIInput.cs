using UnityEngine;
using System.Collections;

public class LocalUIInput : KhetInput {
    static LocalUIInput currentinstance;
    public static LocalUIInput CurrentInstance
    {
        get
        {
            if (currentinstance != null)
                return currentinstance;
            return null;
        }
    }
    public LocalUIInput(KhetColor color)
    {
        this.color = color;
    }
    bool AcceptingInput = false;
    private KhetColor color;
    private GameObject highlighter;
    private GameObject selection;
    private Vector2 selpos;
    public void Update()
    {
        if (AcceptingInput)
        {
            if (selection)
                GUIManager.ShowObjectTools = true;
            else
                GUIManager.ShowObjectTools = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit, 1000);
            if (hit.collider != null)
            {
                int x = (int)Mathf.Round(hit.point.x / 10);
                int y = (int)Mathf.Round(hit.point.z / 10);
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log(hit.point);
                    if (GameArea.area[x, y].obj != null)
                    {
                        if (selection != null)
                            GameObject.Destroy(selection);
                        if (GameArea.area[x, y].obj.color == color)
                        {
                            selpos = new Vector2(x, y);
                            selection = (GameObject)GameObject.Instantiate((Object)Resources.Load("Selection"), new Vector3(x * 10, 0.5f, y * 10), Quaternion.identity);
                        }
                    }
                    else
                    {
                        if (selection != null)
                        {
                            if (OnMove((int)selpos.x, (int)selpos.y, x, y))
                                GameObject.Destroy(selection);
                        }
                    }
                }
                if (highlighter == null)
                {
                    highlighter = (GameObject)GameObject.Instantiate((Object)Resources.Load("Highlighter"), new Vector3(x * 10, 0.5f, y * 10), Quaternion.identity);
                }
                highlighter.transform.position = new Vector3(x * 10, 0.05f, y * 10);
            }
            else
            {
                GameObject.Destroy(highlighter);
            }
        }
    }
    public override void GameOver(Result result)
    {
        if(color == KhetColor.Silver)
        {
            if (result == Result.SilverWon)
                GUIManager.GameResult = "Silver won!";
            else if (result == Result.RedWon)
                GUIManager.GameResult = "Red won!";
            else
                GUIManager.GameResult = "Draw!";
        }
        if (color == KhetColor.Red)
        {
            if (result == Result.SilverWon)
                GUIManager.GameResult = "Silver won!";
            else if (result == Result.RedWon)
                GUIManager.GameResult = "Red won!";
            else
                GUIManager.GameResult = "Draw!";
        }
    }
    public void RotateClockwise()
    {
        OnRotate((int)selpos.x, (int)selpos.y, true);
        GameObject.Destroy(selection);
    }
    public void RotateCounterClockwise()
    {
        OnRotate((int)selpos.x, (int)selpos.y, false);
        GameObject.Destroy(selection);
    }
    public override void StartInput()
    {
        AcceptingInput = true;
        currentinstance = this;
        GUIManager.RotateClockwise = RotateClockwise;
        GUIManager.RotateCounterClockwise = RotateCounterClockwise;
    }
    public override void StopInput()
    {
        AcceptingInput = false;
        GUIManager.ShowObjectTools = false;
        GameObject.Destroy(highlighter);
        GameObject.Destroy(selection);
    }
}
