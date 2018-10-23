using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {

    private List<GameObject>  enemyHand;

	// Use this for initialization
	public void TurnStart()
    {

        for(int i = 0 ; i <= enemyHand.Count ; i++)
        {
            if(enemyHand[i].GetComponent<CardObj>().CardName == "Skitter")
            {
                //Play card
            }
            if(enemyHand[i].GetComponent<CardObj>().CardName == "Bite")
            {
                //Play card
            }
        }
    }
	public void Draw(int drawNum)
    {
        //Draws Cards
    }
	
}
