﻿


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

    public int DamageTaken;
    public int HP;
    public int NumSigilsInBurn;
    public int sizeBurnPile;

    //Card Counting
    private int numEldritchOath = 0;
    private int numCrushBlow = 0;
    private int numScale = 0;
    private int numCotD = 0;

    //Cards in play
    public int NumCBInPlay = 0;
    public int numRemoveCards = 0;


    //UPDATED
    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private StatsManager statsManagerScript;
    private GameManager gameManagerScript;
    private CardGenerator cardGenScript;

    //AREAS
    private Transform playerTempDisplay, enemyTempDisplay;
    private Transform playerDeck;

    [SerializeField]
    private List<GameObject> NagaHand = new List<GameObject>();

    private void Start()
    {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        cardGenScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();


        playerTempDisplay = GameObject.Find("PlayerTempDisplay").GetComponent<Transform>();
        enemyTempDisplay = GameObject.Find("EnemyTempDisplay").GetComponent<Transform>();
        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();

    }

    public void StopEverything()
    {
        StopAllCoroutines();
        NagaHand.Clear();
    }

    IEnumerator reactionDiscard(int cost, int index)
    {
        while (cost != 0)
        {
            print("DISCARD");
            if (numCotD > 0)
            {
                for (int k = 0; k > NagaHand.Count; k++)
                {
                    if (cost > 0 && NagaHand[k].GetComponent<CardObj>().CardName == "Call of the Deep" && k != index)
                    {
                        numCotD--;
                        cost--;
                        NagaHand[k].GetComponent<CardMovement>().DiscardEnemyCard();
                        NagaHand.RemoveAt(k);
                        yield return new WaitForSecondsRealtime(1);
                    }
                }
            }

            else if (numScale > 0)
            {
                for (int k = 0; k > NagaHand.Count; k++)
                {
                    if (cost > 0 && NagaHand[k].GetComponent<CardObj>().CardName == "Serpent's Scale" && k != index)
                    {
                        numScale--;
                        cost--;
                        NagaHand[k].GetComponent<CardMovement>().DiscardEnemyCard();
                        NagaHand.RemoveAt(k);
                        yield return new WaitForSecondsRealtime(1);
                    }
                }
            }

            else if (numCrushBlow > 0)
            {
                for (int k = 0; k > NagaHand.Count; k++)
                {
                    if (cost > 0 && NagaHand[k].GetComponent<CardObj>().CardName == "Crushing Blow" && k != index)
                    {
                        
                        numCrushBlow--;
                        cost--;
                        NagaHand[k].GetComponent<CardMovement>().DiscardEnemyCard();
                        NagaHand.RemoveAt(k);
                        yield return new WaitForSecondsRealtime(1);
                    }
                }
            }


        }
    }

    public IEnumerator Reaction()
    {
        DamageTaken = statsManagerScript.numAttack;
       print("DAMAGE:" + DamageTaken);
        print("START ENEMY REACTION");
        HP = statsManagerScript.numHealth_enemy;
        NumSigilsInBurn = statsManagerScript.numSigilsRemaining_enemy;
        UpdateEnemyHand();
        for (int i = NagaHand.Count -1; i >= 0; i--)
        {


            if (numEldritchOath > 0 && NagaHand.Count >= 2 && HP < DamageTaken)
            {
                for (int x = 0; x <= NagaHand.Count - 1; x++)
                {
                    if (NagaHand[x].GetComponent<CardObj>().CardName == "Eldritch Oath")
                    {
                        // int cost = 1;
                        StartCoroutine(attackDiscard(1, x));
                        //TRIGGER DISCARD FIRST


                        NagaHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                        statsManagerScript.UpdateAttack("enemy", NagaHand[i].GetComponent<CardObj>().Attack);
                        NagaHand[i].GetComponent<CardObj>().Attack += 3;
                        NagaHand[i].GetComponent<CardObj>().Defense += 2;
                        print("ELDRITCH OATH");


                        numEldritchOath--;
                        print("EE:" + numEldritchOath);
                        yield return new WaitForSecondsRealtime(1);
                    }
                }

            }
        
           
            else if (DamageTaken > 0 && numScale > 0)
            {
                print("SERPENT SCALE");
                bool isfound = false;
                int index = 0;
                while(!isfound)
                {
                    for(int z = 0; z < NagaHand.Count; z ++)
                    {
                        if(NagaHand[z].GetComponent<CardObj>().CardName == "Serpent's Scale" && DamageTaken > 0)
                        {
                            isfound = true;
                            NagaHand[z].GetComponent<CardMovement>().PlayEnemyCard();

                            statsManagerScript.UpdateDefense("enemy", NagaHand[z].GetComponent<CardObj>().Defense);
                           // i = NagaHand.Count - 1;
                            DamageTaken -= NagaHand[z].GetComponent<CardObj>().Defense;
                            print("Damage Taken by Naga:" + DamageTaken);
                            if(DamageTaken > 0)
                            {
                                //statsManagerScript.UpdateAttack("enemy", 1);
                                yield return new WaitForSecondsRealtime(1);
                                giveWound();
                                yield return new WaitForSecondsRealtime(1);
                                giveWound();
                                yield return new WaitForSecondsRealtime(1);
                            }
                            numScale--;
                            yield return new WaitForSecondsRealtime(1);
                        }
                    }
                }
                
               
                
            }
        }

        if(NumSigilsInBurn <= 2 && areaManagerScript.enemy_TrashCardList.Count >= 5)
        {
           StartCoroutine(CalloftheDeep());
          
        }

        print("DONE WITH ENEMY REACTION");
        StartCoroutine(gameManagerScript.EndEnemyReact());
    }

    IEnumerator CalloftheDeep()
    {
        bool foundSigil = false;
        int numCard = 2;
        for (int k = 0; k < NagaHand.Count; k++)
        {
            if (NagaHand[k].GetComponent<CardObj>().CardName == "Call of the Deep")
            {
                StartCoroutine(attackDiscard(1, k));
                NagaHand[k].GetComponent<CardMovement>().PlayEnemyCard();
                //statsManagerScript.UpdateAttack("enemy", 1);
                for (int i = 0; i < areaManagerScript.enemy_TrashCardList.Count; i++)
                {
                    if(!foundSigil && numCard > 0)
                    {
                        if(areaManagerScript.enemy_TrashCardList[i].GetComponent<CardObj>().SigilNum > 0)
                        {
                            numCard--;
                            //handManagerScript.enemyHandlist.Add(areaManagerScript.enemy_TrashCardList[i]);
                            var tempCard = areaManagerScript.enemy_TrashCardList[i];
                            areaManagerScript.enemy_TrashCardList.RemoveAt(i);
                            areaManagerScript.enemy_TrashCardList.Add(tempCard);
                            StartCoroutine(handManagerScript.DrawTrash(1, "enemy"));

                            foundSigil = true;
                            
                        }
                    }
                    else
                    {
                        numCard--;
                        handManagerScript.Shuffle(ref areaManagerScript.enemy_DiscardCardList);
                        StartCoroutine(handManagerScript.DrawTrash(1, "enemy"));
                    }
                }
                numCotD--;
                UpdateEnemyHand();
                yield return new WaitForSecondsRealtime(1);
            }
        }
        for (int i = 0; i < handManagerScript.enemyHandlist.Count; i++)
        {
            handManagerScript.enemyHandlist[i].GetComponent<CardMovement>().isPlayed = false;
            handManagerScript.enemyHandlist[i].GetComponent<CardMovement>().isInHand = true;
        }
        numCard = 2;
    }

    void giveWound()
    {
        print("GIVE WOUND");
        //Check for wound cards
        if(cardGenScript.WoundDeck.Count > 0)
        {
            int rand = Random.Range(0, handManagerScript.playerDeckList.Count - 1);
            handManagerScript.playerDeckList.Insert(rand, cardGenScript.WoundDeck[0]);

            StartCoroutine(areaManagerScript.TempDisplay(cardGenScript.WoundDeck[0], playerTempDisplay, playerDeck));
            cardGenScript.WoundDeck.RemoveAt(0);
            statsManagerScript.UpdateCardsInDeck("player", 1, 1);

        }
    }

    public void CrushingBlow( int damagedealt)
    {
        if(NumCBInPlay > areaManagerScript.player_PlayCardList.Count)
        {
            NumCBInPlay = areaManagerScript.player_PlayCardList.Count;
        }
        print("CRUSHING BLOW EFFECT");
        int PlayerDefTotal = statsManagerScript.numDefense;
        print("Player defence is : " + PlayerDefTotal);
        for (int i = 0; i < NumCBInPlay; i ++)
        {
            //PlayerDefTotal-=3;
         
            if(damagedealt > 0)
            {
                print("Player defence is : " + PlayerDefTotal);
                if (areaManagerScript.player_PlayCardList.Count > 0)
                {
                    numRemoveCards++;
                    print("Num Cards to Remove is :" + numRemoveCards);
                    areaManagerScript.Call_TrashCard(areaManagerScript.player_PlayCardList[i], "player");
                    //areaManagerScript.player_PlayCardList.RemoveAt(areaManagerScript.player_PlayCardList.Count);
                    print("CARDS BIATCH");
                 
                    
                   
                   
                }

            }
        }
    }

  IEnumerator attackDiscard(int cost, int index)
    {
        while (cost > 0)
        {
            // counter++;
            print("DISCARD");
            if (numCotD > 0)
            {


                for (int k = 0; k < NagaHand.Count; k++)
                {

                    if (cost > 0 && NagaHand[k].GetComponent<CardObj>().CardName == "Call of the Deep")
                    {
                        print("DISCARD CALL");
                        numCotD--;
                        cost--;
                        NagaHand[k].GetComponent<CardMovement>().DiscardEnemyCard();

                        //areaManagerScript.Call_DiscardCard(NagaHand[k], "enemy");

                        NagaHand.RemoveAt(k);
                        yield return new WaitForSecondsRealtime(1);
                        // k = 0;


                    }
                }
            }

            else if (numScale > 0)
            {

                for (int k = 0; k < NagaHand.Count; k++)
                {

                    if (cost > 0 && NagaHand[k].GetComponent<CardObj>().CardName == "Serpent's Scale")
                    {
                        print("DISCARD SCALES");
                        numScale--;
                        cost--;
                        NagaHand[k].GetComponent<CardMovement>().DiscardEnemyCard();
                        //areaManagerScript.Call_DiscardCard(NagaHand[k], "enemy");

                        NagaHand.RemoveAt(k);
                        yield return new WaitForSecondsRealtime(1);
                        // k = 0;


                    }
                }
            }

            else if (numCrushBlow > 0)
            {

                for (int k = 0; k < NagaHand.Count; k++)
                {

                    if (cost > 0 && NagaHand[k].GetComponent<CardObj>().CardName == "Crushing Blow")
                    {
                        print("DISCARD BLOW");
                        numCrushBlow--;
                        cost--;
                        NagaHand[k].GetComponent<CardMovement>().DiscardEnemyCard();
                        //areaManagerScript.Call_DiscardCard(NagaHand[k], "enemy");
                        NagaHand.RemoveAt(k);
                        yield return new WaitForSecondsRealtime(1);


                    }
                }
            }


        }
    }


    public IEnumerator Action()
    {
        numRemoveCards = 0;
        NumCBInPlay = 0;
        print("ENEMY ACTION");
        UpdateEnemyHand();
        HP = statsManagerScript.numHealth_enemy;
        //UpdateEnemyHand();
        for (int i = 0; i < NagaHand.Count ; i++)
        {

            if (numEldritchOath > 0 && NagaHand.Count >= 2)
            {
                
                for (int x = 0; x <= NagaHand.Count - 1; x++)
                {
                    if (NagaHand[x].GetComponent<CardObj>().CardName == "Eldritch Oath")
                    {
                        NagaHand[x].GetComponent<CardMovement>().PlayEnemyCard();
                        statsManagerScript.UpdateAttack("enemy", NagaHand[x].GetComponent<CardObj>().Attack);
                        NagaHand[x].GetComponent<CardObj>().Attack += 3;
                        NagaHand[x].GetComponent<CardObj>().Defense += 2;
                        print("ELDRITCH OATH");


                        numEldritchOath--;
                        print("EE:" + numEldritchOath);
                        print("ELDRTICH OATH");
                        int counter = 0;
                        int cost = 1;

                        //DISCARD
                        StartCoroutine (attackDiscard(1, x));
                        
                    
                                                
                        yield return new WaitForSecondsRealtime(1);
                    }
                }

            }
            else if (numCrushBlow > 0)
            {
                for (int k = 0; k < NagaHand.Count; k ++)
                {
                    if (NagaHand[k].GetComponent <CardObj>().CardName == "Crushing Blow")
                    {
                        NumCBInPlay++;
                        NagaHand[k].GetComponent<CardMovement>().PlayEnemyCard();
                        statsManagerScript.UpdateAttack("enemy", NagaHand[k].GetComponent<CardObj>().Attack);
                        numCrushBlow--;
                        yield return new WaitForSecondsRealtime(1);
                    }
                }               
                              
            }
           
        }
        print("END ENEMY ACTION");
        gameManagerScript.EndEnemyTurn();
    }



 

    private void UpdateEnemyHand()
    {
     
        NagaHand.Clear();
        NagaHand = new List<GameObject>(handManagerScript.enemyHandlist);
        for (int i = 0; i < handManagerScript.enemyHandlist.Count; i++)
        {
           
           // NagaHand.Add(handManagerScript.enemyHandlist[i]);

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
            else if (NagaHand[i].GetComponent<CardObj>().CardName == "Serpent's Scale")
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
