


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


    //UPDATED
    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private StatsManager statsManagerScript;
    private GameManager gameManagerScript;
    [SerializeField]
    private List<GameObject> NagaHand = new List<GameObject>();

    private void Start()
    {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

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
        DamageTaken = statsManagerScript.numAttack_player;
       print("DAMAGE:" + DamageTaken);
        print("START ENEMY REACTION");
        HP = statsManagerScript.numHealth_enemy;
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
                        NagaHand[i].GetComponent<CardObj>().Attack += 2;
                        NagaHand[i].GetComponent<CardObj>().Defense += 1;
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

                            statsManagerScript.UpdateDefense("enemy", 3);
                           // i = NagaHand.Count - 1;
                            DamageTaken -= 3;
                            numScale--;
                            yield return new WaitForSecondsRealtime(1);
                        }
                    }
                }
                
               
                
            }
        }

        if(NumSigilsInBurn >= 2 && areaManagerScript.enemy_TrashCardList.Count >= 5)
        {
            for (int k = 0; k < NagaHand.Count; k++)
            {
                if (NagaHand[k].GetComponent<CardObj>().CardName == "Call of the Deep")
                {
                    StartCoroutine(attackDiscard(1, k));
                    NagaHand[k].GetComponent<CardMovement>().PlayEnemyCard();
                    statsManagerScript.UpdateAttack("enemy", 1);
                    numCotD--;
                    yield return new WaitForSecondsRealtime(1);
                }
            }

        }

        print("DONE WITH ENEMY REACTION");
        gameManagerScript.EndEnemyReact();
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
                        NagaHand[x].GetComponent<CardObj>().Attack += 2;
                        NagaHand[x].GetComponent<CardObj>().Defense += 1;
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
                        NagaHand[k].GetComponent<CardMovement>().PlayEnemyCard();
                        statsManagerScript.UpdateAttack("enemy", 3);
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

        for (int i = 0; i < handManagerScript.enemyHandlist.Count; i++)
        {

            NagaHand.Add(handManagerScript.enemyHandlist[i]);

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
