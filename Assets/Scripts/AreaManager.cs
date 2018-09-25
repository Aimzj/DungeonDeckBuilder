﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {

    public List<GameObject> cardList_Play;
    public List<GameObject> cardList_Discard;
    public List<GameObject> cardList_Trash;
    public List<GameObject> cardList_Deck;

    //possible target positions
    private Transform playerDeck, enemyDeck;
    private Transform playerDiscard, enemyDiscard;
    private Transform playerTrash, enemyTrash;

    public GameObject TempObj;

    //list of x positions for the cards in the play area
    public List<float> PlayAreaPositions;
    //distance between the cards in the player's hand.
    private float x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    private float newCardPos, multiplier;

    private int numCardsInPlay;

    private HandManager handManagerScript;

    // Use this for initialization
    void Start () {
        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();
        playerDiscard = GameObject.Find("Discard").GetComponent<Transform>();
        playerTrash = GameObject.Find("Trash").GetComponent<Transform>();

        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();

        numCardsInPlay = 0;

        x = 4;
        boundaryValue = 5;
    }

    public void DiscardCard(GameObject cardObj)
    {
        //add card to list of discards
        cardList_Discard.Add(cardObj);

        //card has now been played
        cardObj.GetComponent<CardMovement>().isPlayed = true;

        //move the card's position to discard pile
        var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDiscard.position.x, playerDiscard.position.y, playerDiscard.position.z), Quaternion.Euler(90, 0, 0));
        cardObj.GetComponent<CardMovement>()._targetTransform = obj.transform;

        //arrange cards in hand
        handManagerScript.SetCardPositionsInHand();
        handManagerScript.UpdateCardPositionsInHand();
    }

    public void TrashCard(GameObject cardObj)
    {
        //add card to list of trashed cards
        cardList_Trash.Add(cardObj);

        //card has now been played
        cardList_Trash[cardList_Trash.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        //move the card's position to trash pile
        var obj = (GameObject)Instantiate(TempObj, new Vector3(playerTrash.position.x, playerTrash.position.y, playerTrash.position.z), Quaternion.Euler(90, 0, 0));
        cardObj.GetComponent<CardMovement>()._targetTransform = obj.transform;

        //arrange cards in hand
        handManagerScript.SetCardPositionsInHand();
        handManagerScript.UpdateCardPositionsInHand();
    }

    public void PlayCard(GameObject cardObj)
    {
        numCardsInPlay++;
        PlayAreaPositions.Add(0f);
        newCardPos = 0;
        //check if number of cards is odd or even
        if (numCardsInPlay > 1)
        {
            multiplier = (numCardsInPlay - 1f) / 2;

            //check to see that the boundary value hasn't been crossed
            //if it has been crossed, make the distance between cards smaller
            newCardPos = multiplier * x;
            while (newCardPos > boundaryValue)
            {
                x -= 0.1f;
                newCardPos = multiplier * x;
            }

            //start on the right hand side
            //move from right to left
            for (int i = 0; i < numCardsInPlay; i++)
            {
                newCardPos = multiplier * x;
                PlayAreaPositions[i] = newCardPos;
                multiplier -= 1f;
            }
        }
        else
        {
            PlayAreaPositions[0] = 0;
        }

        //add the card to list
        cardList_Play.Add(cardObj);

        //card has now been played
        cardList_Play[cardList_Play.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        //assign positions to cards
        for (int i = 0; i < cardList_Play.Count; i++)
        {
            var obj = (GameObject)Instantiate(TempObj, new Vector3(PlayAreaPositions[i], 0f, 4f), Quaternion.Euler(90,0,0));

            cardList_Play[i].GetComponent<CardMovement>()._targetTransform = obj.transform;

        }

        handManagerScript.SetCardPositionsInHand();
        handManagerScript.UpdateCardPositionsInHand();
    }

    public void DiscardPlayArea()
    {
        //check if there are cards present in the play area
        if (numCardsInPlay > 0)
        {
            //loop through play area list and add to discard list
            for (int i = 0; i < cardList_Play.Count; i++)
            {
                cardList_Discard.Add(cardList_Play[i]);

                //change target position of each card to the discard pile
                var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDiscard.position.x, playerDiscard.position.y, playerDiscard.position.z), Quaternion.Euler(90, 0, 0));
                cardList_Play[i].GetComponent<CardMovement>()._targetTransform = obj.transform;
            }
            //clear play list
            cardList_Play.Clear();
            numCardsInPlay = 0;
        }
    }

    public void RenewDeck()
    {
        //check if there are cards present in the discard pile
        if (cardList_Discard.Count > 0)
        {
            //loop through play area list and add to deck list
            for (int i = 0; i < cardList_Discard.Count; i++)
            {
                cardList_Deck.Add(cardList_Discard[i]);

                //change target position of each card to the deck
                var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDeck.position.x, playerDeck.position.y, playerDeck.position.z), Quaternion.Euler(90, 0, 0));
                cardList_Discard[i].GetComponent<CardMovement>()._targetTransform = obj.transform;
            }
            //clear play list
            cardList_Discard.Clear();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
