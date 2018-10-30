


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
    private GameObject[] clearList;

    //Outside Scripts
    private DeckManager NagaDeck;
    private EnemyHand NagaHand;

    //Card Counting
    private int numEldritchOath = 0;
    private int numCrushBlow = 0;
    private int numScale = 0;
    private int numCotD = 0;

    //Stat tracker
    public int hp = 10;
    public int sigilCount = 4;
    public int handSize = 4;
    public int Numdebuffs = 10;
    private int damageTaken = 0;
    int totalDamage = 0;

    //Zones
    [SerializeField]
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Reaction();

        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Action();

        }


        if (Input.GetKeyUp(KeyCode.E))
        {
            NagaDeck.DrawCard(4);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        clearList = GameObject.FindGameObjectsWithTag("EnemyCard");
        foreach (var card in clearList)
        {
            if (card.GetComponent<EnemyCardMove>().state == 2)
                card.GetComponent<EnemyCardMove>().SetTarget(discardZone.transform.position);
        }

        totalDamage = 0;
    }

    public void TurnStart()//Start Turn
    {

        if (isReactive)//Reactive Functions
        {
            Reaction();
        }

        else
        {

            NagaDeck.DrawCard(4);
            cardCount();
            Action();//Action functions
        }
      
    }

    //Count cards on start
    protected void cardCount()
    {

        numEldritchOath = 0;
        numCrushBlow = 0;
        numScale = 0;
        numCotD = 0;

        for (int i = 0; i <= NagaHand.inHand.Count -1; i++)
        {
            if(NagaHand.inHand[i].GetComponent<CardObj>().CardName == "Eldritch Oath")
            {
                numEldritchOath++;
                
            }
            else if (NagaHand.inHand[i].GetComponent<CardObj>().CardName == "Crushing Blow")
            {
                numCrushBlow++;

            }
            else if (NagaHand.inHand[i].GetComponent<CardObj>().CardName == "Serpent Scale")
            {
                numScale++;
            }
            else if (NagaHand.inHand[i].GetComponent<CardObj>().CardName == "Call of the Deep")
            {
                numCotD++;
            }
        }
        print("EEO:" + numEldritchOath);
        print("CB:" + numCrushBlow);
        print("SS:" + numScale);
        print("CotD:" + numCotD);
    }

    protected void Reaction()
    {
        cardCount();
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
                for (int x = 0; x <= NagaHand.inHand.Count -1; x++)
                {
                    if (NagaHand.inHand[x].GetComponent<CardObj>().CardName == "Eldritch Oath")
                    {
                        print("ELDRITCH OATH");
                        // Play EE
                        triggerEffects(x);
                        //  Discard(NagaHand.inHand[i].GetComponent<CardObj>().DiscardCost, i);
                        //i = 0;

                        numEldritchOath--;
                        print("EE:" + numEldritchOath);
                       // x = 0;
                    }
                }
                //i = NagaHand.inHand.Count - 1;
            }
            /* if (numEldritchOath > 0)
             {

                 if(NagaHand.inHand[i].GetComponent<CardObj>().CardName == "Eldritch Oath")
                 {
                     print("ELDRITCH OATH");
                     // Play EE
                     triggerEffects(i);
                     Discard(NagaHand.inHand[i].GetComponent<CardObj>().DiscardCost, i);
                     i = NagaHand.inHand.Count - 1;
                 }
                 numEldritchOath--;
                 i = NagaHand.inHand.Count - 1;
             }*/
            else if (Numdebuffs >= 5)
            {
                print("PURGE");
                if (numCotD > 0)
                {
                    print("CALL OF THE DEEP");
                    //Play card
                    //triggerEffects(i);
                    Discard(NagaHand.inHand[i].GetComponent<CardObj>().DiscardCost, i);
                    i = NagaHand.inHand.Count - 1;
                    numCotD--;
                }
            }
            else if (damageTaken > hp)
            {
                if (numScale > 0)
                {
                    print("SERPENT SCALE");
                    //Play Card
                    triggerEffects(i);
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
        cardCount();
        actionDiscard();
       
    }


    void actionDiscard()
    {
        cardCount();
        for (int i = 0; i <= NagaHand.inHand.Count - 1; i++)
        {

            if (numEldritchOath > 0)
            {
                for (int x = 0; x <= NagaHand.inHand.Count - 1; x++)
                {
                    if (NagaHand.inHand[x].GetComponent<CardObj>().CardName == "Eldritch Oath")
                    {
                        print("ELDRITCH OATH");
                        // Play EE
                        triggerEffects(x);
                        //  Discard(NagaHand.inHand[i].GetComponent<CardObj>().DiscardCost, i);
                        //i = 0;

                        numEldritchOath--;
                        print("EE:" + numEldritchOath);
                    }
                }

            }
            else if (numCrushBlow > 0 && NagaHand.inHand.Count >= 2)
            {


                if (NagaHand.inHand.Count >= 2)
                {
                    triggerEffects(i);



                    //playcard
                    numCrushBlow--;
                    for (int k = 0; k <= NagaHand.inHand.Count - 1; k++)
                    {

                        if (hp > 3 && numScale > 0)
                        {

                            if (NagaHand.inHand[k].GetComponent<CardObj>().CardName == "Serpent Scale")
                            {
                                print("DISCARD");
                                RemoveCard(k, true);
                                //  k = NagaHand.inHand.Count - 1;
                            }
                            //Discard Serpent scale
                        }
                        else if (numCotD > 0)
                        {
                            //print("CRUSHING BLOW1");
                            if (NagaHand.inHand[k].GetComponent<CardObj>().CardName == "Call of the Deep")
                            {
                                print("DISCARD");
                                RemoveCard(k, true);
                                // k = NagaHand.inHand.Count - 1;
                            }
                            //Discard Cotd
                        }
                        else
                        {
                            print("CRUSHING BLOW2");
                            if (NagaHand.inHand[k].GetComponent<CardObj>().CardName == "Crushing Blow")
                            {
                                print("DISCARD");
                                RemoveCard(k, true);
                                // k = NagaHand.inHand.Count - 1;
                            }
                        }
                    }

                }
                i = 0;
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
        NagaHand.inHand[index].GetComponent<EnemyCardMove>().SetTarget(board.transform.position);
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

    void RemoveCard(int arrPos, bool isDisc)
    {
        var removedCard = NagaHand.inHand[arrPos];
        var dummyCard = removedCard;
        
        NagaDeck.discardPile.Add(dummyCard);
        NagaHand.inHand.RemoveAt(arrPos);
        // Destroy(removedCard);
        NagaHand.reOrderHand();
        dummyCard.GetComponent<EnemyCardMove>().SetTarget(discardZone.transform.position);
    }

    //CHANGE TO USE NESTED FOR LOOPS
    public void Discard(int Cost, int index)
    {
        int remainingCost = Cost;
        int i = 0;
        //bool foundCard = false;

       
        while(remainingCost > 0)
        {
           
            print("remaining cost is" + remainingCost);

            if (i!= index)
            {
                print(i);
                if (NagaHand.inHand[i].GetComponent<CardObj>().name == "Serpent Scale")
                {
                    print("DISCARD:" + NagaHand.inHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                    RemoveCard(i, true);

                    //Remove Card

                }
                else if (NagaHand.inHand[i].GetComponent<CardObj>().name == "Eldritch Oath")
                {
                    print("DISCARD:" + NagaHand.inHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                    RemoveCard(i, true);
                    //Remove Card

                }
                else if (NagaHand.inHand[i].GetComponent<CardObj>().name == "Call of the deep")
                {
                    print("DISCARD:" + NagaHand.inHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                    RemoveCard(i, true);
                    //Remove Card

                }
                else if (NagaHand.inHand[i].GetComponent<CardObj>().name == "Crushing Blow")
                {
                    print("DISCARD:" + NagaHand.inHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                    RemoveCard(i, true);
                    //Remove Card

                }

              
            }

            i++;


        }
      
        triggerEffects(index);
      
    }
}
