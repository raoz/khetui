using UnityEngine;
using System.Collections;

public class GameGUIManager : MonoBehaviour {
    public static bool ShowObjectTools;
    public static Action RotateClockwise;
    public static Action RotateCounterClockwise;
    public static string GameResult = "";
    public static bool areyousuredialog = false;
    public GUIStyle resultstyle;
    void OnGUI()
    {
        if (GUI.Button(new Rect(5, 5, 200, 20), "Exit to main menu"))
            areyousuredialog = true;
            //Application.LoadLevel(0);
        if (areyousuredialog)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Are you sure you want to exit");
            if (GUI.Button(new Rect(Screen.width / 2 - 95, Screen.height / 2 , 90, 45), "YES"))
            {
                areyousuredialog = false;
                Application.LoadLevel(0);
            }
            if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2, 90, 45), "NO"))
                areyousuredialog = false;
        }
        GUI.Box(new Rect(Screen.width - 200, 5, 195, 80), "Turn id: " + KhetServer.Instance.turnid.ToString());
        if (ShowObjectTools)
        {            
            if (GUI.Button(new Rect(Screen.width - 195, 30, 190, 20), "Rotate clockwise"))
                RotateClockwise();
            if(GUI.Button(new Rect(Screen.width - 195, 55, 190, 20), "Rotate counterclockwise"))
                RotateCounterClockwise();
        }
        GUI.Label(new Rect(5, Screen.height / 2 - 100, Screen.width - 10, 200), GameResult, resultstyle);
    }
}
