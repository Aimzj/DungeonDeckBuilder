using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    private Transform playerDeck, enemyDeck;
    private Transform playerDiscard, enemyDiscard;
    private Transform playerTrash, enemyTrash;

    public GameObject CardObj;

    public List<GameObject> cardList;

    //list of x positions for the cards in the player's hand
    public List<float> HandPositions;
    //distance between the cards in the player's hand.
    private float x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    private int numCardsInHand;

    // Use this for initialization
    void Start () {
        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();

        numCardsInHand = 0;

        x = 3;
        boundaryValue = 6;
    }

    public void AddCardToHand()
    {
        //  Instantiate(cardObj, PlayerDeck.transform.position, Quaternion.identity);
        numCardsInHand++;
        HandPositions.Add(0f);
        float newCardPos = 0;
        //check if number of cards is odd or even
        if (numCardsInHand > 1)
        {
            //number is even
            float multiplier = (numCardsInHand - 1) / 2;
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
                /* if (i< (numCardsInHand / 2) - 1)
                 {
                     //right hand side
                     multiplier -= 1
                     HandPositions[i] = newCardPos;
                 }
                 else
                 {
                     //left hand side
                     HandPositions[i] = -newCardPos;
                 }*/

            }
        }
        else
        {
            HandPositions[0] = 0;
        }
        /*else
        {
            //number is odd
            if (numCardsInHand > 1)
            {
                float multiplier = (numCardsInHand - 1)/2;

                newCardPos = multiplier * x;
                while (newCardPos > boundaryValue)
                {
                    x -= 0.1f;
                    newCardPos = multiplier * x;
                }
            }
            else
            {
                //only one card in hand
                HandPositions[0] = 0;
            }
        }*/
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //add a card to the hand
            AddCardToHand();

            //instantiate card in deck and add to list
            var temp = (GameObject)Instantiate(CardObj, Vector3.zero, Quaternion.identity);
            cardList.Add(temp);

            Debug.Log("num cards " + numCardsInHand.ToString());
            //assign positions to cards
            for (int i = 0; i < cardList.Count; i++)
            {
                Debug.Log(HandPositions[i]);
                CardMovement cardScript = cardList[i].GetComponent<CardMovement>();
               // cardScript._targetTransform = null;
                cardList[i].GetComponent<CardMovement>()._targetTransform.position = new Vector3(HandPositions[i], 0f, -0.83f);
                Debug.Log("lol");
            }
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            //move all cards to the deck
            for(int i =0; i < cardList.Count; i++)
            {
                cardList[i].GetComponent<CardMovement>()._targetTransform = playerDeck;
            }
        }
    }
}
