using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	void Start () {
        GameArea.Init();
        new KhetObjectType("Pyramid");
        #region //HACK Test region
        new KhetObject(0, 0, KhetObjectType.Types[0], KhetColor.Red, 0);
        new KhetObject(1, 1, KhetObjectType.Types[0], KhetColor.Silver, 0);
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
