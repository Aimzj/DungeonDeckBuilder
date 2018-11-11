using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {

    private List<GameObject> spiderHand = new List<GameObject>();

    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private StatsManager statsManagerScript;
    private GameManager gameManagerScript;

    private int upperBound;
    private void Start()
    {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    private void Update()
    {
 
    }
    
    //For reaction phase
    public IEnumerator Reaction()
    {
        UpdateEnemyHand();

        for (int i = 0; i < spiderHand.Count; i++)
        {
            if (spiderHand[i].GetComponent<CardObj>().CardName == "Skitter")
            {
                //play skitter
                spiderHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                //enemyAreaManagerScript.PlayCard(spiderHand[i]);
                //enemyHandManagerScript.RemoveCardFromHand(i);
                //Skitter();
                statsManagerScript.UpdateDefense("enemy", 1);

                yield return new WaitForSecondsRealtime(1);
            }
        }
        UpdateEnemyHand();
        //loop through to check for a Skitter card
        bool isFound = false;
        for (int i = 0; i < spiderHand.Count; i++)
        {
            if (spiderHand[i].GetComponent<CardObj>().CardName == "Skitter")
            {
                isFound = true;

            }
        }

        //call reaction function again if Skitter was found
        if (isFound)
        {
            Reaction();
        }
        else
        {
            //call end of enemy reaction function
            print("DONE WITH ENEMY REACTION");
            gameManagerScript.EndEnemyReact();
        }
    }

    private void Skitter()
    {
        statsManagerScript.UpdateAttack("enemy", 1);
        statsManagerScript.UpdateDefense("enemy", 1);

      //  StartCoroutine(enemyHandManagerScript.DrawCards(1));
    }

    private void Bite()
    {
        statsManagerScript.UpdateAttack("enemy", 1);
        statsManagerScript.UpdateDefense("enemy", 1);

        //ADD POISON TO PLAYER DECK

    }

    //played as it arrives in enemy's hand
    public void Lethargy()
    {
       // upperBound--;
        if(statsManagerScript.phase_enemy=="reaction")
            statsManagerScript.UpdateDefense("enemy", 1);

    }


    //For action phase
    public IEnumerator Action()
    {

        UpdateEnemyHand();
        print("Action #cards in hand: " + spiderHand.Count);
        //looping through hand of enemy hand
        int upperBound = spiderHand.Count;
        for (int i = 0; i<upperBound;i++)
        {
            if (spiderHand[i].GetComponent<CardObj>().CardName == "Skitter")
            {
                //check number of cards before Skitter
                int numInHand_before = handManagerScript.numCardsInEnemyHand;

                //play skitter card
                spiderHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                //upperBound++;
                //enemyAreaManagerScript.PlayCard(spiderHand[i]);
                // yield return new WaitForSecondsRealtime(1);
                //enemyHandManagerScript.RemoveCardFromHand(i);
                //Skitter();
                statsManagerScript.UpdateAttack("enemy",1);

                yield return new WaitForSecondsRealtime(2);
                //check number of cards in hand after skitter
               /* int numInHand_after = enemyHandManagerScript.numCardsInHand;
                print("before: " + numInHand_before + "    after: " + numInHand_after);
                if (numInHand_after - numInHand_before == 0)
                {
                    //skitter added card
                    print("SKITTER ADDED CARD");
                    upperBound++;
                }*/
                //upperBound += numInHand_after - numInHand_before;
            }
            else if (spiderHand[i].GetComponent<CardObj>().CardName == "Bite")
            {
                //play bite card
                spiderHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                //enemyAreaManagerScript.PlayCard(spiderHand[i]);
                // yield return new WaitForSecondsRealtime(1);
                //enemyHandManagerScript.RemoveCardFromHand(i);
                //Bite();
                statsManagerScript.UpdateAttack("enemy", 1);

                yield return new WaitForSecondsRealtime(2);
            }
          /*  i = 0;
            UpdateEnemyHand();
            upperBound = spiderHand.Count;
            print("upper bound: " + spiderHand.Count);*/
        }
        print("DONE WITH ENEMY ACTION");
        gameManagerScript.EndEnemyTurn();
        /*UpdateEnemyHand();
        //check if more cards are in the enemy's hand
        //if so, call action function again
        if (spiderHand.Count > 0)
        {
            Action();
        }
        else
        {
            print("DONE WITH ENEMY ACTION");
            gameManagerScript.EndEnemyTurn();
        }*/

    }

    private void UpdateEnemyHand()
    {
        spiderHand.Clear();
        //create a list of the current cards existing in the enemy's hand
        for (int i = 0; i < handManagerScript.enemyDeckList.Count; i++)
        {
            if (handManagerScript.enemyDeckList[i].GetComponent<CardMovement>().isInHand)
                spiderHand.Add(handManagerScript.enemyDeckList[i]);
        }
    }

	
}
