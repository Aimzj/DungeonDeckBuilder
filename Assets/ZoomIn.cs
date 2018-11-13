using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomIn : MonoBehaviour {

    GameObject target;
    public Vector3 startDis1, endDis1, startDis2, endDis2;
    public Vector3 startRot1, endRot1;
    //[HideInInspector]
    public float moveTimer, moveTimerLimit, movePerc;
    public int camState; 
	// Use this for initialization
	void Start () {
        ChangeCamState0();
		
	}
	
	// Update is called once per frame
	void Update () {



        switch (camState)
        {
            case 0:
                Timer(ref moveTimer, moveTimerLimit, ref movePerc);

                movePerc = movePerc * movePerc * movePerc * (movePerc * (6f * movePerc - 15f) + 10f);
                transform.position = Vector3.Lerp(startDis1, endDis1, movePerc);

                if (moveTimer == moveTimerLimit)
                {
                    Invoke("ChangeCamState1", 0.5f);
                }
                break;

            case 1:
                Timer(ref moveTimer, moveTimerLimit, ref movePerc);

                movePerc = Mathf.Sin(movePerc * Mathf.PI * 0.5f);

                transform.position = Vector3.Lerp(startDis2, endDis2, movePerc);
                if (moveTimer == moveTimerLimit)
                {
                    Invoke("ChangeCamState2",0.5f);
                }
                break;

            case 2:

                Timer(ref moveTimer, moveTimerLimit, ref movePerc);

                movePerc = movePerc * movePerc * movePerc * (movePerc * (6f * movePerc - 15f) + 10f);

                transform.eulerAngles = new Vector3 (Mathf.LerpAngle (startRot1.x, endRot1.x, movePerc),0f, 0f);

                if (moveTimer == moveTimerLimit)
                {
                    Invoke("ChangeCamState3", 0.5f);
                }

                break;
                

        }
               

		
	}

    public void ChangeCamState0 ()
    {
        CancelInvoke();
        camState = 0;
        moveTimer = 0f;
        moveTimerLimit = 45f;
        
        
        //if (camState )
    }

    public void ChangeCamState1()
    {
        CancelInvoke();
        camState = 1;
        moveTimer = 0f;
        moveTimerLimit = 10f;
        
        
        //if (camState )
    }

    public void ChangeCamState2()
    {
        CancelInvoke();
        camState = 2;
        moveTimer = 0f;
        moveTimerLimit = 10f;
        
        
        //if (camState )
    }

    public void ChangeCamState3()
    {
        CancelInvoke();
        camState = 3;
        moveTimer = 0f;
        moveTimerLimit = 2f;


        //if (camState )
    }


    public void Timer  (ref float timer, float timerlim, ref float perc)
    {
        if (timer < timerlim)
        {
            timer += Time.deltaTime;
        }
        else if (timer >= timerlim)
        {
            timer = timerlim;
        }

        perc = timer / timerlim;
    }
}
