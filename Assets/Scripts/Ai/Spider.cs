using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {


  
    private DeckManager spiderDeck;
    private EnemyHand spiderHand;
    [SerializeField]
    private GameObject discardZone;
    [SerializeField]
    private GameObject deckZone;
    [SerializeField]
    private GameObject board;

    [SerializeField]
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> inHand = new List<GameObject>();
    private List<GameObject> onBoard = new List<GameObject>( new GameObject[6]);


    int totalDamage = 0;

    private GameObject[] clearList;
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

        if (Input.GetKeyUp(KeyCode.W))
        {
            EndTurn();
        }
    }


    public void EndTurn()
    {
        clearList = GameObject.FindGameObjectsWithTag("EnemyCard");
        foreach(var card in clearList)
        {
            if (card.GetComponent<EnemyCardMove>().state == 2)
            card.GetComponent<EnemyCardMove>().SetTarget(discardZone.transform.position);
        }

        totalDamage = 0;
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
            print("TEST");
            //Reshuffle if deck is empty
            if (spiderDeck.deck.Count <= 0)
            {
                print("EMPTY");
                spiderDeck.Reshuffle();
             

            }
            if (spiderHand.inHand[i].GetComponent<CardObj>().CardName == "Skitter")
            {
                // print("Skitter Played");
                triggerEffects(i);
                
                spiderDeck.DrawCard(1);
                // i = 0;
                i = spiderHand.inHand.Count - 1;
                totalDamage++;



            }
            else if (spiderHand.inHand[i].GetComponent<CardObj>().CardName == "Bite")
            {
                //print("Bite Played");
                triggerEffects(i);
                // i = 0;
                i = spiderHand.inHand.Count - 1;
            }
        }

    
    }

    void triggerEffects(int index)
    {
        spiderHand.inHand[index].GetComponent<EnemyCardMove>().state = 2;
       
        spiderHand.inHand[index].GetComponent<BoxCollider>().enabled = false;
        setBoard(spiderHand.inHand[index]);
        RemoveCard(index);
       
    }

    void setBoard(GameObject CurrentCard)
    {
       
        onBoard.Add(CurrentCard);
        int numcards = onBoard.Count;
        // CurrentCard.GetComponent<EnemyCardMove>().SetTarget(board.transform.position);

        while (numcards >= 0)
        {
            CurrentCard.GetComponent<EnemyCardMove>().SetTarget(board.transform.position);
            if (onBoard[(onBoard.Count -1)/2])
            {
               
            }
            numcards--;
        }
    }

    //Add to discard list
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
