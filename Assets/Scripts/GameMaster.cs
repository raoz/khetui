using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaitedCall
{
    public Action act;
    public float timeleft;
    public WaitedCall(Action act, float t)
    {
        this.act = act;
        timeleft = t;
    }
}

public class GameMaster : MonoBehaviour {
    public static GameMaster instance;
	void Start () {
        GameArea.Init();
        instance = this;
        KhetObjectType.ClearTypes(42);
        new KhetObjectType("Pyramid", CurrentGameSettings.Instance.simplified3D);
        new KhetObjectType("Sphinx", CurrentGameSettings.Instance.simplified3D);
        new KhetObjectType("Anubis", CurrentGameSettings.Instance.simplified3D);
        new KhetObjectType("Pharaoh", CurrentGameSettings.Instance.simplified3D);
        new KhetObjectType("Scarab", CurrentGameSettings.Instance.simplified3D, "Scarab");
        
        new KhetObject(9, 0, KhetObjectType.Types[1], KhetColor.Silver, 0);
        new KhetObject(2, 0, KhetObjectType.Types[0], KhetColor.Silver, 3);
        new KhetObject(7, 1, KhetObjectType.Types[0], KhetColor.Silver, 0);
        new KhetObject(2, 3, KhetObjectType.Types[0], KhetColor.Silver, 3);
        new KhetObject(9, 3, KhetObjectType.Types[0], KhetColor.Silver, 2);
        new KhetObject(2, 4, KhetObjectType.Types[0], KhetColor.Silver, 2);
        new KhetObject(9, 4, KhetObjectType.Types[0], KhetColor.Silver, 3);
        new KhetObject(3, 5, KhetObjectType.Types[0], KhetColor.Silver, 3);
        new KhetObject(3, 0, KhetObjectType.Types[2], KhetColor.Silver, 0);
        new KhetObject(5, 0, KhetObjectType.Types[2], KhetColor.Silver, 0);
        new KhetObject(4, 0, KhetObjectType.Types[3], KhetColor.Silver, 0);
        new KhetObject(4, 3, KhetObjectType.Types[4], KhetColor.Silver, 3);
        new KhetObject(5, 3, KhetObjectType.Types[4], KhetColor.Silver, 2);
        new KhetObject(6, 2, KhetObjectType.Types[0], KhetColor.Red, 1);
        new KhetObject(0, 3, KhetObjectType.Types[0], KhetColor.Red, 1);
        new KhetObject(7, 3, KhetObjectType.Types[0], KhetColor.Red, 0);
        new KhetObject(0, 4, KhetObjectType.Types[0], KhetColor.Red, 0);
        new KhetObject(7, 4, KhetObjectType.Types[0], KhetColor.Red, 1);
        new KhetObject(2, 6, KhetObjectType.Types[0], KhetColor.Red, 2);
        new KhetObject(7, 7, KhetObjectType.Types[0], KhetColor.Red, 1);
        new KhetObject(0, 7, KhetObjectType.Types[1], KhetColor.Red, 2);
        new KhetObject(4, 7, KhetObjectType.Types[2], KhetColor.Red, 2);
        new KhetObject(6, 7, KhetObjectType.Types[2], KhetColor.Red, 2);
        new KhetObject(5, 7, KhetObjectType.Types[3], KhetColor.Red, 2);
        new KhetObject(4, 4, KhetObjectType.Types[4], KhetColor.Red, 0);
        new KhetObject(5, 4, KhetObjectType.Types[4], KhetColor.Red, 1);
        #region Blocks
        GameArea.area[0, 0].mod = KhetModifier.RedBlock;       
        GameArea.area[0, 1].mod = KhetModifier.RedBlock;
        GameArea.area[0, 2].mod = KhetModifier.RedBlock;
        GameArea.area[0, 3].mod = KhetModifier.RedBlock;
        GameArea.area[0, 4].mod = KhetModifier.RedBlock;
        GameArea.area[0, 5].mod = KhetModifier.RedBlock;
        GameArea.area[0, 6].mod = KhetModifier.RedBlock;
        GameArea.area[0, 7].mod = KhetModifier.RedBlock;
        GameArea.area[8, 0].mod = KhetModifier.RedBlock;
        GameArea.area[8, 7].mod = KhetModifier.RedBlock;
        GameArea.area[1, 0].mod = KhetModifier.SilverBlock;
        GameArea.area[1, 7].mod = KhetModifier.SilverBlock;
        GameArea.area[9, 1].mod = KhetModifier.SilverBlock;
        GameArea.area[9, 2].mod = KhetModifier.SilverBlock;
        GameArea.area[9, 3].mod = KhetModifier.SilverBlock;
        GameArea.area[9, 4].mod = KhetModifier.SilverBlock;
        GameArea.area[9, 5].mod = KhetModifier.SilverBlock;
        GameArea.area[9, 6].mod = KhetModifier.SilverBlock;
        GameArea.area[9, 7].mod = KhetModifier.SilverBlock;
        #endregion
        #region //HACK Test region
        KhetServer.Instance.SetupRedInput(new LocalUIInput(KhetColor.Red, "Player1"));
        KhetServer.Instance.SetupSilverInput(new LocalUIInput(KhetColor.Silver, "Player2"));
        KhetServer.Instance.Start();
        #endregion
    }
    
    void Update()
    {
        if (LocalUIInput.CurrentInstance != null)
            LocalUIInput.CurrentInstance.Update();
        for(int i = 0; i < ToInvoke.Count; i++)
        {
            ToInvoke[i].timeleft -= Time.deltaTime;
            if (ToInvoke[i].timeleft < 0)
            {
                ToInvoke[i].act();
                ToInvoke.RemoveAt(i);
            }
        }
    }
    public static List<WaitedCall> ToInvoke = new List<WaitedCall>();
}
