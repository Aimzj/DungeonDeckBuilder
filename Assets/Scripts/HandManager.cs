using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandManager : MonoBehaviour {
    //possible target positions
    private Transform playerDeck, enemyDeck;
    private Transform playerDiscard, enemyDiscard;
    private Transform playerTrash, enemyTrash;

    public GameObject CardObj;
    public GameObject TempObj;

    public List<GameObject> playerCardList;

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

    //sounds
    private SoundManager soundScript;

    //hand size
    private int maxHandSize;

    public bool isExceedingHandSize;
    private TextMeshProUGUI limitExceeded_Text;
    private SpriteRenderer limit_halo;

    private CardGenerator cardGenScript;
    private AreaManager areaManagerScript;

    // Use this for initialization
    void Start () {
        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();
        playerDiscard = GameObject.Find("Discard").GetComponent<Transform>();
        playerTrash = GameObject.Find("Trash").GetComponent<Transform>();

        soundScript = GameObject.Find("SoundMaker").GetComponent<SoundManager>();
        cardGenScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();

        limitExceeded_Text = GameObject.Find("HandLimitNotice").GetComponent<TextMeshProUGUI>();
        limitExceeded_Text.enabled = false;
        limit_halo = GameObject.Find("HandLimit_Halo").GetComponent<SpriteRenderer>();
        limit_halo.enabled = false;

        numCardsInHand = 0;

        x = 4;
        boundaryValue = 5;

        isHoldingCard = false;

        maxHandSize = 10;
    }

    public void SpawnCards()
    {
        //looping through the card list, adding to local list and instantiating into scene
        for(int i=0; i< cardGenScript.PlayerDeck.Count; i++)
        {
            var temp = (GameObject)Instantiate(CardObj, playerDeck.position, Quaternion.Euler(90, 0, 0));
            playerCardList.Add(temp);
        }
    }

    public void RemoveCardFromHand(int pos)
    {
        numCardsInHand--;
        HandPositions.RemoveAt(pos);
        playerCardList.RemoveAt(pos);

        //loop through all cards after the removed card and change their pos value
        //check if the card removed was the last card
        if(pos != playerCardList.Count)
        {
            for (int i = pos; i < playerCardList.Count; i++)
            {
                playerCardList[i].GetComponent<CardMovement>().posInHand = i;
            }
        }

        //check if card hand size is below the limit
        if (numCardsInHand < maxHandSize)
        {
            //hide message informing player that they need to discard down to 10 cards before they can continue play
            limitExceeded_Text.enabled = false;
            limit_halo.enabled = false;
            isExceedingHandSize = false;
        }

    }

    public void AddCardToHand()
    {
        numCardsInHand++;
        HandPositions.Add(0f);
        newCardPos = 0;

        SetCardPositionsInHand();
    }

    public IEnumerator DrawCards(int numCards)
    {
        
        for(int i =0; i<numCards; i++)
        {
            //check if there are enough cards left in deck
            if (playerCardList.Count - numCards >= 0)
            {
                //play sound
                soundScript.PlaySound_DrawCard();

                //add a card to the hand
                AddCardToHand();

                //instantiate card in deck and add to list
                // var temp = (GameObject)Instantiate(CardObj, playerDeck.position, Quaternion.Euler(90,0,0));
                // playerCardList.Add(temp);

                //card is now in the player's hand
                playerCardList[playerCardList.Count - 1].GetComponent<CardMovement>().isInHand = true;
                //set the position(index) of the card in the hand
                playerCardList[playerCardList.Count - 1].GetComponent<CardMovement>().posInHand = playerCardList.Count - 1;

                UpdateCardPositionsInHand();

                yield return new WaitForSecondsRealtime(0.3f);
            }
            else
            {
                //Need to shuffle the discard pile and reform the playerdeck
                Shuffle(ref areaManagerScript.cardList_Discard);

                //add shuffled discard pile to playerDeck
                for(int j= 0; j< areaManagerScript.cardList_Discard.Count; j++)
                {
                    playerCardList.Add(areaManagerScript.cardList_Discard[j]);
                }
                areaManagerScript.cardList_Discard.Clear();
            }

        }

        //check if card hand size is exceeded
        if (numCardsInHand > maxHandSize)
        {
            //display message informing player that they need to discard down to 10 cards before they can continue play
            //another check in the Discard Card function
            limitExceeded_Text.enabled = true;
            limit_halo.enabled = true;
            isExceedingHandSize = true;
        }
    }

    //shuffling cards
    public void Shuffle(ref List<GameObject> cardList)
    {
        int len = cardList.Count;
        Random rand = new Random();

        for(int i=0; i<len; i++)
        {
            Swap(ref cardList, i, i + Random.Range(0,len - i));
        }
    }

    //swapping two positions in List
    private void Swap(ref List<GameObject> cards, int pos1, int pos2)
    {
        GameObject temp = cards[pos1];
        cards[pos1] = cards[pos2];
        cards[pos2] = temp;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(DrawCards(1));
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
        for (int i = 0; i < playerCardList.Count; i++)
        {
            CardMovement cardScript = playerCardList[i].GetComponent<CardMovement>();

            var obj = (GameObject)Instantiate(TempObj, new Vector3(HandPositions[i], 0f, -0.83f), Quaternion.Euler(90,0,0));

            playerCardList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;

        }
    }
}
