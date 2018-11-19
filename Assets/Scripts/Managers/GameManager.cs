using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    private AreaManager areaManagerScript;
    private HandManager handManagerScript;

    private bool isActionPhase;
    private bool isReactionPhase;
    private int cycleTokens;
    private int discardBank, trashBank;
    private int attackVal, defenseVal;

    private Button endTurnButton;
    public Button NextBossButton;

    private StatsManager statManagerScript;
    private CardEffectManager cardEffectScript;
    private CardGenerator cardGenScript;
    private Menu menuScript;

    private Spider spiderScript;
    private Naga nagaScript;
    private Dummy dummyScript;

    private TextMeshPro gamePhaseText;
    private GameObject gamePhaseDisplay;

    private int level = 0;
    private int turnCount;

    private Canvas burnCanvas;
    public TextMeshProUGUI effectText, costText;

    //HELP CANVAS
    public Canvas HelpCanvas;
    public Canvas CheatSheetCanvas;
    public SpriteRenderer CheatsheetSprite;
    private GameObject[] HelpSprites;

    bool isCheat = false;
    bool isHelp = false;

    private Canvas endGameCanvas;

    // Use this for initialization
    void Start () {

        gamePhaseDisplay = GameObject.Find("GamePhase");
        gamePhaseText = GameObject.Find("GamePhaseText").GetComponent<TextMeshPro>();
     
        endTurnButton = GameObject.Find("EndTurn").GetComponent<Button>();

        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        cardEffectScript = GameObject.Find("GameManager").GetComponent<CardEffectManager>();
        menuScript = GameObject.Find("GameManager").GetComponent<Menu>();

        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        spiderScript = GameObject.Find("GameManager").GetComponent<Spider>();

        nagaScript = GameObject.Find("GameManager").GetComponent<Naga>();
        dummyScript = GameObject.Find("GameManager").GetComponent<Dummy>();

        cardGenScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();

        burnCanvas = GameObject.Find("Burn_Canvas").GetComponent<Canvas>();
        burnCanvas.enabled = false;

        //HELP
        HelpSprites = GameObject.FindGameObjectsWithTag("Help");

        endGameCanvas = GameObject.Find("GameOver_Canvas").GetComponent<Canvas>();
    }

    //always restart player from the beginning
    public void RestartButton()
    {
        PlayerPrefs.SetInt("Level",1);
        StartCoroutine(menuScript.LoadLevel(2));
    }

    public void NextLevel()
    {
        int level = PlayerPrefs.GetInt("Level");
        level++;
        PlayerPrefs.SetInt("Level", level);
        if (level >2)
        {
            StartCoroutine(menuScript.LoadLevel(3));
        }
        else
        {
            StartCoroutine(menuScript.LoadLevel(2));
        }

    }

    public void StopEverything()
    {
        StopAllCoroutines();
    }

    public void EndLevel(int win)
    {
        endGameCanvas.enabled = true;

        //stop all the coroutines from running
        StopEverything();
        spiderScript.StopEverything();
        areaManagerScript.StopEverything();
        nagaScript.StopEverything();

        //freeze the player cards
        for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
        {
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isFrozen = true;
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isPlayed = true;
        }

        endTurnButton.enabled = false;

        if (win == 0)
        {
            NextBossButton.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = "End Game";
            NextBossButton.enabled = true;
        }
        else
        {
            NextBossButton.enabled = false;
            
        }

    }

    public void StartGame(int lvl)
    {
        level = lvl;
        print("Level is :" + level);
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();

        if (level>0)
        {
            StartCoroutine(DisplayPhase("Player Goes First"));

           statManagerScript.SetPhase("player", "action");
           statManagerScript.SetPhase("enemy", "waiting");

            StartCoroutine(handManagerScript.DrawCards(4, "player"));
            StartCoroutine(Delay(3, "enemy"));
        }
        else
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
        StartCoroutine(EndPlayerReact_wait());
    }

    public IEnumerator EndPlayerReact_wait()
    {
        //resolve attack and defense values and other effects
        int DamageDealt_toPlayer = statManagerScript.numAttack - statManagerScript.numDefense;
        if (DamageDealt_toPlayer > 0)
        {
            // statManagerScript.UpdateHealth("player", -DamageDealt_toPlayer);
           
               
            StartCoroutine(handManagerScript.Call_TakeDamage(DamageDealt_toPlayer, "player"));
        
        }
        if (level == 2)
        {
            nagaScript.CrushingBlow();

        }
        //wait until all damage is finished being dealt before drawing new cards
        yield return new WaitForSecondsRealtime(1.2f * DamageDealt_toPlayer);
        
       
        //clear attack and defense values
        statManagerScript.ClearAttack();
        statManagerScript.ClearDefense();
        //play areas cleared after enemy reacts
        areaManagerScript.Call_DiscardPlayArea("player");
        areaManagerScript.Call_DiscardPlayArea("enemy");
        if (level == 2)
        {
            if (nagaScript.numRemoveCards > 0)
            {

                for (int i = 0; i < nagaScript.numRemoveCards - 1; i++)
                {
                    if (areaManagerScript.player_DiscardCardList.Count > 0)
                    {
                        areaManagerScript.player_DiscardCardList.RemoveAt(areaManagerScript.player_DiscardCardList.Count - i);
                    }

                }
            }

        }
       
        //enemy draws 1 card
        StartCoroutine(handManagerScript.DrawCards(1, "enemy"));
        //end of enemy's turn

        StartCoroutine(DisplayPhase("Player's Turn"));
        //player draws 3
        StartCoroutine(Delay(3, "player"));


       
        if(level==0)
        {
            print("DIALOGUE TINGS");
            StartCoroutine(dummyScript.PlayerDialogue());
        }
            
        //PLAYER ACTS
        statManagerScript.SetPhase("player", "action");
        statManagerScript.SetPhase("enemy", "waiting");

        //enable the button until the player's turn is done
        endTurnButton.enabled = true;
    }

    //called by enemy script
    public IEnumerator EndEnemyReact()
    {

        //after the enemy reacts to player's cards, it is the enemy's turn

        //resolve attack and defense values and other effects
        int DamageDealt_toEnemy = statManagerScript.numAttack - statManagerScript.numDefense;
        if (DamageDealt_toEnemy > 0)
        {
           // statManagerScript.UpdateHealth("enemy", -DamageDealt_toEnemy);
            StartCoroutine(handManagerScript.Call_TakeDamage(DamageDealt_toEnemy,"enemy"));
        }
        //wait until all damage is finished being dealt before drawing new cards
        yield return new WaitForSecondsRealtime(1.2f * DamageDealt_toEnemy);

        //clear attack and defense values
        statManagerScript.ClearAttack();
        statManagerScript.ClearDefense();
        //play areas cleared after enemy reacts
        areaManagerScript.Call_DiscardPlayArea("player");
        areaManagerScript.Call_DiscardPlayArea("enemy");

        //poison exists
        if (level == 1)
        {
            int waitFor = cardEffectScript.PlayUndyingPoisonEffects();
            print("WAIT FOR: " + waitFor.ToString());
            yield return new WaitForSecondsRealtime(waitFor);
        }

        //player draws 1 card
        StartCoroutine(handManagerScript.DrawCards(1, "player"));
        //Check for pact of maggots
       for(int i = 0; i < areaManagerScript.player_DiscardCardList.Count; i ++)
        {
            print("PACT");
            if (areaManagerScript.player_DiscardCardList[i].GetComponent<CardObj>().CardName == "Pact of maggots")
            {
                print("FOUND PACT");
                cardEffectScript.PactOfMaggot();
            }
        }
        //END OF PLAYER TURN

        StartCoroutine(DisplayPhase("Enemy's Turn"));
        //enemy draws 3
        statManagerScript.SetPhase("player", "waiting");
        statManagerScript.SetPhase("enemy", "action");
        StartCoroutine(Delay(3, "enemy"));

    
        StartCoroutine(EnemyWaitToAct());
        //StartCoroutine(dummyScript.PlayerDialogue());



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
       
        if(level == 0)
        {
            StartCoroutine(dummyScript.Action());
        }
       else  if (level == 1)
        {
            print("do the thing DUMB SPIDER!");
            StartCoroutine(spiderScript.Action());
        }
        else if (level == 2)
        {
            StartCoroutine(nagaScript.Action());
        }
        
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
            if (level == 0)
                StartCoroutine(dummyScript.Reaction());
            else if (level == 1)
                StartCoroutine(spiderScript.Reaction());
            else if (level == 2)
                StartCoroutine(nagaScript.Reaction());
            
        }
        
    
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.W))
        {
            //win
            EndLevel(0);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //lose
            EndLevel(1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            areaManagerScript.Call_DiscardPlayArea("player");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            areaManagerScript.Call_RenewDeck("player");
        }
        //For cheat sheet
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(isCheat == false)
            {
                isCheat = true;
            CheatSheetCanvas.enabled = true;
                CheatsheetSprite.enabled = true;
            }
            else
            {
                isCheat = false;
                CheatSheetCanvas.enabled = false;
                CheatsheetSprite.enabled = false;
            }
       
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(!isHelp)
            {
                isHelp = true;
                HelpCanvas.enabled = true;
                foreach (GameObject helpBoard in HelpSprites)
                {
                    helpBoard.GetComponent<SpriteRenderer>().enabled = true;
                }

            }
            else
            {
                isHelp = false;
                HelpCanvas.enabled = false;
                foreach (GameObject helpBoard in HelpSprites)
                {
                    helpBoard.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    
    }


    public void DisplayBurnUI(GameObject cardObj)
    {
        burnCanvas.enabled = true;
        effectText.text = "(" + cardObj.GetComponent<CardObj>().BurnEffect + ")";
        costText.text = "Cost: " + cardObj.GetComponent<CardObj>().BurnCost.ToString();

        //freeze all interactions with cards
        for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
        {
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isFrozen = true;
        }
        //disable the button
        endTurnButton.enabled = false;
    }

    public void NoBurn()
    {
        //unfreeze all interactions with cards
        for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
        {
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isFrozen = false;
        }
        //enable the button
        endTurnButton.enabled = true;

        burnCanvas.enabled = false;
    }

    public void YesBurn()
    {
        //unfreeze all interactions with cards
        for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
        {
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isFrozen = false;
        }
        //enable the button
        endTurnButton.enabled = true;

        burnCanvas.enabled = false;

        //give the card a burn halo whilst in play area
        areaManagerScript.player_PlayCardList[areaManagerScript.player_PlayCardList.Count-1].transform.Find("BurnBorder").GetComponent<SpriteRenderer>().enabled = true;

        //play the card's burn effects
        cardEffectScript.PlayBurn(areaManagerScript.player_PlayCardList[areaManagerScript.player_PlayCardList.Count - 1]);

    }
}
