﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    private Scene_Manager sceneManagerScript;
    private CardGenerator cardGenScript;

    private GameObject blackScreen;

    private LevelTracker levelTrackerScript;

    public int level;
	// Use this for initialization
	void Start () {

        blackScreen = GameObject.Find("BlackScreen");

        StartCoroutine(FadeIn());

        level = 0;
	}

 
    IEnumerator FadeIn()
    {
        float val = 1;
        for (int i = 0; i < 40; i++)
        {
            val = val - 0.025f;
            blackScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, val);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator FadeOutFadeIn()
    {
        //if the scene is still fading in
        StopCoroutine(FadeIn());

        //fade out
        float val = 0;
        for (int i = 0; i < 30; i++)
        {
            val = val + 0.04f;
            blackScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, val);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.2f);
        //fade in
        val = 1;
        for (int i = 0; i < 40; i++)
        {
            val = val - 0.025f;
            blackScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, val);
            yield return new WaitForSeconds(0.05f);
        }
    }


    public IEnumerator LoadLevel(int numScene)
    {
        //fade out
        float val = 0;
        for (int i = 0; i < 30; i++)
        {
            val = val + 0.04f;
            blackScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, val);
            yield return new WaitForSeconds(0.05f);
        }

        SceneManager.LoadScene(numScene);
        
    }

    public void StartTutorial()
    {
        PlayerPrefs.SetInt("Level", 0);
        StartCoroutine(LoadLevel(3));
    }

    public void StartLevel1()
    {
        PlayerPrefs.SetInt("Level", 1);
        StartCoroutine(LoadLevel(2));
    }

    private IEnumerator GoBack(int scene)
    {
        StartCoroutine(FadeOutFadeIn());
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        PlayerPrefs.SetInt("Level", 0);
        StartCoroutine(sceneManagerScript.FadeOut_Quit());
    }

    public void WatchOpening()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void ViewCredits()
    {
        StartCoroutine(LoadLevel(4));
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name !="Menu")
            {
                //fade out and reload the scene
                PlayerPrefs.SetInt("Level", 0);
                StartCoroutine(LoadLevel(1));
            }
            else
            {
                ExitGame();
            }
        }
	}
}