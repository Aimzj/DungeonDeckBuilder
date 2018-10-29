using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectManager : MonoBehaviour {
    private StatsManager statManagerScript;

	// Use this for initialization
	void Start () {
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();

	}

    //play a card from the hand
    //check for all card stats and abilities
    public void PlayCard(GameObject playedCard, bool isBurn)
    {
        //retrieve card data
        CardObj Card = playedCard.GetComponent<CardObj>();

        //take off of the discard pool
        statManagerScript.UpdateDiscard("player", -Card.DiscardCost);

        if (isBurn)
        {
            //take off of the burn pool
            statManagerScript.UpdateBurn("player", -Card.BurnCost);

            //give the card a burn halo whilst in play area
            playedCard.transform.Find("BurnBorder").GetComponent<SpriteRenderer>().enabled = true;
        }

        //check opponent

        //analyse card data

        //check if action or reaction phase

        //ATTACK
        if (Card.Attack > 0)
        {
            statManagerScript.UpdateAttack("player", Card.Attack);
        }

        //DEFEND
        if (Card.Defense > 0)
        {
            statManagerScript.UpdateDefense("enemy", Card.Defense);
        }
    }

    //deal damage to opponent
    private void AttackOpponent()
    {

    }
    
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
