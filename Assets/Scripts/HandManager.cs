﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    //possible target positions
    private Transform playerDeck, enemyDeck;
    private Transform playerDiscard, enemyDiscard;
    private Transform playerTrash, enemyTrash;

    public GameObject CardObj;
    public GameObject TempObj;

    public List<GameObject> cardList;

    //list of x positions for the cards in the player's hand
    public List<float> HandPositions;
    //distance between the cards in the player's hand.
    private float x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    private int numCardsInHand;

    private float newCardPos, multiplier;

    //is the player holding a card
    public bool isHoldingCard;

    // Use this for initialization
    void Start () {
        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();
        playerDiscard = GameObject.Find("Discard").GetComponent<Transform>();
        playerTrash = GameObject.Find("Trash").GetComponent<Transform>();

        numCardsInHand = 0;

        x = 3;
        boundaryValue = 6;

        isHoldingCard = false;
    }

    public void RemoveCardFromHand(int pos)
    {
        numCardsInHand--;
        HandPositions.RemoveAt(pos);
        cardList.RemoveAt(pos);

        //loop through all cards after the removed card and change their pos value
        //check if the card removed was the last card
        if(pos != cardList.Count)
        {
            for (int i = pos; i < cardList.Count; i++)
            {
                cardList[i].GetComponent<CardMovement>().posInHand = i;
            }
        }
        
    }

    public void AddCardToHand()
    {
        numCardsInHand++;
        HandPositions.Add(0f);
        newCardPos = 0;

        SetCardPositionsInHand();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //add a card to the hand
            AddCardToHand();

            //instantiate card in deck and add to list
            var temp = (GameObject)Instantiate(CardObj, playerDeck.position, Quaternion.identity);
            cardList.Add(temp);

            //card is now in the player's hand
            cardList[cardList.Count - 1].GetComponent<CardMovement>().isInHand = true;
            //set the position(index) of the card in the hand
            cardList[cardList.Count - 1].GetComponent<CardMovement>().posInHand = cardList.Count - 1;

            UpdateCardPositionsInHand();
        }

    }

    public void SetCardPositionsInHand()
    {
        //check if number of cards is odd or even
        if (numCardsInHand > 1)
        {
            multiplier = (numCardsInHand - 1f) / 2;

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
            for (int i = 0; i < numCardsInHand; i++)
            {
                newCardPos = multiplier * x;
                HandPositions[i] = newCardPos;
                multiplier -= 1f;
            }
        }
        else
        {
            if(numCardsInHand != 0)
                HandPositions[0] = 0;
        }
    }

    public void UpdateCardPositionsInHand()
    {
        //assign positions to cards
        for (int i = 0; i < cardList.Count; i++)
        {
            CardMovement cardScript = cardList[i].GetComponent<CardMovement>();

            var obj = (GameObject)Instantiate(TempObj, new Vector3(HandPositions[i], 0f, -0.83f), Quaternion.identity);

            cardList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;

        }
    }
}
