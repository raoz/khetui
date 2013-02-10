using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
    public static bool ShowObjectTools;
    public static Action RotateClockwise;
    public static Action RotateCounterClockwise;
    public static string GameResult = "";
    public GUIStyle resultstyle;
    void OnGUI()
    {
        if (ShowObjectTools)
        {
            GUI.Box(new Rect(Screen.width - 200, 5, 195, 60), GUIContent.none);
            if (GUI.Button(new Rect(Screen.width - 195, 10, 190, 20), "Rotate clockwise"))
                RotateClockwise();
            if(GUI.Button(new Rect(Screen.width - 195, 35, 190, 20), "Rotate counterclockwise"))
                RotateCounterClockwise();
        }
        GUI.Label(new Rect(5, Screen.height / 2 - 100, Screen.width - 10, 200), GameResult, resultstyle);
    }
}
