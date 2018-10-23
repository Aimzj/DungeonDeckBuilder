using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private Scene_Manager sceneManagerScript;

    private int cycleTokens;
    private int discardBank, trashBank;
    private int attackVal, defenseVal;
	// Use this for initialization
	void Start () {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        sceneManagerScript = GameObject.Find("GameManager").GetComponent<Scene_Manager>();

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(handManagerScript.DrawCards(3));
        }

        cycleTokens = 0;
        discardBank = 0;
        trashBank = 0;
        attackVal = 0;
        defenseVal = 0;
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
