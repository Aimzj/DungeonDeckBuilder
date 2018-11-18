using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotation : MonoBehaviour {

    public Vector3 targetAngle;

    private Vector3 currentAngle;

    public float speed;

    public void Start()
    {
        targetAngle = new Vector3(90f, 0f, 180f);
        currentAngle = transform.eulerAngles;
    }

    public void Update()
    {
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime*speed),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime*speed),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime*speed));

        transform.eulerAngles = currentAngle;
    }
}
