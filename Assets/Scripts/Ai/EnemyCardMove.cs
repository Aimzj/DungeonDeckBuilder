using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardMove : MonoBehaviour {


    public float speed = 4f;
    public Transform targetPos;
    bool isMoving = false;

    //Use states to determine which pile to be deposited into : 0 - Deck, 1 - hand, 2 - discard, 3 - burn
    public int state = 0;

	public void SetTarget(Transform target)
    {
        targetPos = target;
        isMoving = true;
    }

	// Update is called once per frame
	void Update () {
        if(isMoving)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, step);
            if(transform.position == targetPos.position)
            {
                isMoving = false;
            }
        }
        
	}
}
