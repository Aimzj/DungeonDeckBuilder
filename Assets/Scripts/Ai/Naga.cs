


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

    private List<GameObject> NagaHand = new List<GameObject>();

    private void Start()
    {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

  


    public IEnumerator Reaction()
    {
        HP = statsManagerScript.numHealth_enemy;
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
                        statsManagerScript.UpdateDefense("enemy", NagaHand[i].GetComponent<CardObj>().Defense);
                        NagaHand[i].GetComponent<CardObj>().Attack += 2;
                        NagaHand[i].GetComponent<CardObj>().Defense += 1;
                        print("ELDRITCH OATH");
                     
                        numEldritchOath--;
                        print("EE:" + numEldritchOath);
                       
                        yield return new WaitForSecondsRealtime(1);
                    }
                }
               
            }

           //PLAYER HAS NO STATUS EFFECT CARDS NOW SO THIS IS UNNEEDED
           /* else if (Numdebuffs >= 5)
            {
                print("PURGE");
                if (numCotD > 0)
                {
                    NagaHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                    print("CALL OF THE DEEP");
                    statsManagerScript.UpdateDefense("enemy", 1);
                
                    numCotD--;
                    yield return new WaitForSecondsRealtime(1);
                }
            }*/

            if (DamageTaken > HP)
            {
                if (numScale > 0)
                {
                    NagaHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                    print("SERPENT SCALE");
                    statsManagerScript.UpdateDefense("enemy", 3);           
                    i = NagaHand.Count - 1;
                    
                    numScale--;
                    yield return new WaitForSecondsRealtime(1);
                }
            }
        }
    }



    public IEnumerator Action()
    {
        UpdateEnemyHand();
        actionDiscard();
        yield return new WaitForSecondsRealtime(1);

    }


    IEnumerator actionDiscard()
    {
        HP = statsManagerScript.numHealth_enemy;
        UpdateEnemyHand();
        for (int i = 0; i <= NagaHand.Count - 1; i++)
        {

            if (numEldritchOath > 0)
            {
                for (int x = 0; x <= NagaHand.Count - 1; x++)
                {
                    if (NagaHand[x].GetComponent<CardObj>().CardName == "Eldritch Oath")
                    {
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
            else if (numCrushBlow > 0 && NagaHand.Count >= 2)
            {


                if (NagaHand.Count >= 2)
                {
                    NagaHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                    statsManagerScript.UpdateAttack("enemy", NagaHand[i].GetComponent<CardObj>().Attack);



                    //playcard
                    numCrushBlow--;
                    for (int k = 0; k <= NagaHand.Count - 1; k++)
                    {

                        if (HP > 3 && numScale > 0)
                        {

                            if (NagaHand[k].GetComponent<CardObj>().CardName == "Serpent Scale")
                            {
                                areaManagerScript.enemy_DiscardCardList.Add(NagaHand[k]);
                                areaManagerScript.Call_DiscardCard(NagaHand[k], "player");
                                NagaHand.RemoveAt(k);
                                k = NagaHand.Count - 1;
                                print("DISCARD");
                              
                            }
                            //Discard Serpent scale
                        }
                        else if (numCotD > 0)
                        {
                            //print("CRUSHING BLOW1");
                            if (NagaHand[k].GetComponent<CardObj>().CardName == "Call of the Deep")
                            {
                                areaManagerScript.enemy_DiscardCardList.Add(NagaHand[k]);
                                NagaHand.RemoveAt(k);
                                k = NagaHand.Count - 1;
                                print("DISCARD");
                            
                            }
                            //Discard Cotd
                        }
                        else
                        {
                            print("CRUSHING BLOW2");
                            if (NagaHand[k].GetComponent<CardObj>().CardName == "Crushing Blow")
                            {
                                areaManagerScript.enemy_DiscardCardList.Add(NagaHand[k]);
                                NagaHand.RemoveAt(k);
                                k = NagaHand.Count - 1;
                                print("DISCARD");
                               
                            }
                        }
                    }

                }
                i = 0;
            }

        }
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
                   

                    //Remove Card

                }
                else if (NagaHand[i].GetComponent<CardObj>().name == "Eldritch Oath")
                {
                    print("DISCARD:" + NagaHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                   
                    //Remove Card

                }
                else if (NagaHand[i].GetComponent<CardObj>().name == "Call of the deep")
                {
                    print("DISCARD:" + NagaHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                  
                    //Remove Card

                }
                else if (NagaHand[i].GetComponent<CardObj>().name == "Crushing Blow")
                {
                    print("DISCARD:" + NagaHand[i].GetComponent<CardObj>().name);
                    remainingCost--;
                  
                    //Remove Card

                }

              
            }

            i++;


        }

        NagaHand[index].GetComponent<CardMovement>().PlayEnemyCard();

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
