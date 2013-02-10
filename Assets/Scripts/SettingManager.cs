using UnityEngine;
using System.Collections;

public class CurrentGameSettings
{
    static CurrentGameSettings instance;
    public static CurrentGameSettings GetInstance()
    {
        if(instance == null)
            new CurrentGameSettings();
        return instance;
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
    GUIStyle GameTitle;
    GUIStyle MenuButton;
	void Start () {
        GlobalSettings.Load();
	}

    void OnGUI()
    {
        switch (state)
        {
            case MenuState.MainMenu:
                GUI.Label(new Rect(5, 5, Screen.width - 10, Screen.height / 5 - 5), "RAOKHET", GameTitle);
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 5 + 5, 300, 35), "New game", MenuButton))
                {
                    state = MenuState.NewGame;
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 5 + 5 + 45, 300, 35), "Options", MenuButton))
                {
                    state = MenuState.NewGame;
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 5 + 5 + 80, 300, 35), "About", MenuButton))
                {
                    state = MenuState.NewGame;
                }
                break;
        }
    }
}
