using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {
    // Use this for initialization
    void Start () {
       Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x+20f, Input.mousePosition.y-20,10));

    }
}
