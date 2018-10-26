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
            //Reshuffle if deck is empty
            if (spiderDeck.deck.Count <= 0)
            {
                print("EMPTY");
                spiderDeck.Reshuffle();
                //spiderHand.inHand.fin

            }
            if (spiderHand.inHand[i].GetComponent<CardObj>().CardName == "Skitter")
            {
                print("Skitter Played");
             
               
                RemoveCard(i);
                spiderDeck.DrawCard(1);
                // i = 0;
                i = spiderHand.inHand.Count - 1;



            }
            else if (spiderHand.inHand[i].GetComponent<CardObj>().CardName == "Bite")
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
