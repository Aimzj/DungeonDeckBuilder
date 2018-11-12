using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private Scene_Manager sceneManagerScript;

    private bool isActionPhase;
    private bool isReactionPhase;
    private int cycleTokens;
    private int discardBank, trashBank;
    private int attackVal, defenseVal;

    private Button endTurnButton;

    private StatsManager statManagerScript;

    private Spider spiderScript;
    private Naga nagaScript;

    private TextMeshPro gamePhaseText;
    private GameObject gamePhaseDisplay;

    // Use this for initialization
    void Start () {

        gamePhaseDisplay = GameObject.Find("GamePhase");
        gamePhaseText = GameObject.Find("GamePhaseText").GetComponent<TextMeshPro>();

        endTurnButton = GameObject.Find("EndTurn").GetComponent<Button>();

        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();

         //spiderScript = GameObject.Find("GameManager").GetComponent<Spider>();
        nagaScript = GameObject.Find("GameManager").GetComponent<Naga>(); ;
    }

    public void StartGame()
    {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        sceneManagerScript = GameObject.Find("GameManager").GetComponent<Scene_Manager>();

        if (SceneManager.GetActiveScene().name == "BetaScene")
        {
            StartCoroutine(DisplayPhase("Player Goes First"));

            statManagerScript.SetPhase("player", "action");
            statManagerScript.SetPhase("enemy", "waiting");

            StartCoroutine(handManagerScript.DrawCards(2, "player"));
            StartCoroutine(Delay(3, "enemy"));
        }

        cycleTokens = 0;
        discardBank = 0;
        trashBank = 0;
        attackVal = 0;
        defenseVal = 0;

        isActionPhase = true;
        isReactionPhase = true;
    }

    IEnumerator Delay(int numCards, string target)
    {
        yield return new WaitForSecondsRealtime(1);
        if(target=="player")
            StartCoroutine(handManagerScript.DrawCards(numCards,"player"));
        else if(target=="enemy")
            StartCoroutine(handManagerScript.DrawCards(numCards, "enemy"));
    }

    //called by the Enemy when they are finished playing cards
    public void EndEnemyTurn()
    {
        //enable the card interactions until it is the enemy's turn
        for (int i = 0; i < handManagerScript.playerDeckList.Count; i++)
        {
            handManagerScript.playerDeckList[i].GetComponent<CardMovement>().isFrozen = false;
        }

        StartCoroutine(DisplayPhase("Player Reaction"));
        //PLAYER REACT
        statManagerScript.SetPhase("player", "reaction");
        statManagerScript.SetPhase("enemy", "waiting");

        endTurnButton.enabled = true;
    }

    IEnumerator DisplayPhase(string phase)
    {
        gamePhaseDisplay.SetActive(true);
        gamePhaseText.text = phase;
        yield return new WaitForSecondsRealtime(2);
        gamePhaseDisplay.SetActive(false);
    }

    public void EndPlayerReact()
    {
        //resolve attack and defense values and other effects
        int DamageDealt_toPlayer = statManagerScript.numAttack_enemy - statManagerScript.numDefense_player;
        if (DamageDealt_toPlayer > 0)
        {
            statManagerScript.UpdateHealth("player", -DamageDealt_toPlayer);
            //StartCoroutine(handManagerScript.DamagePlayer(DamageDealt_toPlayer));
        }
        //clear attack and defense values
        statManagerScript.ClearAttack();
        statManagerScript.ClearDefense();
        //play areas cleared after enemy reacts
        areaManagerScript.Call_DiscardPlayArea("player");
        areaManagerScript.Call_DiscardPlayArea("enemy");

        //enemy draws 1 card
        StartCoroutine(handManagerScript.DrawCards(1, "enemy"));
        //end of enemy's turn

        StartCoroutine(DisplayPhase("Player's Turn"));
        //player draws 3
        StartCoroutine(Delay(3, "player"));

        //PLAYER ACTS
        statManagerScript.SetPhase("player", "action");
        statManagerScript.SetPhase("enemy", "waiting");

        //enable the button until the player's turn is done
        endTurnButton.enabled = true;
    }

    //called by enemy script
    public void EndEnemyReact()
    {
        //after the enemy reacts to player's cards, it is the enemy's turn

        //resolve attack and defense values and other effects
        int DamageDealt_toEnemy = statManagerScript.numAttack_player - statManagerScript.numDefense_enemy;
        if (DamageDealt_toEnemy > 0)
        {
            statManagerScript.UpdateHealth("enemy", -DamageDealt_toEnemy);
           // StartCoroutine(enemyHandManagerScript.DamageEnemy(DamageDealt_toEnemy));
        }

        //clear attack and defense values
        statManagerScript.ClearAttack();
        statManagerScript.ClearDefense();
        //play areas cleared after enemy reacts
        areaManagerScript.Call_DiscardPlayArea("player");
        areaManagerScript.Call_DiscardPlayArea("enemy");

        //player draws 1 card
        StartCoroutine(handManagerScript.DrawCards(1, "player"));
        //END OF PLAYER TURN

        StartCoroutine(DisplayPhase("Enemy's Turn"));
        //enemy draws 3
        statManagerScript.SetPhase("player", "waiting");
        statManagerScript.SetPhase("enemy", "action");
        StartCoroutine(Delay(3, "enemy"));


        StartCoroutine(EnemyWaitToAct());



        //disable the card interactions until it is the player's turn
        for (int i = 0; i < handManagerScript.playerDeckList.Count; i++)
        {
            handManagerScript.playerDeckList[i].GetComponent<CardMovement>().isFrozen = true;
        }
        //disable the button
        endTurnButton.enabled = false;
    }

    IEnumerator EnemyWaitToAct()
    {
        yield return new WaitForSeconds(3f);
        //ENEMY ACTS      
        StartCoroutine(nagaScript.Action());
        //StartCoroutine(spiderScript.Action());
    }

    //called when the player CLICKS the button 
    public void EndPlayerTurn()
    {
        if(statManagerScript.phase_player == "reaction")
        {
            EndPlayerReact();
        }
        else
        {
            StartCoroutine(DisplayPhase("Enemy Reaction"));
            //ENEMY REACTS
            statManagerScript.SetPhase("player", "waiting");
            statManagerScript.SetPhase("enemy", "reaction");
            StartCoroutine(nagaScript.Reaction());
            //StartCoroutine(spiderScript.Reaction());
        }
        
    
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            areaManagerScript.Call_DiscardPlayArea("player");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            areaManagerScript.Call_RenewDeck("player");
        }
    
    }

    public void OpenStore()
    {
        StartCoroutine(sceneManagerScript.FadeOut(2));
    }
}
