using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {


  
    private DeckManager spiderDeck;
    private EnemyHand spiderHand;

    [SerializeField]
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> inHand = new List<GameObject>();

    // Use this for initialization

    private void Start()
    {

        spiderDeck = gameObject.GetComponent<DeckManager>();
        spiderHand = GetComponent<EnemyHand>();
    }

    //Just to test functionality
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            TurnStart();
        }

        if(Input.GetKeyUp(KeyCode.E))
        {
            spiderDeck.DrawCard(4);
        }
    }

    //Trigger turn start and then all ohter related functions
    public void TurnStart()
    {
        // deck = spiderDeck.deck;
        //inHand = spiderHand.inHand;
        // int totalHand = spiderHand.inHand.Count-1;


        //Loop through each card in hand
        for (int i = spiderHand.inHand.Count -1 ; i >= 0 ; i--)
            //for (int i = 0 ; i <= spiderHand.inHand.Count  ; i++)
        {
<<<<<<< HEAD
            //Reshuffle if deck is empty
            if (spiderDeck.deck.Count <= 0)
=======
            if(enemyHand[i].GetComponent<CardObj>().CardName == "Skitter")
>>>>>>> e93707e19e116c1ed865d4557c03675ca2ef577b
            {
                print("EMPTY");
                spiderDeck.Reshuffle();
                //spiderHand.inHand.fin

            }
            if (spiderHand.inHand[i].GetComponent<CardObj>().Card_Name == "Skitter")
            {
                print("Skitter Played");
             
               
                RemoveCard(i);
                spiderDeck.DrawCard(1);
                // i = 0;
                i = spiderHand.inHand.Count - 1;



            }
<<<<<<< HEAD
            else if (spiderHand.inHand[i].GetComponent<CardObj>().Card_Name == "Bite")
=======
            if(enemyHand[i].GetComponent<CardObj>().CardName == "Bite")
>>>>>>> e93707e19e116c1ed865d4557c03675ca2ef577b
            {
                print("Bite Played");
                RemoveCard(i);
                // i = 0;
                i = spiderHand.inHand.Count - 1;
            }
        }

    
    }

   /*void reshuffle()
    {
        //spiderDeck.deck = new List<GameObject>();
        spiderDeck.deck = spiderDeck.discardPile;
        print("RELOAD");
        spiderDeck.discardPile = new List<GameObject>(); 
    }*/

    void RemoveCard(int arrPos)
    {
        var removedCard = spiderHand.inHand[arrPos];
        var dummyCard = removedCard;
        
        spiderDeck.discardPile.Add(dummyCard);
        spiderHand.inHand.RemoveAt(arrPos);
       // Destroy(removedCard);
        spiderHand.reOrderHand();

    }

	
}
