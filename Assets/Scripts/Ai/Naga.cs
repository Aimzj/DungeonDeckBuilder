using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naga : MonoBehaviour {

    //Required Lists
    private List<GameObject> enemyHand;
    private bool isReactive = false; // Get from master scriopt
    [SerializeField]
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> inHand = new List<GameObject>();

    //Outside Scripts
    private DeckManager NagaDeck;
    private EnemyHand NagaHand;

    //Card Counting
    private int numEldritchOath = 0;
    private int numCrushBlow = 0;
    private int numScale = 0;
    private int numCotD = 0;

    //Stat tracker
    private int hp = 10;
    private int sigilCount = 4;
    private int handSize = 4;
    public int Numdebuffs = 0;
    private int damageTaken = 0;

    //Zones
    private GameObject discardZone;
    [SerializeField]
    private GameObject deckZone;
    [SerializeField]
    private GameObject board;

    private void Start()
    {
       NagaDeck = gameObject.GetComponent<DeckManager>();
       NagaHand = GetComponent<EnemyHand>();
        cardCount();
    }

    public void TurnStart()//Start Turn
    {

        if (isReactive)//Reactive Functions
        {
            Reaction();
        }

        else
        {
            Action();//Action functions
        }
      
    }

    //Count cards on start
    protected void cardCount()
    {
        for(int i = 0; i <= NagaHand.inHand.Count; i++)
        {
            if(enemyHand[i].GetComponent<CardObj>().CardName == "Eldritch Oath")
            {
                numEldritchOath++;
            }
            else if (enemyHand[i].GetComponent<CardObj>().CardName == "Crushing Blow")
            {
                numCrushBlow++;
            }
            else if (enemyHand[i].GetComponent<CardObj>().CardName == "Serpent's Scale")
            {
                numScale++;
            }
            else if (enemyHand[i].GetComponent<CardObj>().CardName == "Call of the Deep")
            {
                numCotD++;
            }
        }
    }

    protected void Reaction()
    {
        for (int i = NagaHand.inHand.Count -1; i >= 0; i--)
        {
            //Reshuffle if deck empty
            if (NagaDeck.deck.Count <= 0)
            {
                print("EMPTY");
                NagaDeck.Reshuffle();
             

            }

            //Start of priority queue
            if (numEldritchOath > 0)
            {
               
                if(NagaHand.inHand[i].GetComponent<CardObj>().CardName == "Eldritch Oath")
                {
                    // Play EE
                    Discard(NagaHand.inHand[i].GetComponent<CardObj>().DiscardCost, i);
                    i = NagaHand.inHand.Count - 1;
                }
                numEldritchOath--;
                i = NagaHand.inHand.Count - 1;
            }
         else if(Numdebuffs  >= 5)
            {
                if(numCotD > 0)
                {
                    //Play card
                    Discard(NagaHand.inHand[i].GetComponent<CardObj>().DiscardCost, i);
                    i = NagaHand.inHand.Count - 1;
                    numCotD--;
                }
            }
         else if(damageTaken > hp)
            {
                if(numScale > 0)
                {
                    //Play Card
                    Discard(NagaHand.inHand[i].GetComponent<CardObj>().DiscardCost, i);
                    i = NagaHand.inHand.Count - 1;
                    damageTaken -= enemyHand[i].GetComponent<CardObj>().Defense;
                    numScale--;
                }
            }
        }
    }




    protected void Action()
    {

         for (int i = 0; i <= enemyHand.Count; i++)
        {
        if(numEldritchOath > 0)
            {
                //playcard

                numEldritchOath--;
            }
        else   if (numCrushBlow > 0)
            {
                //playcard
               numCrushBlow--;
                if(enemyHand.Count >= 2)
                {
                    for(int k = 0; k < 2; k ++)
                    {
                        if (hp > 3 && numScale > 0)
                        {
                            //Discard Serpent scale
                        }
                        else if (numCotD > 0)
                        {
                            //Discard Cotd
                        }
                    }
                    
                }
                
            }

        }
    }

    public void Draw(int drawNum)
    {
        //Draws Cards
    }

    void triggerEffects(int index)
    {
        NagaHand.inHand[index].GetComponent<EnemyCardMove>().state = 2;
        NagaHand.inHand[index].GetComponent<EnemyCardMove>().SetTarget(board.transform);
        NagaHand.inHand[index].GetComponent<BoxCollider>().enabled = false;
        RemoveCard(index);
    }

    void RemoveCard(int arrPos)
    {
        var removedCard = NagaHand.inHand[arrPos];
        var dummyCard = removedCard;

       NagaDeck.discardPile.Add(dummyCard);
       NagaHand.inHand.RemoveAt(arrPos);
        // Destroy(removedCard);
       NagaHand.reOrderHand();

    }

    public void Discard(int Cost, int index)
    {
        int remainingCost = Cost;
        int i = 0;
        bool foundCard = false;
        while(remainingCost > 0)
        {
            if(numCotD > 0)
            {
                while (!foundCard)
                {
                    if(remainingCost == 0)

                    {
                        foundCard = true;
                    }
                    else
                    {
                        if (NagaHand.inHand[i].GetComponent<CardObj>().name == "Eldritch Oath")
                        {
                            remainingCost--;
                            RemoveCard(i);
                            //Remove Card

                        }
                        else if (NagaHand.inHand[i].GetComponent<CardObj>().name == "Call of the deep")
                        {
                            remainingCost--;
                            RemoveCard(i);
                            //Remove Card

                        }
                        else if (NagaHand.inHand[i].GetComponent<CardObj>().name == "Crushing Blow")
                        {
                            remainingCost--;
                            RemoveCard(i);
                            //Remove Card

                        }
                    }
                   
                }
            }
            
        }
        //Discard Cost

        RemoveCard(index);
    }
}
