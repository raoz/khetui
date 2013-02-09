using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	void Start () {
        GameArea.Init();
        new KhetObjectType("Pyramid");
        new KhetObjectType("Sphinx");
        
        new KhetObject(9, 0, KhetObjectType.Types[1], KhetColor.Silver, 0);
        new KhetObject(2, 0, KhetObjectType.Types[0], KhetColor.Silver, 3);
        new KhetObject(0, 7, KhetObjectType.Types[1], KhetColor.Red, 2);
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
    }

}
