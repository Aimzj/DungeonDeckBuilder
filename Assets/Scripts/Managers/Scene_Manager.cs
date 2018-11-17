﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene_Manager : MonoBehaviour {

    private GameObject blackScreen;
	void Start () {
        blackScreen = GameObject.Find("BlackScreen");
        StartCoroutine(FadeIn());
	}
	
    public void StartGame()
    {
        StartCoroutine(FadeOut(1));
        
    }

    public void StartTutorial()
    {
        StartCoroutine(FadeOut(2));
    }

    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    public IEnumerator Quit()
    {
        float val = 0;
        for (int i = 0; i < 30; i++)
        {
            val = val + 0.04f;
            blackScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, val);
            yield return new WaitForSeconds(0.05f);
        }
        Application.Quit();
    }

    private IEnumerator FadeIn()
    {
        float val = 1;
        for (int i = 0; i < 40; i++)
        {
            val = val - 0.025f;
            blackScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, val);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator FadeOut(int numScene)
    {
        float val = 0;
        for (int i = 0; i < 30; i++)
        {
            val = val + 0.04f;
            blackScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, val);
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadScene(numScene);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex >0)
            {
                StartCoroutine(FadeOut(0));
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
