using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	void Start () {
        GameArea.Init();
        new KhetObjectType("Pyramid");
        #region //HACK Test region
        new KhetObject(0, 0, KhetObjectType.Types[0], KhetColor.Red, Quaternion.identity);
        new KhetObject(1, 1, KhetObjectType.Types[0], KhetColor.Silver, Quaternion.identity);
        Invoke("Test", 2);
        #endregion
    }
    void Test()//HACK Testing function
    {
        KhetServer.Instance.OnMove(0, 0, 1, 0);
    }

}
