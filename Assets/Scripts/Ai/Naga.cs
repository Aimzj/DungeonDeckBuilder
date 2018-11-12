


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
    //private DeckManager NagaDeck;
    //private EnemyHand NagaHand;

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


    //UPDATED
    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private StatsManager statsManagerScript;
    private GameManager gameManagerScript;

    private List<GameObject> NagaHand = new List<GameObject>();

    private void Start()
    {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        //NagaDeck = gameObject.GetComponent<DeckManager>();
       //NagaHand = GetComponent<EnemyHand>();
        //cardCount();
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

   /* public void TurnStart()//Start Turn
    {

        if (isReactive)//Reactive Functions
        {
            Reaction();
        }

        else
        {

            //NagaDeck.DrawCard(4);
            cardCount();
            Action();//Action functions
        }
      
    }*/

    //Count cards on start


    protected void Reaction()
    {
        UpdateEnemyHand();
        for (int i = NagaHand.Count -1; i >= 0; i--)
        {
        

            if (numEldritchOath > 0)
            {
                for (int x = 0; x <= NagaHand.Count -1; x++)
                {
                    if (NagaHand[x].GetComponent<CardObj>().CardName == "Eldritch Oath")
                    {
                        NagaHand[x].GetComponent<CardMovement>().PlayEnemyCard();
                        print("ELDRITCH OATH");
                       
                        
                        // Play EE
                        //triggerEffects(x);
                        //  Discard(NagaHand.inHand[i].GetComponent<CardObj>().DiscardCost, i);
                        //i = 0;

                        numEldritchOath--;
                        print("EE:" + numEldritchOath);
                       // x = 0;
                    }
                }
                //i = NagaHand.inHand.Count - 1;
            }
           
            else if (Numdebuffs >= 5)
            {
                print("PURGE");
                if (numCotD > 0)
                {
                    NagaHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                    print("CALL OF THE DEEP");
                 
                    
                    //Play card
                    //triggerEffects(i);
                    //Discard(NagaHand[i].GetComponent<CardObj>().DiscardCost, i);
                    //i = NagaHand.Count - 1;
                    numCotD--;
                }
            }
            else if (damageTaken > hp)
            {
                if (numScale > 0)
                {
                    NagaHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                    print("SERPENT SCALE");
                   
                    
                    //Play Card
                    //triggerEffects(i);
                    //Discard(NagaHand[i].GetComponent<CardObj>().DiscardCost, i);
                    i = NagaHand.Count - 1;
                    //damageTaken -= enemyHand[i].GetComponent<CardObj>().Defense;
                    numScale--;
                }
            }
        }
    }




    protected void Action()
    {
        UpdateEnemyHand();
        actionDiscard();
       
    }


    void actionDiscard()
    {
        UpdateEnemyHand();
        for (int i = 0; i <= NagaHand.Count - 1; i++)
        {

            if (numEldritchOath > 0)
            {
                for (int x = 0; x <= NagaHand.Count - 1; x++)
                {
                    if (NagaHand[x].GetComponent<CardObj>().CardName == "Eldritch Oath")
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
            else if (numCrushBlow > 0 && NagaHand.Count >= 2)
            {


                if (NagaHand.Count >= 2)
                {
                    triggerEffects(i);



                    //playcard
                    numCrushBlow--;
                    for (int k = 0; k <= NagaHand.Count - 1; k++)
                    {

                        if (hp > 3 && numScale > 0)
                        {

                            if (NagaHand[k].GetComponent<CardObj>().CardName == "Serpent Scale")
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
                            if (NagaHand[k].GetComponent<CardObj>().CardName == "Call of the Deep")
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
                            if (NagaHand[k].GetComponent<CardObj>().CardName == "Crushing Blow")
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
        NagaHand[index].GetComponent<EnemyCardMove>().state = 2;
        NagaHand[index].GetComponent<EnemyCardMove>().SetTarget(board.transform.position);
        NagaHand[index].GetComponent<BoxCollider>().enabled = false;
        RemoveCard(index);
    }

    void RemoveCard(int arrPos)
    {
        var removedCard = NagaHand[arrPos];
        var dummyCard = removedCard;

      // NagaDeck.discardPile.Add(dummyCard);
       NagaHand.RemoveAt(arrPos);
        // Destroy(removedCard);
      // NagaHand.reOrderHand();

    }

    void RemoveCard(int arrPos, bool isDisc)
    {
        var removedCard = NagaHand[arrPos];
        var dummyCard = removedCard;
        
        //NagaDeck.discardPile.Add(dummyCard);
        NagaHand.RemoveAt(arrPos);
        // Destroy(removedCard);
       // NagaHand.reOrderHand();
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
                if (NagaHand[i].GetComponent<CardObj>().name == "Serpent Scale")
                {
                    print("DISCARD:" + NagaHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                    RemoveCard(i, true);

                    //Remove Card

                }
                else if (NagaHand[i].GetComponent<CardObj>().name == "Eldritch Oath")
                {
                    print("DISCARD:" + NagaHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                    RemoveCard(i, true);
                    //Remove Card

                }
                else if (NagaHand[i].GetComponent<CardObj>().name == "Call of the deep")
                {
                    print("DISCARD:" + NagaHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                    RemoveCard(i, true);
                    //Remove Card

                }
                else if (NagaHand[i].GetComponent<CardObj>().name == "Crushing Blow")
                {
                    print("DISCARD:" + NagaHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                    RemoveCard(i, true);
                    //Remove Card

                }

              
            }

            i++;


        }
      
        triggerEffects(index);
      
    }


    private void UpdateEnemyHand()
    {
        NagaHand.Clear();
        //create a list of the current cards existing in the enemy's hand
        for (int i = 0; i < handManagerScript.enemyDeckList.Count; i++)
        {
            if (handManagerScript.enemyDeckList[i].GetComponent<CardMovement>().isInHand)
                NagaHand.Add(handManagerScript.enemyDeckList[i]);
                

        }

        numEldritchOath = 0;
        numCrushBlow = 0;
        numScale = 0;
        numCotD = 0;

        for (int i = 0; i <= NagaHand.Count - 1; i++)
        {
            if (NagaHand[i].GetComponent<CardObj>().CardName == "Eldritch Oath")
            {
                numEldritchOath++;

            }
            else if (NagaHand[i].GetComponent<CardObj>().CardName == "Crushing Blow")
            {
                numCrushBlow++;

            }
            else if (NagaHand[i].GetComponent<CardObj>().CardName == "Serpent Scale")
            {
                numScale++;
            }
            else if (NagaHand[i].GetComponent<CardObj>().CardName == "Call of the Deep")
            {
                numCotD++;
            }
        }
        print("EEO:" + numEldritchOath);
        print("CB:" + numCrushBlow);
        print("SS:" + numScale);
        print("CotD:" + numCotD);
    }
}
