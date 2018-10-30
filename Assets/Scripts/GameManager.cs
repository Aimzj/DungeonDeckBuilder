﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private AreaManager areaManagerScript;
    private EnemyAreaManager enemyAreaManagerScript;
    private HandManager handManagerScript;
    private EnemyHandManager enemyHandManagerScript;
    private Scene_Manager sceneManagerScript;

    private bool isActionPhase;
    private bool isReactionPhase;
    private int cycleTokens;
    private int discardBank, trashBank;
    private int attackVal, defenseVal;
	// Use this for initialization
	void Start () {

    }

    public void StartGame()
    {
        print("hi");

        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        sceneManagerScript = GameObject.Find("GameManager").GetComponent<Scene_Manager>();

        enemyAreaManagerScript = GameObject.Find("GameManager").GetComponent<EnemyAreaManager>();
        enemyHandManagerScript = GameObject.Find("GameManager").GetComponent<EnemyHandManager>();

        if (SceneManager.GetActiveScene().name == "BetaScene")
        {
            StartCoroutine(handManagerScript.DrawCards(3));
            StartCoroutine(Delay());
        }

        cycleTokens = 0;
        discardBank = 0;
        trashBank = 0;
        attackVal = 0;
        defenseVal = 0;

        isActionPhase = true;
        isReactionPhase = true;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(enemyHandManagerScript.DrawCards(3));
    }

    public void EndPlayerTurn()
    {
        areaManagerScript.DiscardPlayArea();
        StartCoroutine(handManagerScript.DrawCards(4));        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            areaManagerScript.DiscardPlayArea();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            areaManagerScript.RenewDeck();
        }
    
    }

    public void OpenStore()
    {
        StartCoroutine(sceneManagerScript.FadeOut(2));
    }
}
