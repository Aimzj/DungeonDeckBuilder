using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaManager : MonoBehaviour {

    public List<GameObject> cardList;

    public GameObject TempObj;

    //list of x positions for the cards in the plaoy area
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
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();

        numCardsInPlay = 0;

        x = 3;
        boundaryValue = 6;
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
        cardList.Add(cardObj);

        //card is now in the playArea
        cardList[cardList.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        //assign positions to cards
        for (int i = 0; i < cardList.Count; i++)
        {
            CardMovement cardScript = cardList[i].GetComponent<CardMovement>();

            var obj = (GameObject)Instantiate(TempObj, new Vector3(PlayAreaPositions[i], 0f, 5f), Quaternion.identity);

            cardList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;

        }

        handManagerScript.SetCardPositionsInHand();
        handManagerScript.UpdateCardPositionsInHand();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
