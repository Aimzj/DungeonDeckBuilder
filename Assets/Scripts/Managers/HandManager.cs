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
    private CardEffectManager cardEffectScript;

    private Spider spiderScript;

    private Transform tempPlayerDisplay;

    // Use this for initialization
    void Start () {
        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();
        playerDiscard = GameObject.Find("Discard").GetComponent<Transform>();
        playerTrash = GameObject.Find("Trash").GetComponent<Transform>();
        enemyDeck = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        enemyDiscard = GameObject.Find("EnemyDiscard").GetComponent<Transform>();
        enemyTrash = GameObject.Find("EnemyTrashPile").GetComponent<Transform>();

        tempPlayerDisplay = GameObject.Find("PlayerTempDisplay").GetComponent<Transform>();

        soundScript = GameObject.Find("SoundMaker").GetComponent<SoundManager>();
        cardGenScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        cardEffectScript = GameObject.Find("GameManager").GetComponent<CardEffectManager>();

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

    public void StopEverything()
    {
        StopAllCoroutines();
    }

    //called at the start of the game to initialise cards
    //the enemyDeckList changes according to the level - spider or level 1 and Naga for level 2
    public void InitialiseCards(int level)
    {

        print("LEVEL: " + level.ToString());
        playerDeckList = cardGenScript.PlayerDeck;
        if(level == 0)
        {
            print("START TUT");
            playerDeckList = cardGenScript.PlayerTutDeck;
            enemyDeckList = cardGenScript.EnemyTutDeck;
            //Generate Tut Decks
            for(int i  = 0; i < playerDeckList.Count; i ++)
            {
                var playerTempCard = Instantiate(playerDeckList[i], playerDeck.position, Quaternion.Euler(90, 0, 0));
                playerDeckList[i] = playerTempCard;
                var enemyTempCard = Instantiate(enemyDeckList[i], enemyDeck.position, Quaternion.Euler(90, 0, 0));
                enemyDeckList[i] = enemyTempCard;
            }
        }
        else if (level == 1)
        {
            enemyDeckList = cardGenScript.SpiderDeck;
        } else if (level == 2)
        {
            enemyDeckList = cardGenScript.NagaDeck;
        }

        if(level != 0)
        {
            //shuffle the card lists
            Shuffle(ref playerDeckList);
            Shuffle(ref enemyDeckList);
        }
       
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
                if(playerDeckList.Count - 1 >= 0)
                {
                    DrawCards("player", ref playerDeckList, ref Player_HandPositions, ref player_x, player_yPos, ref playerHandList);
                }
                else
                {
                    if (areaManagerScript.player_DiscardCardList.Count > 0)
                    {
                        RemakeDeck("player", ref areaManagerScript.player_DiscardCardList, ref playerDeckList, playerDeck);
                        yield return new WaitForSecondsRealtime(0.3f);
                        i--;
                    }
                }
            }
            else if (target == "enemy")
            {
                if (enemyDeckList.Count - 1 >= 0)
                {
                    DrawCards("enemy", ref enemyDeckList, ref Enemy_HandPositions, ref enemy_x, enemy_yPos, ref enemyHandlist);
                }
                else
                {
                    print("I'm trying to draw");
                    //check that there are cards in the discard
                    if (areaManagerScript.enemy_DiscardCardList.Count > 0)
                    {
                        RemakeDeck("enemy", ref areaManagerScript.enemy_DiscardCardList, ref enemyDeckList, enemyDeck);
                        yield return new WaitForSecondsRealtime(0.3f);
                        i--;
                    }
                   
                }
            }

            yield return new WaitForSecondsRealtime(0.3f);
        }

    }

    public IEnumerator DrawDiscard(int numCards, string target)
    {
        for (int i = 0; i < numCards; i++)
        {
            if (target == "player")
            {
                if (areaManagerScript.player_DiscardCardList.Count - numCards >= 0)
                {
                    DrawCards("player", ref areaManagerScript.player_DiscardCardList, ref Player_HandPositions, ref player_x, player_yPos, ref playerHandList);
                }

            }
            else if (target == "enemy")
            {
                if (areaManagerScript.enemy_DiscardCardList.Count - numCards >= 0)
                {
                    DrawCards("enemy", ref areaManagerScript.enemy_DiscardCardList, ref Enemy_HandPositions, ref enemy_x, enemy_yPos, ref enemyHandlist);
                }
            }

                yield return new WaitForSecondsRealtime(0.3f);
        }

    }

    public IEnumerator DrawTrash(int numCards, string target)
    {
        for (int i = 0; i < numCards; i++)
        {
        
           if (target == "enemy")
            {
                if (areaManagerScript.enemy_TrashCardList.Count - numCards >= 0)
                {
                    DrawCards("enemy", ref areaManagerScript.enemy_TrashCardList, ref Enemy_HandPositions, ref enemy_x, enemy_yPos, ref enemyHandlist);
                }
            }

            yield return new WaitForSecondsRealtime(0.3f);
        }

    }

    private void DrawCards(string target, ref List<GameObject> deckList, ref List<float> handPositions, ref float x, float yPos, ref List<GameObject> handList)
    {
        //play sound
        soundScript.PlaySound_DrawCard();

        statManagerScript.UpdateCardsInDeck(target, -1,0);
        
        //add the card in the first position of the deck list to the hand list
        handList.Add(deckList[deckList.Count-1]);
        //update positions in hand
        UpdatePosInHand(ref handList);
        //the card is now in the hand
        handList[handList.Count-1].GetComponent<CardMovement>().isInHand = true;
        //remove the card in the decklist that was just added to the hand list
        deckList.RemoveAt(deckList.Count - 1);

        //check if the card drawn was kindling
        if(handList[handList.Count - 1].GetComponent<CardMovement>().isKindling)
        {
            if (target == "enemy")
            {
                print("ENEMY DREW KINDLING");
            }
            statManagerScript.UpdateKindling(target, -1, 0);
        }

        //set the position(index) of the card in the hand
       // handList[handList.Count - 1].GetComponent<CardMovement>().posInHand = handList.Count-1;

        //ADD card to hand (visually)
        handPositions.Add(0f);
        newCardPos = 0;
        SetCardPositionsInHand(ref x, ref handPositions, ref handList);
        UpdateCardPositionsInHand(ref handPositions, yPos, ref handList);

        //change the card's layering
        ReorderHandLayers(target);

        //ON ARRIVAL PLAYER
        cardEffectScript.PlayOnArrivalEffects(handList[handList.Count - 1]);

        //ON ARRIVAL
        if(target == "enemy")
        {
            if (handList[handList.Count - 1].GetComponent<CardObj>().CardName == "Lethargy")
            {
                print("LETHARGY");
                spiderScript.Lethargy();

                handList[handList.Count - 1].GetComponent<CardMovement>().PlayEnemyCard();

            }
            else if (handList[handList.Count - 1].GetComponent<CardObj>().CardName == "Lesser Guard")
            {
                print("LesserGuard");
                statManagerScript.UpdateDiscard("enemy", 1);
                //Trigger Prompt

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

    //shuffles the discard pile and adds cards back into the deck
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

            if (discardList[j].GetComponent<CardObj>().CardType == "status")
            {
                statManagerScript.UpdateNumStatusCards(0, -1);
            }

            //change target position of each card to the player's deck
            var obj = (GameObject)Instantiate(TempObj, new Vector3(deckPos.position.x, deckPos.position.y, deckPos.position.z), Quaternion.Euler(90, 0, 0));
            deckList[count].GetComponent<CardMovement>()._targetTransform = obj.transform;
            deckList[count].GetComponent<CardMovement>().isInHand = false;
            deckList[count].GetComponent<CardMovement>().isPlayed = false;

            //check if the added card is kindling
            if (deckList[count].GetComponent<CardMovement>().isKindling)
            {
                statManagerScript.UpdateKindling(target, 1, 0);
            }

            count++;

            statManagerScript.UpdateCardsInDeck(target, 1, 0);
        }

        //statManagerScript.SetTotalCards(target, deckList.Count,0);
        discardList.Clear();
        

        //deal damage for the cycle tokens
        /*  if (target == "player")
          {
              if (statManagerScript.numCycleTokens_player - 1 > 0)
              {
                  Call_TakeDamage(statManagerScript.numCycleTokens_player - 1, "player");
              }
          }
          else if (target == "enemy")
          {
              if (statManagerScript.numCycleTokens_enemy - 1 > 0)
              {
                  Call_TakeDamage(statManagerScript.numCycleTokens_enemy - 1, "enemy");
              }
          }*/

    }

    public void ReorderHandLayers(string target)
    {
        if (target == "player")
        {
            //loop through all the cards in the hand (that are not hovering) and set their positions
            //starting from layer 6
            int count = 6;
            for (int i = playerHandList.Count-1; i >=0; i--)
            {
                if (!playerHandList[i].GetComponent<CardMovement>().isHovering)
                {
                    playerHandList[i].GetComponent<CardMovement>().ChangeOrder(count);
                    count +=2;
                }
            }
        }
        else if (target == "enemy")
        {
            //loop through all the cards in the hand (that are not hovering) and set their positions
            //starting from layer 6
            int count = 6;
            for (int i = enemyHandlist.Count - 1; i >= 0; i--)
            {
                if (!enemyHandlist[i].GetComponent<CardMovement>().isHovering)
                {
                    enemyHandlist[i].GetComponent<CardMovement>().ChangeOrder(count);
                    count += 2;
                }
            }
        }

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
            StartCoroutine(Call_TakeDamage(5, "player"));
        }
    }

    public IEnumerator DiscardFromBottomOfDeck(int value)
    {
        for(int i=0; i<value; i++)
        {
            //check that there are cards to discard in the deck
            if (playerDeckList.Count > 0)
            {
                //choose the card at the bottom of the deck
                areaManagerScript.player_DiscardCardList.Add(playerDeckList[0]);

                StartCoroutine(areaManagerScript.TempDisplay(playerDeckList[0], tempPlayerDisplay, playerDiscard));

                //check if the card was kindling
                if (playerDeckList[0].GetComponent<CardMovement>().isKindling)
                {
                    statManagerScript.UpdateKindling("player", -1, 0);
                }
                //check if the card was a status card
                if (playerDeckList[0].GetComponent<CardObj>().CardType == "status")
                {
                    statManagerScript.UpdateNumStatusCards(-1, 1);
                }

                //remove from the decklist
                playerDeckList.RemoveAt(0);

                yield return new WaitForSeconds(1);
            }
            else
            {
                RemakeDeck("player", ref areaManagerScript.player_DiscardCardList, ref playerDeckList, playerDeck);
                yield return new WaitForSeconds(0.5f);
                i--;
            }
        }
        

        
    }

    public IEnumerator Call_TakeDamage(int value, string target)
    {
        int chosenIndex=0;
        List<GameObject> tempDeckPileList = new List<GameObject>();
        List<GameObject> tempHandList = new List<GameObject>();
        List<GameObject> tempDiscardList = new List<GameObject>();
        List<GameObject> tempPlayList = new List<GameObject>();

        InitialiseDeck(ref tempDeckPileList, ref tempHandList, ref tempDiscardList,ref tempPlayList, target);

        //loop through deck and randomly burn cards
        for (int i = 0; i < value; i++)
        {
            //play burn sound 

            //check that there are cards in the deck to burn
            if (tempDeckPileList.Count > 0)
            {
                //KINDLING
                if (target == "player")
                {
                    //if there is kindling in the deck
                    if (statManagerScript.numKindling_player>0)
                    {
                        //loop through the entire deck looking for the kindling
                        for(int k=0; k<tempDeckPileList.Count; k++)
                        {
                            //is this card kindling?
                            if (tempDeckPileList[k].GetComponent<CardMovement>().isKindling)
                            {
                                chosenIndex = k;
                            }
                        }
                        //update kindling left in deck
                        statManagerScript.UpdateKindling(target, -1, -1);
                    }
                    else
                    {
                        chosenIndex = Random.Range(0, tempDeckPileList.Count);
                    }
                }else if (target == "enemy")
                {
                    //if there is kindling in the deck
                    if (statManagerScript.numKindling_enemy > 0)
                    {
                        //loop through the entire deck looking for the kindling
                        for (int k = 0; k < tempDeckPileList.Count; k++)
                        {
                            //is this card kindling?
                            if (tempDeckPileList[k].GetComponent<CardMovement>().isKindling)
                            {
                                chosenIndex = k;
                            }
                        }
                        //update kindling left in deck
                        statManagerScript.UpdateKindling(target, -1, -1);
                    }
                    else
                    {
                        chosenIndex = Random.Range(0, tempDeckPileList.Count);
                    }
                }

                if (target == "player")
                {                    
                    areaManagerScript.Call_TakeDamage(tempDeckPileList[chosenIndex], "player");
                }     
                else if(target == "enemy")
                {
                    areaManagerScript.Call_TakeDamage(tempDeckPileList[chosenIndex], "enemy");
                }
                tempDeckPileList.RemoveAt(chosenIndex);
            }
            else if (tempDiscardList.Count > 0)
            {
                //check if there are cards in the discard pile
                //reshuffle the discard pile
                if (target == "player")
                {
                    RemakeDeck(target, ref areaManagerScript.player_DiscardCardList, ref playerDeckList, playerDeck);
                }
                else if (target == "enemy")
                {
                    print("I need to reshuffle to burn a card");
                    RemakeDeck(target, ref areaManagerScript.enemy_DiscardCardList, ref enemyDeckList, enemyDeck); 
                }

                yield return new WaitForSecondsRealtime(0.5f);
                InitialiseDeck(ref tempDeckPileList, ref tempHandList, ref tempDiscardList,ref tempPlayList, target);
                yield return new WaitForSecondsRealtime(0.2f);
                i--;
            }
            //check if there are cards in the player's hand
            else if (tempHandList.Count > 0)
            {
                
                print("num cards in deck: " + tempDeckPileList.Count + "!!");
                //check if there is kindling in the hand
                bool isFound = false;
                for(int k =0; k<tempHandList.Count; k++)
                {
                    //is this card kindling?
                    if (tempHandList[k].GetComponent<CardMovement>().isKindling)
                    {
                        chosenIndex = k;
                        isFound = true;
                    }
                }
                if (isFound)
                {
                    statManagerScript.UpdateKindling(target, 0, -1);
                }
                else
                {
                    chosenIndex = Random.Range(0, tempHandList.Count);
                }           

                //burn them
                print("index: " + chosenIndex);
                areaManagerScript.Call_TakeDamage(tempHandList[chosenIndex], target);
                tempHandList.RemoveAt(chosenIndex);

                Call_SetPositionsInHand(target);
                Call_UpdateCardPositionsInHand(target);
            }
            //check if there are cards in play
            else if (tempPlayList.Count > 0)
            {
                bool isFound = false;
                //check if there is kindling in play
                for (int k = 0; k < tempPlayList.Count; k++)
                {
                    //is this card kindling?
                    if (tempPlayList[k].GetComponent<CardMovement>().isKindling)
                    {
                        chosenIndex = k;
                        isFound = true;
                    }
                }
                if(!isFound)
                {
                    chosenIndex = Random.Range(0, tempPlayList.Count);
                }

                //burn them
                print("index: " + chosenIndex);
                areaManagerScript.Call_TakeDamage(tempPlayList[chosenIndex], target);
                tempPlayList.RemoveAt(chosenIndex);

                Call_SetPositionsInHand(target);
                Call_UpdateCardPositionsInHand(target);
            }
            
            yield return new WaitForSecondsRealtime(1.1f);

        }

    }

    private void InitialiseDeck(ref List<GameObject> tempDeckPileList, ref List<GameObject> tempHandList, ref List<GameObject> tempDiscardList,ref List<GameObject> tempPlayList, string target)
    {
        if (target == "player")
        {
            tempDeckPileList = playerDeckList;
            tempHandList = playerHandList;
            tempDiscardList = areaManagerScript.player_DiscardCardList;
            tempPlayList = areaManagerScript.player_PlayCardList;
        }
        else if(target == "enemy")
        {
            tempDeckPileList = enemyDeckList;
            tempHandList = enemyHandlist;
            tempDiscardList = areaManagerScript.enemy_DiscardCardList;
            tempPlayList = areaManagerScript.enemy_PlayCardList;
        }

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
