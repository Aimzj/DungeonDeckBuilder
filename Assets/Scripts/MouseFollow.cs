using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour {
    public GameObject CardObj;
    private bool isFollowing;
    private double angle = 15.0;
    private double smooth = 5.0;
    // Use this for initialization
    void Start () {
        isFollowing = false;
	}
	
	// Update is called once per frame
	void Update () {
            if (isFollowing)
        {
            transform.position = new Vector3((Input.mousePosition.x - Screen.width / 2) / Screen.width / 0.05f, 0, (Input.mousePosition.y - Screen.width /8) / Screen.width / 0.05f);

            // Smoothly tilts a transform towards a target rotation.
            double tiltAroundZ = Input.GetAxis("Mouse X") * angle * 2;
            double tiltAroundX = Input.GetAxis("Mouse Y") * angle * 2;
            var target = Quaternion.Euler((float)tiltAroundX, 0, -(float)tiltAroundZ);
            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * (float)smooth);
        }
	}

    private void OnMouseDown()
    {
        isFollowing = true;
    }
}
