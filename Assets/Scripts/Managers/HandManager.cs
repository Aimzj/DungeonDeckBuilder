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

    //only contains card in the deck (NOT in play, hand, discard or trash)
    public List<GameObject> playerDeckList, enemyDeckList;
    //only contains cards in the hand
    public List<GameObject> playerHandList = new List<GameObject>();
    public List<GameObject> enemyHandlist = new List<GameObject>();

    //list of x positions for the cards in the player/enemy hand
    public List<float> Player_HandPositions, Enemy_HandPositions;
    //distance between the cards in the player/enemy hand.
    private float player_x, enemy_x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    //public int numCardsInPlayerHand, numCardsInEnemyHand;
    //replace by the length of the handList


    private float newCardPos, multiplier;

    //is the player holding a card
    public bool isPlayerHoldingCard, isEnemyHoldingCard;

    //sounds
    private SoundManager soundScript;

    //hand size
    private int maxHandSize;

    //y positions of cards in the hands
    private float player_yPos, enemy_yPos;

    public bool isExceedingHandSize;
    private TextMeshProUGUI limitExceeded_Text;
    private SpriteRenderer limit_halo;

    private CardGenerator cardGenScript;
    private AreaManager areaManagerScript;
    private StatsManager statManagerScript;

    private Spider spiderScript;

    // Use this for initialization
    void Start () {
        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();
        playerDiscard = GameObject.Find("Discard").GetComponent<Transform>();
        playerTrash = GameObject.Find("Trash").GetComponent<Transform>();
        enemyDeck = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        enemyDiscard = GameObject.Find("EnemyDiscard").GetComponent<Transform>();
        enemyTrash = GameObject.Find("EnemyTrashPile").GetComponent<Transform>();

        soundScript = GameObject.Find("SoundMaker").GetComponent<SoundManager>();
        cardGenScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();

        spiderScript = GameObject.Find("GameManager").GetComponent<Spider>();

        limitExceeded_Text = GameObject.Find("HandLimitNotice").GetComponent<TextMeshProUGUI>();
        limitExceeded_Text.enabled = false;
        limit_halo = GameObject.Find("HandLimit_Halo").GetComponent<SpriteRenderer>();
        limit_halo.enabled = false;

      //  numCardsInPlayerHand = 0;
       // numCardsInEnemyHand = 0;

        player_x = 4;
        enemy_x = 4;

        player_yPos = -0.83f;
        enemy_yPos = 10f;

        boundaryValue = 5;

        isPlayerHoldingCard = false;
        isEnemyHoldingCard = false;

        maxHandSize = 10;
    }

    //called at the start of the game to initialise cards
    //the enemyDeckList changes according to the level - spider or level 1 and Naga for level 2
    public void InitialiseCards(int level)
    {
        playerDeckList = cardGenScript.PlayerDeck;
        if (level == 1)
        {
            enemyDeckList = cardGenScript.SpiderDeck;
        } else if (level == 2)
        {
            enemyDeckList = cardGenScript.NagaDeck;
        }

        //shuffle the card lists
        Shuffle(ref playerDeckList);
        Shuffle(ref enemyDeckList);
    }

    public void Call_RemoveCardFromHand(int pos, string target)
    {
        if (target == "player")
        {
            RemoveCardFromHand(pos, ref Player_HandPositions, ref playerDeckList, ref player_x, player_yPos, ref playerHandList);
        }
        else if(target == "enemy")
        {
            RemoveCardFromHand(pos, ref Enemy_HandPositions, ref enemyDeckList, ref enemy_x, enemy_yPos, ref enemyHandlist);
        }
    }

    private void RemoveCardFromHand(int pos, ref List<float> handPositions, ref List<GameObject> deckList, ref float x, float yPos, ref List<GameObject> handList)
    {
        handList.RemoveAt(pos);
        handPositions.RemoveAt(pos);
        //deckList.RemoveAt(pos);

        //loop through all cards after the removed card and change their pos value
        UpdatePosInHand(ref handList);

        //check if card hand size is equal to or below the limit
        if (handList.Count <= maxHandSize)
        {
            //hide message informing player that they need to discard down to 10 cards before they can continue play
            limitExceeded_Text.enabled = false;
            limit_halo.enabled = false;
            isExceedingHandSize = false;
        }

        SetCardPositionsInHand(ref x, ref handPositions,ref handList);
        UpdateCardPositionsInHand(ref handPositions, yPos, ref handList);

    }

    //loop through all cards after the removed card and change their pos value
    private void UpdatePosInHand(ref List<GameObject> handList)
    {
        for (int i = 0; i < handList.Count; i++)
        {
            handList[i].GetComponent<CardMovement>().posInHand = i;
        }
    }

    public IEnumerator DrawCards(int numCards, string target)
    {
        for(int i = 0; i < numCards; i++)
        {
            if (target == "player")
            {
                if(playerDeckList.Count - numCards >= 0)
                {
                    DrawCards("player", ref playerDeckList, ref Player_HandPositions, ref player_x, player_yPos, ref playerHandList);
                }
                else
                {
                    RemakeDeck("player",ref areaManagerScript.player_DiscardCardList,ref playerDeckList, playerDeck);
                    yield return new WaitForSecondsRealtime(0.3f);
                    i--;
                }
            }
            else if (target == "enemy")
            {
                print("draw enemy card!");
                if (enemyDeckList.Count - numCards >= 0)
                {
                    DrawCards("enemy", ref enemyDeckList, ref Enemy_HandPositions, ref enemy_x, enemy_yPos, ref enemyHandlist);
                }
                else
                {
                    RemakeDeck("enemy", ref areaManagerScript.enemy_DiscardCardList, ref enemyDeckList, enemyDeck);
                    yield return new WaitForSecondsRealtime(0.3f);
                    i--;
                }
            }

            yield return new WaitForSecondsRealtime(0.3f);
        }

    }

    private void DrawCards(string target, ref List<GameObject> deckList, ref List<float> handPositions, ref float x, float yPos, ref List<GameObject> handList)
    {
        //play sound
        soundScript.PlaySound_DrawCard();

        statManagerScript.UpdateCardsInDeck(target, -1);

        //playerHandList.Add(playerDeckList[numCardsInHand - 1]);
        if (target == "player")
        {
            print("number of cards in hand before drawing: " + handList.Count);
          //  print("position in hand before moving into player hand: " + deckList[numCardsInHand - 1].GetComponent<CardMovement>().posInHand);
        }
            
        
        //add the card in the first position of the deck list to the hand list
        handList.Add(deckList[0]);
        //update positions in hand
        UpdatePosInHand(ref handList);
        //the card is now in the hand
        handList[handList.Count-1].GetComponent<CardMovement>().isInHand = true;
        //remove the card in the decklist that was just added to the hand list
        deckList.RemoveAt(0);

        //set the position(index) of the card in the hand
       // handList[handList.Count - 1].GetComponent<CardMovement>().posInHand = handList.Count-1;

        //ADD card to hand (visually)
        handPositions.Add(0f);
        newCardPos = 0;
        SetCardPositionsInHand(ref x, ref handPositions, ref handList);
        UpdateCardPositionsInHand(ref handPositions, yPos, ref handList);

        //change the card's layering
        SetLayeringInHand(ref deckList);

        //ON ARRIVAL
        if(target == "enemy")
        {
            if (handList[handList.Count - 1].GetComponent<CardObj>().CardName == "Lethargy")
            {
                print("LETHARGY");
                spiderScript.Lethargy();

                handList[handList.Count - 1].GetComponent<CardMovement>().PlayEnemyCard();

            }
        }
        


        //check if card hand size is exceeded
        if (handList.Count > maxHandSize)
        {
            //display message informing player that they need to discard down to 10 cards before they can continue play
            //another check in the Discard Card function
            limitExceeded_Text.enabled = true;
            limit_halo.enabled = true;
            isExceedingHandSize = true;
        }
    }

    //shuffles the dsicard pile and adds cards back into the deck
    private void RemakeDeck(string target, ref List<GameObject> discardList, ref List<GameObject> deckList, Transform deckPos )
    {
        //gain a cycle token
        statManagerScript.UpdateCycleTokens(target, 1);

        //Need to shuffle the discard pile and reform the playerdeck
        Shuffle(ref discardList);

        int count = deckList.Count;
        //add shuffled discard pile to playerDeck
        for (int j = 0; j < discardList.Count; j++)
        {
            deckList.Add(discardList[j]);

            //change target position of each card to the player's deck
            var obj = (GameObject)Instantiate(TempObj, new Vector3(deckPos.position.x, deckPos.position.y, deckPos.position.z), Quaternion.Euler(90, 0, 0));
            deckList[count].GetComponent<CardMovement>()._targetTransform = obj.transform;
            deckList[count].GetComponent<CardMovement>().isInHand = false;
            deckList[count].GetComponent<CardMovement>().isPlayed = false;
            count++;
        }

        statManagerScript.SetTotalCards(target, deckList.Count,0);
        discardList.Clear();

    }

    //loop through all cards and assign Layers
    private void SetLayeringInHand(ref List<GameObject> deckList)
    {
        int count = 0;
        for(int i=0; i< deckList.Count; i++)
        {
            if (deckList[i].GetComponent<CardMovement>().isInHand)
            {
                deckList[i].GetComponent<SpriteRenderer>().sortingOrder = 15 + count*2;
                TextMeshPro tMP = deckList[i].transform.Find("Title").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count*2;
                tMP = deckList[i].transform.Find("DiscardCost").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = deckList[i].transform.Find("BurnCost").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = deckList[i].transform.Find("DiscardEffect").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = deckList[i].transform.Find("BurnEffect").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = deckList[i].transform.Find("AttackCost").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = deckList[i].transform.Find("DefenseCost").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;

                count++;
            }
            
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
        if (Input.GetKeyDown(KeyCode.K))
        {
         //   StartCoroutine(Call_TakeDamage(5, "player"));
        }
    }

    /*public IEnumerator Call_TakeDamage(int value, string target)
    {
        int chosenIndex;
        List<GameObject> tempDeckPileList;
        List<GameObject> tempHandList;
        List<GameObject> tempDiscardList;
        if (target == "player")
        {
            tempDeckPileList = new List<GameObject>(playerDeckList);
            tempHandList = new List<GameObject>(playerHandList);
            tempDiscardList = new List<GameObject>(areaManagerScript.player_DiscardCardList);
            // InitialiseDeck(ref tempDeckPileList,ref tempHandList,ref tempDiscardList, playerDeckList, areaManagerScript.player_DiscardCardList);
        }
        else if(target == "enemy")
        {
            tempDeckPileList = new List<GameObject>(playerDeckList);
            tempHandList = new List<GameObject>(playerHandList);
            tempDiscardList = new List<GameObject>(areaManagerScript.player_DiscardCardList);
            // InitialiseDeck(ref tempDeckPileList, ref tempHandList, ref tempDiscardList, enemyDeckList, areaManagerScript.enemy_DiscardCardList);
        }
        

        //loop through deck and randomly burn cards
        for (int i = 0; i < value; i++)
        {
            //play burn sound 

            //check that there are cards in the deck to burn
            if (tempDeckPileList.Count > 0)
            {
                chosenIndex = Random.Range(0, tempDeckPileList.Count);
                print("index: " + chosenIndex);
                if (target == "player")
                    areaManagerScript.Call_TakeDamage(tempDeckPileList[chosenIndex], "player");
                else if(target == "enemy")
                    areaManagerScript.Call_TakeDamage(tempDeckPileList[chosenIndex], "enemy");
                tempDeckPileList.RemoveAt(chosenIndex);
            }
            else if (tempDiscardList.Count > 0)
            {
                //check if there are cards in the discard pile
                //reshuffle the discard pile
                if (target == "player")
                {
                    RemakeDeck(target, ref areaManagerScript.player_DiscardCardList, ref playerDeckList, playerDeck);
                    yield return new WaitForSecondsRealtime(1f);
                    InitialiseDeck(ref tempDeckPileList, ref tempHandList, ref tempDiscardList, playerDeckList, areaManagerScript.player_DiscardCardList);
                    yield return new WaitForSecondsRealtime(0.5f);
                    i--;
                }
                else if (target == "enemy")
                {
                    RemakeDeck(target, ref areaManagerScript.enemy_DiscardCardList, ref enemyDeckList, enemyDeck);
                    yield return new WaitForSecondsRealtime(1f);
                    InitialiseDeck(ref tempDeckPileList, ref tempHandList, ref tempDiscardList, enemyDeckList, areaManagerScript.enemy_DiscardCardList);
                    yield return new WaitForSecondsRealtime(0.5f);
                    i--;
                }

            }
            else if (tempHandList.Count > 0)
            {
                
                print("num cards in deck: " + tempDeckPileList.Count + "!!");
                //check if there are cards in the player's hand
                //burn them
                chosenIndex = Random.Range(0, tempHandList.Count);
                print("index: " + chosenIndex);
                areaManagerScript.Call_TakeDamage(tempHandList[chosenIndex], target);
                tempHandList.RemoveAt(chosenIndex);

                Call_SetPositionsInHand(target);
                Call_UpdateCardPositionsInHand(target);
            }
            
            yield return new WaitForSecondsRealtime(1.1f);

        }

    }*/

    private void InitialiseDeck(ref List<GameObject> tempDeckList,ref List<GameObject> tempHandList, ref List<GameObject> tempDiscardList,  List<GameObject> deckList, List<GameObject> discardList)
    {
        //create a new list that contains only the cards in the deck pile
        //create a new list that contains only cards in the player's hand
        //loop through all cards
        for (int i = 0; i < deckList.Count; i++)
        {
            //if the card is not being held and has not been played (in the deck pile)
            if (!deckList[i].GetComponent<CardMovement>().isInHand
                && !deckList[i].GetComponent<CardMovement>().isPlayed)
            {
                tempDeckList.Add(deckList[i]);
            }

            //if the card is being held and has not been played
            if (deckList[i].GetComponent<CardMovement>().isInHand
                && !deckList[i].GetComponent<CardMovement>().isPlayed)
            {
                tempHandList.Add(deckList[i]);
            }

            tempDiscardList = discardList;
        }

    }

    private void TakeDamage(int value)
    {
        
    }

        //called by the area manager
    public void Call_SetPositionsInHand(string target)
    {
        if (target == "player")
        {
            SetCardPositionsInHand(ref player_x, ref Player_HandPositions, ref playerHandList);
        }
        else if (target == "enemy")
        {
            print("enemy numCards in hand: " + enemyHandlist.Count);
            SetCardPositionsInHand(ref enemy_x, ref Enemy_HandPositions, ref enemyHandlist);
        }
    }

    private void SetCardPositionsInHand(ref float x, ref List<float> handPositions, ref List<GameObject> handList)
    {
        //check if number of cards is odd or even
        if (handList.Count > 1)
        {
            multiplier = (handList.Count - 1f) / 2;

            //check to see that the boundary value hasn't been crossed
            //if it has been crossed, make the distance between cards smaller
            x = 4;
            newCardPos = multiplier * x;
            while (newCardPos > boundaryValue)
            {
                x -= 0.1f;
                newCardPos = multiplier * x;
            }

            //start on the right hand side
            //move from right to left
            for (int i = 0; i < handList.Count; i++)
            {
                newCardPos = multiplier * x;
                handPositions[i] = newCardPos;
                multiplier -= 1f;
            }
        }
        else
        {
            if(handList.Count != 0)
                handPositions[0] = 0;
        }
    }

    //called by the area manager
    public void Call_UpdateCardPositionsInHand(string target)
    {
        if (target == "player")
        {
            UpdateCardPositionsInHand(ref Player_HandPositions, player_yPos, ref playerHandList);
        }
        else if (target == "enemy")
        {
            UpdateCardPositionsInHand(ref Enemy_HandPositions, enemy_yPos, ref enemyHandlist);
        }
    }

    private void UpdateCardPositionsInHand(ref List<float> handPositions, float yPos, ref List<GameObject> handList)
    {
        //assign positions to cards
        for (int i = 0; i < handList.Count; i++)
        {
            CardMovement cardScript = handList[i].GetComponent<CardMovement>();

            var obj = (GameObject)Instantiate(TempObj, new Vector3(handPositions[i], 0f, yPos), Quaternion.Euler(90,0,0));

            handList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;

        }
    }
}
