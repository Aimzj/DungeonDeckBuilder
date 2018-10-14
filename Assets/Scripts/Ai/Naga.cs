using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naga : MonoBehaviour {

    private List<GameObject> enemyHand;
    private bool isReactive = false; // Get from master scriopt


    //Card Counting
    private int numEldritchOath = 0;
    private int numCrushBlow = 0;
    private int numScale = 0;
    private int numCotD = 0;

    //Stat tracker
    private int hp = 10;
    private int sigilCount = 4;
    private int handSize = 4;


    private void Start()
    {
        cardCount();
    }

    public void TurnStart()//Start Turn
    {

        Draw(2);
        if (isReactive)//Reactive Functions
        {
            Reaction();
        }

        else
        {
            Action();//Action functions
        }
      
    }

    protected void cardCount()//Count cards on start
    {
        for(int i = 0; i <= enemyHand.Count; i++)
        {
            if(enemyHand[i].GetComponent<CardObj>().Card_Name == "Eldritch Oath")
            {
                numEldritchOath++;
            }
            else if (enemyHand[i].GetComponent<CardObj>().Card_Name == "Crushing Blow")
            {
                numCrushBlow++;
            }
            else if (enemyHand[i].GetComponent<CardObj>().Card_Name == "Serpent's Scale")
            {
                numScale++;
            }
            else if (enemyHand[i].GetComponent<CardObj>().Card_Name == "Call of the Deep")
            {
                numCotD++;
            }
        }
    }

    protected void Reaction()
    {
        for (int i = 0; i <= enemyHand.Count; i++)
        {
         
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
}
