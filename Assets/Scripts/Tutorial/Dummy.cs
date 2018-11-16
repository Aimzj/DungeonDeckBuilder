﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dummy : MonoBehaviour {

    private List<GameObject> dummyHand = new List<GameObject>();

    public int TurnCount = 1;

    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private StatsManager statsManagerScript;
    private GameManager gameManagerScript;
    string output = "";

    public TextMeshProUGUI dialogueText;

    private void Start()
    {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

       
    }

    IEnumerator startMessage()
    {
        //output = "Welcome to burn. The card game where you will seek the eternal flame ";
        dialogueText.text = "Welcome to burn. The card game where you will seek the eternal flame";
        yield return new WaitForSecondsRealtime(2);
        dialogueText.text = "Play attacks during your action phase to do damage";
        yield return new WaitForSecondsRealtime(2);
        dialogueText.text = "Each point of damage will destroy one card from the opponent's deck";
        yield return new WaitForSecondsRealtime(2);
    }


    public IEnumerator Action()
    {
        TurnCount++;
        print("DO A THING FFS!!!");
        updateEnemyHand();

        switch (TurnCount)
        {
            case 0:
                //print(1);
                playCard("Strike",1);
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "Try to defend yourself with a guard";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "Cards with shield give you defence points for the reaction phase";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "These guards have a discard cost";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "Put one of the guards into the discard pile so you can defend";
                break;

            case 1:
                
                playCard("Strike",3);
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "Looks like you dont have enough guards to defend";
                //Make player lose red eye gem
                //playCard("Strike");
                break;

            

        }
        dialogueText.text = " ";
        print("DONE WITH ENEMY Action");
        gameManagerScript.EndEnemyTurn();
    }

    void playCard(string name, int numcards)
    {
        
        for (int i = 0; i < dummyHand.Count; i++)
        {
            if (dummyHand[i].GetComponent<CardObj>().CardName == name && numcards > 0)
            {
                numcards--;
                dummyHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                statsManagerScript.UpdateAttack("enemy", 1);
               
                //TRIGGER PROMPT FOR PLAYER
            }
        }
    }

    public IEnumerator PlayerDialogue()
    {
        if (TurnCount == 0)
        {
          
            output = "Burn them all to win!";
            yield return new WaitForSecondsRealtime(2);
        }
        else if (TurnCount == 1)
        {
            dialogueText.text = "Lucky Charm has a special effect";
            yield return new WaitForSecondsRealtime(2);
            dialogueText.text = "Play it to see how it works";
            yield return new WaitForSecondsRealtime(2);
            dialogueText.text = "Play out the new cards you got from the effect";
            yield return new WaitForSecondsRealtime(2);
            dialogueText.text = "Some effects only active in the reactive or active phase";
            yield return new WaitForSecondsRealtime(2);
            dialogueText.text = "You'll get to try these out later";
            yield return new WaitForSecondsRealtime(2);
        }
        else if (TurnCount == 2)
        {
            dialogueText.text = "The enemy is low on cards and vulnerable now!";
            yield return new WaitForSecondsRealtime(2);
            dialogueText.text = "Play the focused stike card for massive damage";
            yield return new WaitForSecondsRealtime(2);
            dialogueText.text = "Active it's powerful burn effect by burning one card before playing this";
            yield return new WaitForSecondsRealtime(2);
            dialogueText.text = "Dont forget the discard cost too!";
            yield return new WaitForSecondsRealtime(2);
        }
        else if (TurnCount == 3)
        {
            dialogueText.text = "";
        }
        dialogueText.text = " ";
        yield return new WaitForSecondsRealtime(2);
    }


    public IEnumerator Reaction()
    {
       // TurnCount++;
        print("DUMMY REACTION");
        updateEnemyHand();

        switch (TurnCount)
        {
            case 0:
               
                //TRIGGER PROMPT FOR PLAYER
                break;

            case 1:

                //Trigger Prompt
                playCard("Lesser Guard",2);
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "Some cards trigger effects when drawn";
                print(dialogueText.text);
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "These are marked with the 'on arrival' keyword";

                break;
            case 3:
                dialogueText.text = "Congratualtions you've finished training";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "An encouter ends once either the player or the monster reach their burn limits or lose all sigils";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "Don't worry your sigils come back after each battle";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "After each victory you will awarded with the your choice of card pack to add to your deck";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "Remember your burn limit persists through encounters";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "You are now ready to delve deeper and face greater challenges";
                yield return new WaitForSecondsRealtime(2);
                dialogueText.text = "Good luck an get ready to burn!";
                yield return new WaitForSecondsRealtime(2);
                //dialogueText.text = "";
                //TRIGGER PROMPT FOR PLAYER
                break;



        }
        dialogueText.text = " ";
       yield return new WaitForSecondsRealtime(2);
        print("END ENEMY REACTION");
        gameManagerScript.EndPlayerReact();
    }

    private void updateEnemyHand()
    {
        dummyHand.Clear();
        dummyHand = new List<GameObject>(handManagerScript.enemyHandlist);
    }
}
