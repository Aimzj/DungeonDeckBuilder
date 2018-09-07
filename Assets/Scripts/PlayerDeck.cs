using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour {
    public List<GameObject> MainDeck;
    public List<GameObject> DiscardPile;
    public List<GameObject> CardsInHand;
    public List<GameObject> BurntCards;

    public bool isPlayer;

    private int deckMainSize, deckMainRemaining;
    private int discardSize;
    private int handSize;
    private int numBurnt;

    private 

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
        MainDeck.Add(temp);

        deckMainSize++;
        deckMainRemaining++;
    }

    //drawing the top card off of the player's deck
    private GameObject DrawCard_MainDeck_Top()
    {
        GameObject topCard = MainDeck[deckMainRemaining - 1];
        MainDeck[deckMainRemaining - 1] = null;
        deckMainRemaining--;
        return topCard;
    }

    private GameObject DrawCard_MainDeck_Random()
    {
        int Rand = Random.Range(0, deckMainRemaining);
        GameObject randomCard = MainDeck[Rand];

        //cycle through remaining deck and move each card above the chosen card down one
        for (int i = Rand; i<deckMainRemaining-1; i++)
        {
            MainDeck[i] = MainDeck[i + 1];
        }
        MainDeck[deckMainRemaining - 1] = null;
        deckMainRemaining--;

        return randomCard;
    }

    //Picking up a card from the deck
    private void AddCard_ToHand()
    {
        //drawing card from the top of the deck
        GameObject cardObj = DrawCard_MainDeck_Top();

        //move card onto board and make visible
        cardObj.SetActive(true);
        
        //instantiate card on table

        //add to cards in hand
        CardsInHand.Add(cardObj);
        handSize++;
        
    }

    private GameObject RemoveCard_FromHand(int pos)
    {
        //cards in hand are numbered 0 to x from left to right
        GameObject cardObj = CardsInHand[pos];

        //move cards to the right of the chosen card left once
        for (int i = pos; i < handSize - 1; i++)
        {
            CardsInHand[i] = CardsInHand[i + 1];
        }
        CardsInHand[handSize - 1] = null;
        handSize--;

        return cardObj;
    }

    //discard a card from the hand
    private void DiscardCard(int posInHand)
    {
        //remove card from hand
        GameObject cardObj = RemoveCard_FromHand(posInHand);

        //add to discard pile
        DiscardPile.Add(cardObj);
        discardSize++;

    }

    //play a card from the hand
    //check for all card stats and abilities
    private void PlayCard(int posInHand)
    {
        //remove card from hand
        GameObject cardObj = RemoveCard_FromHand(posInHand);

        //retrieve card data
        CardObj Card = cardObj.GetComponent<CardObj>();

        //analyse card data

        //ATTACK
        if (Card.Attack > 0)
        {
            
        }

        //DEFEND
        if (Card.Defense > 0)
        {

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
