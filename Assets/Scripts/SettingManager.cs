using UnityEngine;
using System.Collections;

public class CurrentGameSettings
{
    static CurrentGameSettings instance;
    public static CurrentGameSettings Instance
    {
        get
        {
            if (instance == null)
                new CurrentGameSettings();
            return instance;
        }
    }
    public CurrentGameSettings()
    {
        instance = this;
    }
    public bool limitturns = true;
    public int turnslimit = 222;
    public bool simplified3D = true;
    public KhetInput player1;
    public KhetInput player2;
}

public static class GlobalSettings
{
    public static bool use3d = false;
    public static void Load()
    {
        if (PlayerPrefs.HasKey("Use3D"))
            use3d = PlayerPrefs.GetInt("Use3D") != 0;
    }
    public static void Save()
    {
        PlayerPrefs.SetInt("Use3D", use3d ? 1 : 0);
        PlayerPrefs.Save();
    }
}

public enum MenuState
{
    None,
    MainMenu,
    Options,
    NewGame,
    About
}

public class SettingManager : MonoBehaviour {
    MenuState state = MenuState.MainMenu;
    public GUISkin MenuSkin;
    TextAsset about;
    public Vector2 scrollPosition = Vector2.zero;
	void Start () {
        GlobalSettings.Load();
        about = (TextAsset)Resources.Load("about");
	}
    void OnGUI()
    {
        switch (state)
        {
            case MenuState.MainMenu:
                GUI.Label(new Rect(5, 5, Screen.width - 10, Screen.height / 5 - 5), "RAOKHET", MenuSkin.label);
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 5 + 5, 300, 35), "New game", MenuSkin.button))
                {
                    new CurrentGameSettings();
                    state = MenuState.NewGame;
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 5 + 5 + 45, 300, 35), "Options", MenuSkin.button))
                {
                    state = MenuState.Options;
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 5 + 5 + 85, 300, 35), "About", MenuSkin.button))
                {
                    state = MenuState.About;
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 5 + 5 + 125, 300, 35), "Exit", MenuSkin.button))
                {
                    state = MenuState.None;
                    Application.Quit();
                }
                break;
            case MenuState.About:
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, MenuSkin.scrollView);
                GUILayout.Label(about.text, MenuSkin.textArea, GUILayout.ExpandHeight(true));
                if (GUILayout.Button("Back", MenuSkin.button))
                    state = MenuState.MainMenu;
                GUILayout.EndScrollView();
                break;
            case MenuState.Options:
                GlobalSettings.use3d = GUI.Toggle(new Rect(Screen.width / 2 - 100, 5, 200, 20), GlobalSettings.use3d, "Use 3D style(experimental)", MenuSkin.toggle);
                if (GUI.Button(new Rect(Screen.width / 2 - 200, 30, 400, 40), "Save and exit to main menu", MenuSkin.button))
                {
                    GlobalSettings.Save();
                    state = MenuState.MainMenu;
                }
                break;
            case MenuState.NewGame:
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
                //GUILayout.BeginHorizontal();
                //GUILayout.FlexibleSpace();
                //CurrentGameSettings.Instance.simplified3D = GUILayout.Toggle(CurrentGameSettings.Instance.simplified3D, "Use simplified version of the 3D rule(Look in tutorial)", MenuSkin.toggle);
                //GUILayout.FlexibleSpace();
                //GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                CurrentGameSettings.Instance.limitturns = GUILayout.Toggle(CurrentGameSettings.Instance.limitturns, "End game as draw after certain amount of turns", MenuSkin.toggle);
                if (CurrentGameSettings.Instance.limitturns)
                    CurrentGameSettings.Instance.turnslimit = int.Parse(GUILayout.TextField(CurrentGameSettings.Instance.turnslimit.ToString(), MenuSkin.textField));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Start a new game", MenuSkin.button))
                {
                    KhetServer.DestroyInstance(42);
                    KhetObjectType.ClearTypes(42);
                    Application.LoadLevel(1);
                }
                if(GUILayout.Button("Back", MenuSkin.button))
                    state = MenuState.MainMenu;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Support for multiplayer and bots will be in next version, you will be able to get it from https://bitbucket.org/raoz/khetui", MenuSkin.textArea);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
                break;
        }
    }
}
