using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleList : MonoBehaviour {
    public GameObject[] subtitleList;
    int subtitleCounter;
    float subtitleTimer, subtitleTimerLim, subtitlePerc;
    bool fadeIn;

	// Use this for initialization
	void Start () {

        subtitleList = new GameObject[transform.childCount];

        for (int i = 0; i < subtitleList.Length; i++)
        {
            subtitleList[i] = transform.GetChild(i).gameObject;
            subtitleList[i].SetActive(false);
        }

        subtitleCounter = -1;
        subtitleTimerLim = 5f;//initial wait timer before subtitles begin 
        //subtitleTimerLim = subtitleList[subtitleCounter].GetComponent<SubtitleFunctions>().textLength;

    }
	
	// Update is called once per frame
	void Update () {

        if (subtitleCounter< subtitleList.Length)
        {
            Timer(ref subtitleTimer, subtitleTimerLim, ref subtitlePerc);

            CheckTimer();
        }
	}


    public void CheckTimer ()
    {
        if (subtitleTimer == subtitleTimerLim)
        {
            subtitleCounter++;

            for (int i = 0; i < subtitleList.Length; i++)
            {
                subtitleList[i].SetActive(false);
            }

            if (subtitleCounter < subtitleList.Length+1)
            {
                subtitleList[subtitleCounter].SetActive(true);
                subtitleTimer = 0f;
                subtitleTimerLim = subtitleList[subtitleCounter].GetComponent<SubtitleFunctions>().textLength;
            }
            else
            {
                for (int i = 0; i < subtitleList.Length; i++)
                {
                    subtitleList[i].SetActive(false);
                }
            }
            

            

        }
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
