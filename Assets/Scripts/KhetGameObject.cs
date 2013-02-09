using UnityEngine;
using System.Collections;

public class KhetGameObject : MonoBehaviour {
    public GameObject[] retextured;
	void Start () {
        if (retextured.Length == 0)
            throw new System.ApplicationException("Missing texture target");
	}

    public void SetMaterial(Material mat)
    {
        foreach(GameObject go in retextured)
            go.GetComponent<MeshRenderer>().material = mat;
    }
}
