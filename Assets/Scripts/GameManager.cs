using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private AreaManager areaManagerScript;
	// Use this for initialization
	void Start () {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            areaManagerScript.DiscardPlayArea();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            areaManagerScript.RenewDeck();
        }
    }
}
