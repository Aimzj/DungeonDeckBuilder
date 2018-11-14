using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {
    private double angle = 0;
    private double smooth = 5.0;
    // Use this for initialization
    void Start () {
       Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x+20f, Input.mousePosition.y-20,10));

       // transform.position = new Vector3((Input.mousePosition.x - Screen.width / 2) / Screen.width / 0.05f, 2, (Input.mousePosition.y - Screen.width / 8) / Screen.width / 0.05f);

       /* // Smoothly tilts a transform towards a target rotation.
        double tiltAroundZ = Input.GetAxis("Mouse X") * angle * 2;
        double tiltAroundX = Input.GetAxis("Mouse Y") * angle * 2;
        var target = Quaternion.Euler((float)tiltAroundX + 90, 0, -(float)tiltAroundZ);
        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * (float)smooth);*/

    }
}
