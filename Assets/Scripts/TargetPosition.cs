using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition : MonoBehaviour
{
    public Vector3 targetPos; // = new Vector3(0f, 345f, 0f);

    private Vector3 currentPos;

    public float speed;

    public void Start()
    {
        currentPos = transform.position;
    }

    public void Update()
    {
        currentPos = new Vector3(
            Mathf.Lerp(currentPos.x, targetPos.x, Time.deltaTime * speed),
            Mathf.Lerp(currentPos.y, targetPos.y, Time.deltaTime * speed),
            Mathf.Lerp(currentPos.z, targetPos.z, Time.deltaTime * speed));

        transform.position = currentPos;
    }
}
