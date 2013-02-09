using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameArea.Init();
        new KhetObjectType("Pyramid");
        new KhetObject(0, 0, KhetObjectType.Types[0], KhetColor.Red, Quaternion.identity);
        new KhetObject(1, 1, KhetObjectType.Types[0], KhetColor.Silver, Quaternion.identity);
	}

    //void Update () {
	
    //}
}
