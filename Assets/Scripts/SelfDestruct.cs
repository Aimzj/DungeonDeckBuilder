using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroySelf());
	}
	
    IEnumerator DestroySelf()
    {
        yield return new WaitForSecondsRealtime(2f);
        Destroy(this.gameObject);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
