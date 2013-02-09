using UnityEngine;
using System.Collections;

public class KhetGameObject : MonoBehaviour {
    public GameObject main;
	void Start () {
        if (main == null)
            main = gameObject;
	}

    public void SetMaterial(Material mat)
    {
        main.GetComponent<MeshRenderer>().material = mat;
    }
}
