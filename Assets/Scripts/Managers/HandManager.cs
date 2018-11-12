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

    public List<GameObject> playerDeckList, enemyDeckList;

    //list of x positions for the cards in the player/enemy hand
    public List<float> Player_HandPositions, Enemy_HandPositions;
    //distance between the cards in the player/enemy hand.
    private float player_x, enemy_x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    public int numCardsInPlayerHand, numCardsInEnemyHand;

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

        numCardsInPlayerHand = 0;
        numCardsInEnemyHand = 0;

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
        if (level < 2)
        {
            playerDeckList = cardGenScript.PlayerDeck;
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
            RemoveCardFromHand(pos, ref numCardsInPlayerHand, ref Player_HandPositions, ref playerDeckList, ref player_x, player_yPos);
        }
        else if(target == "enemy")
        {
            RemoveCardFromHand(pos, ref numCardsInEnemyHand, ref Enemy_HandPositions, ref enemyDeckList, ref enemy_x, enemy_yPos);
        }
    }

    private void RemoveCardFromHand(int pos, ref int numCardsInHand, ref List<float> handPositions, ref List<GameObject> deckList, ref float x, float yPos)
    {
        numCardsInHand--;
        handPositions.RemoveAt(pos);
        deckList.RemoveAt(pos);

        //loop through all cards after the removed card and change their pos value
        //check if the card removed was the last card
        if(pos != deckList.Count)
        {
            for (int i = pos; i < deckList.Count; i++)
            {
                deckList[i].GetComponent<CardMovement>().posInHand = i;
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

        SetCardPositionsInHand(numCardsInHand, ref x, ref handPositions);
        UpdateCardPositionsInHand(numCardsInHand, ref deckList, ref handPositions, yPos);

    }

    public void AddCardToHand(ref int numCardsInHand, ref List<float> HandPositions, ref float x, ref List<GameObject> deckList, float yPos)
    {
        numCardsInHand++;
        HandPositions.Add(0f);
        newCardPos = 0;

        SetCardPositionsInHand(numCardsInHand, ref x, ref HandPositions);
        UpdateCardPositionsInHand(numCardsInHand, ref deckList, ref HandPositions, yPos);
    }

    public IEnumerator DrawCards(int numCards, string target)
    {
        for(int i = 0; i < numCards; i++)
        {
            if (target == "player")
            {
                if(playerDeckList.Count - numCards >= 0)
                {
                    DrawCards(ref numCardsInPlayerHand, "player", ref playerDeckList, ref Player_HandPositions, ref player_x, player_yPos);
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
                if (enemyDeckList.Count - numCards >= 0)
                {
                    DrawCards(ref numCardsInEnemyHand, "enemy", ref enemyDeckList, ref Enemy_HandPositions, ref enemy_x, enemy_yPos);
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

    private void DrawCards(ref int numCardsInHand, string target, ref List<GameObject> deckList, ref List<float> handPositions, ref float x, float yPos)
    {
        //play sound
        soundScript.PlaySound_DrawCard();

        //add a card to the hand
        AddCardToHand(ref numCardsInHand, ref handPositions, ref x, ref deckList, yPos);

        statManagerScript.UpdateCardsInDeck(target, -1);

        //card is now in the player's hand

        //playerHandList.Add(playerDeckList[numCardsInHand - 1]);
        deckList[numCardsInHand - 1].GetComponent<CardMovement>().isInHand = true;

        //set the position(index) of the card in the hand
        deckList[numCardsInHand - 1].GetComponent<CardMovement>().posInHand = numCardsInHand - 1;

        //change the card's layering
        SetLayeringInHand(ref deckList);

        //ON ARRIVAL
        if(target == "enemy")
        {
            if (enemyDeckList[numCardsInHand - 1].GetComponent<CardObj>().CardName == "Lethargy")
            {
                print("LETHARGY");
                spiderScript.Lethargy();

                enemyDeckList[numCardsInHand - 1].GetComponent<CardMovement>().PlayEnemyCard();

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
            count++;
        }

        statManagerScript.SetTotalCards(target, deckList.Count);
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
       /* if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(DamagePlayer(5));
        }*/
    }

    public IEnumerator Call_TakeDamage(int value, string target)
    {
        int chosenIndex;
        List<GameObject> tempDeckPileList = new List<GameObject>();
        //create a new list that contains only the cards in the deck pile
        //loop through all cards
        for (int i = 0; i < playerDeckList.Count; i++)
        {
            //if the card is not being held and has not been played (in the deck pile)
            if (!playerDeckList[i].GetComponent<CardMovement>().isInHand
                && !playerDeckList[i].GetComponent<CardMovement>().isPlayed)
            {
                tempDeckPileList.Add(playerDeckList[i]);
            }
        }

        //loop through deck and randomly burn cards
        for (int i = 0; i < value; i++)
        {
            //play burn sound 

            //check that there are cards in the to burn
            if (tempDeckPileList.Count > 0)
            {
                chosenIndex = Random.Range(0, tempDeckPileList.Count);
                print("index: " + chosenIndex);
                areaManagerScript.TakeDamage(tempDeckPileList[chosenIndex]);
                tempDeckPileList.RemoveAt(chosenIndex);
            }
            else
            {
                RemakeDeck();
                yield return new WaitForSecondsRealtime(0.2f);
                StartCoroutine(TakeDamage(1));
            }
            yield return new WaitForSecondsRealtime(1.1f);

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
            SetCardPositionsInHand(numCardsInPlayerHand, ref player_x, ref Player_HandPositions);
        }
        else if (target == "enemy")
        {
            print("enemy numCards in hand: " + numCardsInEnemyHand);
            SetCardPositionsInHand(numCardsInEnemyHand, ref enemy_x, ref Enemy_HandPositions);
        }
    }

    private void SetCardPositionsInHand(int numCardsInHand, ref float x, ref List<float> handPositions)
    {
        //check if number of cards is odd or even
        if (numCardsInHand > 1)
        {
            multiplier = (numCardsInHand - 1f) / 2;

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
            for (int i = 0; i < numCardsInHand; i++)
            {
                newCardPos = multiplier * x;
                handPositions[i] = newCardPos;
                multiplier -= 1f;
            }
        }
        else
        {
            if(numCardsInHand != 0)
                handPositions[0] = 0;
        }
    }

    //called by the area manager
    public void Call_UpdateCardPositionsInHand(string target)
    {
        if (target == "player")
        {
            UpdateCardPositionsInHand(numCardsInPlayerHand, ref playerDeckList, ref Player_HandPositions, player_yPos);
        }
        else if (target == "enemy")
        {
            UpdateCardPositionsInHand(numCardsInEnemyHand, ref enemyDeckList, ref Enemy_HandPositions, enemy_yPos);
        }
    }

    private void UpdateCardPositionsInHand(int numCardsInHand, ref List<GameObject> deckList, ref List<float> handPositions, float yPos)
    {
        //assign positions to cards
        for (int i = 0; i < numCardsInHand; i++)
        {
            CardMovement cardScript = deckList[i].GetComponent<CardMovement>();

            var obj = (GameObject)Instantiate(TempObj, new Vector3(handPositions[i], 0f, yPos), Quaternion.Euler(90,0,0));

            deckList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;

        }
    }
}
