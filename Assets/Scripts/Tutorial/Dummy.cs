using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dummy : MonoBehaviour {

    [SerializeField]
    private List<GameObject> dummyHand = new List<GameObject>();

    public int TurnCount = 1;

    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private StatsManager statsManagerScript;
    private GameManager gameManagerScript;
    string output = "";
    public float timer = 0.25f;

    public TextMeshProUGUI dialogueText;

    private void Start()
    {
        StartCoroutine(startMessage());
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        statsManagerScript.SetHealth("player", 100);
        statsManagerScript.SetHealth("enemy", 100);
        statsManagerScript.UpdateSigils("player", 30);
        statsManagerScript.UpdateSigils("enemy", 30);
        statsManagerScript.SetTotalCards("enemy", 100,0);
        statsManagerScript.SetTotalCards("player", 100, 0);

        StartCoroutine(handManagerScript.DrawCards(2, "player"));
        //statsManagerScript.numHealth_player = 100;
        //statsManagerScript.numCardsInDeck_player = 10;
        //statsManagerScript.numCardsInDeck_enemy = 10;

    }

    IEnumerator startMessage()
    {
        //output = "Welcome to burn. The card game where you will seek the eternal flame ";
        dialogueText.text = "Welcome to burn. The card game where you will seek the eternal flame";
        yield return new WaitForSecondsRealtime(0.25f);
        dialogueText.text = "Play attacks during your action phase to do damage";
        yield return new WaitForSecondsRealtime(0.25f);
        dialogueText.text = "Each point of damage will destroy one card from the opponent's deck";
        yield return new WaitForSecondsRealtime(0.25f);
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
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "Try to defend yourself with a guard";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "Cards with shield give you defence points for the reaction phase";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "These guards have a discard cost";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "Put one of the guards into the discard pile so you can defend";
                break;

            case 1:
                
                playCard("Strike",3);
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "Looks like you dont have enough guards to defend";
                //Make player lose red eye gem
                //playCard("Strike");
                break;

            

        }
        yield return new WaitForSecondsRealtime(0.25f);
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
            print("PLAYER INFO");
            //dialogueText.text = "Burn them all to win!";
            yield return new WaitForSecondsRealtime(0.25f);
        }
        else if (TurnCount == 1)
        {
            dialogueText.text = "Lucky Charm has a special effect";
            yield return new WaitForSecondsRealtime(0.25f);
            dialogueText.text = "Play it to see how it works";
            yield return new WaitForSecondsRealtime(0.25f);
            dialogueText.text = "Play out the new cards you got from the effect";
            yield return new WaitForSecondsRealtime(0.25f);
            dialogueText.text = "Some effects only active in the reactive or active phase";
            yield return new WaitForSecondsRealtime(0.25f);
            dialogueText.text = "You'll get to try these out later";
            yield return new WaitForSecondsRealtime(0.25f);
        }
        else if (TurnCount == 0.25f)
        {
            dialogueText.text = "The enemy is low on cards and vulnerable now!";
            yield return new WaitForSecondsRealtime(0.25f);
            dialogueText.text = "Play the focused stike card for massive damage";
            yield return new WaitForSecondsRealtime(0.25f);
            dialogueText.text = "Active it's powerful burn effect by burning one card before playing this";
            yield return new WaitForSecondsRealtime(0.25f);
            dialogueText.text = "Dont forget the discard cost too!";
            yield return new WaitForSecondsRealtime(0.25f);
        }
        else if (TurnCount == 3)
        {
            //dialogueText.text = "";
        }
        dialogueText.text = " ";
        yield return new WaitForSecondsRealtime(0.25f);
    }


    public IEnumerator Reaction()
    {
       // TurnCount++;
       // print("DUMMY REACTION");
        updateEnemyHand();

        switch (TurnCount)
        {
          

            case 0:
                print("REACTION TEXT");
                //Trigger Prompt
                playCard("Lesser Guard",1);
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "Some cards trigger effects when drawn";
                print(dialogueText.text);
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "These are marked with the 'on arrival' keyword";

                break;
            case 3:
                dialogueText.text = "Congratulations you've finished training";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "An encouter ends once either the player or the monster reach their burn limits or lose all sigils";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "Don't worry your sigils come back after each battle";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "After each victory you will awarded with the your choice of card pack to add to your deck";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "Remember your burn limit persists through encounters";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "You are now ready to delve deeper and face greater challenges";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "Good luck an get ready to burn!";
                yield return new WaitForSecondsRealtime(0.25f);
                dialogueText.text = "";
                //TRIGGER PROMPT FOR PLAYER
                break;



        }
        yield return new WaitForSecondsRealtime(0.25f);
        //dialogueText.text = " ";
     
        print("END ENEMY REACTION");
        StartCoroutine(gameManagerScript.EndEnemyReact());
        //StartCoroutine(PlayerDialogue());
    }

    private void updateEnemyHand()
    {
        dummyHand.Clear();
        dummyHand = new List<GameObject>(handManagerScript.enemyHandlist);
    }
}
