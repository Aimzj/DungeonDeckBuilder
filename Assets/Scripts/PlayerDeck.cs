using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour {
    public List<GameObject> PlayerMainDeck;
    public List<GameObject> PlayerDiscardPile;
    public List<GameObject> CardsInHand;
    public List<GameObject> BurntCards;

    private int deckMainSize, deckMainRemaining;
    private int discardSize;
    private int handSize;
    private int numBurnt;

    private enum cardType { main,discard,hand,burnt,poisons,wounds,enemy}
	// Use this for initialization
	void Start () {
        deckMainSize = 0;
        deckMainRemaining = 0;

        discardSize = 0;
        handSize = 0;
        numBurnt = 0;
	}

    //putting a new card into the player's deck
    public void IncreaseSize_MainDeck(GameObject newCard)
    {
        GameObject temp = (GameObject)Instantiate(newCard, Vector3.zero, Quaternion.identity);
        temp.SetActive(false);
        PlayerMainDeck.Add(temp);

        deckMainSize++;
        deckMainRemaining++;
    }

    //drawing the top card off of the player's deck
    private GameObject DrawCard_MainDeck_Top()
    {
        GameObject topCard = PlayerMainDeck[deckMainRemaining - 1];
        deckMainRemaining--;
        return topCard;
    }

    //moving a card into the player's hand
    private void AddCard_Hand(GameObject cardObj, string fromWhere)
    {
        //move card onto board and make visible

        cardObj.SetActive(true);
        CardsInHand.Add(cardObj);

        handSize++;

        if (fromWhere == cardType.main.ToString())
        {
            deckMainRemaining--;
        }
        else if (fromWhere == cardType.discard.ToString())
        {
            discardSize--;
        }
        else if(fromWhere == cardType.burnt.ToString())
        {
            numBurnt--;
        }
        else if(fromWhere == cardType.wounds.ToString())
        {

        }
        else if(fromWhere == cardType.poisons.ToString())
        {

        }else if (fromWhere == cardType.enemy.ToString())
        {

        }
        
    }

    //discard a player's card
    private void DiscardCard(GameObject card, string fromWhere)
    {

    }

    //play a card from a player's hand
    //check for all card stats and abilities
    private void PlayCard()
    {

    }

    //deal damage to opponent
    private void AttackOpponent()
    {

    }
    
	
	// Update is called once per frame
	void Update () {
		
	}
}
