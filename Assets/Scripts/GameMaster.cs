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
        new KhetObjectType("Pyramid");
        new KhetObjectType("Sphinx");
        new KhetObjectType("Anubis");
        new KhetObjectType("Pharaoh");
        
        new KhetObject(9, 0, KhetObjectType.Types[1], KhetColor.Silver, 0);
        new KhetObject(2, 0, KhetObjectType.Types[0], KhetColor.Silver, 3);
        new KhetObject(3, 0, KhetObjectType.Types[2], KhetColor.Silver, 0);
        new KhetObject(5, 0, KhetObjectType.Types[2], KhetColor.Silver, 0);
        new KhetObject(4, 0, KhetObjectType.Types[3], KhetColor.Silver, 0);
        new KhetObject(0, 7, KhetObjectType.Types[1], KhetColor.Red, 2);
        new KhetObject(4, 7, KhetObjectType.Types[2], KhetColor.Red, 2);
        new KhetObject(6, 7, KhetObjectType.Types[2], KhetColor.Red, 2);
        new KhetObject(5, 7, KhetObjectType.Types[3], KhetColor.Red, 2);
        #region //HACK Test region
        KhetServer.Instance.SetupRedInput(new LocalUIInput(KhetColor.Red));
        KhetServer.Instance.SetupSilverInput(new LocalUIInput(KhetColor.Silver));
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
