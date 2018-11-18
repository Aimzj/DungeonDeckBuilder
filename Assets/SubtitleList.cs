using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubtitleList : MonoBehaviour {
    public GameObject[] subtitleList;
    public SubtitleFunctions[] subFuncs;
    int subtitleCounter;
    [HideInInspector]
    public float subtitleTimer, subtitleTimerLim, subtitlePerc;
    public float initialDelay;
    bool fadeIn;

    public bool goingToNextScene = false;
    public float sceneTimeLimit = 120f;

	// Use this for initialization
	void Start () {

        subtitleList = new GameObject[transform.childCount];
        subFuncs = new SubtitleFunctions[subtitleList.Length];

        for (int i = 0; i < subtitleList.Length; i++)
        {
            subtitleList[i] = transform.GetChild(i).gameObject;
            subFuncs[i] = subtitleList[i].GetComponent<SubtitleFunctions>();
            
        }

        subtitleCounter = -1;
        subtitleTimerLim = initialDelay;//initial wait timer before subtitles begin 
        //subtitleTimerLim = subtitleList[subtitleCounter].GetComponent<SubtitleFunctions>().textLength;

        if (goingToNextScene)
        {
            Invoke("NextScene",sceneTimeLimit);
        }
    }
	
	// Update is called once per frame
	void Update () {
        

        if (subtitleCounter< subtitleList.Length)
        {
            Timer(ref subtitleTimer, subtitleTimerLim, ref subtitlePerc);

            CheckTimer();
        }


        //SceneManagement Stuff
        if (Input.GetKeyDown(KeyCode.Escape) && goingToNextScene)
        {
            if (goingToNextScene)
            {
                CancelInvoke();
                NextScene();
            }
            else
            {
                SceneManager.LoadScene(1);
            }

            
        }
        
	}


    public void CheckTimer ()
    {
        if (subtitleTimer == subtitleTimerLim)
        {
            subtitleCounter++;

            /*for (int i = 0; i < subtitleList.Length; i++)
            {
                subFuncs[i].VisibleSwitchOff();
            }
            */

            if (subtitleCounter < subtitleList.Length+1)
            {
                subtitleTimer = 0f;
                subtitleTimerLim = subtitleList[subtitleCounter].GetComponent<SubtitleFunctions>().textLength;
                subFuncs[subtitleCounter].VisibleSwitchOn();
                
                
            }/*
            else
            {
                for (int i = 0; i < subtitleList.Length; i++)
                {
                    subFuncs[i].VisibleSwitchOff();
                }
            }*/
            

            

        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void Timer(ref float timer, float timerlim, ref float perc)
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
