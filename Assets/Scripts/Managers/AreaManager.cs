using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {

    public List<GameObject> player_PlayCardList, enemy_PlayCardList;
    public List<GameObject> player_DiscardCardList, enemy_DiscardCardList;
    public List<GameObject> player_TrashCardList, enemy_TrashCardList;
    public List<GameObject> player_DeckCardList, enemy_DeckCardList;

    //possible target positions
    private Transform playerDeck, enemyDeck;
    private Transform playerDiscard, enemyDiscard;
    private Transform playerTrash, enemyTrash;

    private Transform playerTempDisplay, enemyTempDisplay;

    private Transform player_playAreaTrans, enemy_playAreaTrans;

    public GameObject TempObj;

    //list of x positions for the cards in the play area
    public List<float> player_PlayAreaPositions, enemy_PlayAreaPositions;
    //distance between the cards in the play area.
    private float player_x, enemy_x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    private float newCardPos, multiplier;

    private int player_numCardsInPlay, enemy_numCardsInPlay;

    private HandManager handManagerScript;
    private StatsManager statManagerScript;
    private SoundManager soundManagerScript;

    public ParticleSystem bigFireball;
    private Dummy dummyScript;

    //For Tut
    public int level = 0;

    // Use this for initialization
    void Start () {
        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();
        playerDiscard = GameObject.Find("Discard").GetComponent<Transform>();
        playerTrash = GameObject.Find("Trash").GetComponent<Transform>();
        enemyDeck = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        enemyDiscard = GameObject.Find("EnemyDiscardPile").GetComponent<Transform>();
        enemyTrash = GameObject.Find("EnemyTrashPile").GetComponent<Transform>();

        playerTempDisplay = GameObject.Find("PlayerTempDisplay").GetComponent<Transform>();
        enemyTempDisplay = GameObject.Find("EnemyTempDisplay").GetComponent<Transform>();

        player_playAreaTrans = GameObject.Find("PlayArea").GetComponent<Transform>();
        enemy_playAreaTrans = GameObject.Find("EnemyPlayArea").GetComponent<Transform>();

        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        soundManagerScript = GameObject.Find("SoundMaker").GetComponent<SoundManager>();

        dummyScript = GameObject.Find("GameManager").GetComponent<Dummy>();

        player_numCardsInPlay = 0;
        enemy_numCardsInPlay = 0;

        player_x = 4;
        enemy_x = 4;

        boundaryValue = 5;
    }

    public void StopEverything()
    {
        StopAllCoroutines();
    }

    public void Call_DiscardCard(GameObject cardObj, string target)
    {
        if (target == "player")
        {
            DiscardCard(cardObj, target, ref player_DiscardCardList, playerDiscard);
        }
        else if(target == "enemy")
        {
            DiscardCard(cardObj, target, ref enemy_DiscardCardList, enemyDiscard);
        }
    }

    public void ReorderPlayAreaLayers(string target)
    {
        if (target == "player")
        {
            //loop through all the cards in the hand (that are not hovering) and set their positions
            //starting from layer 6
            int count = 6;
            for (int i = player_PlayCardList.Count - 1; i >= 0; i--)
            {
                if (!player_PlayCardList[i].GetComponent<CardMovement>().isHovering)
                {
                    player_PlayCardList[i].GetComponent<CardMovement>().ChangeOrder(count);
                    count += 2;
                }
            }
        }
        else if (target == "enemy")
        {
            //loop through all the cards in the hand (that are not hovering) and set their positions
            //starting from layer 6
            int count = 6;
            for (int i = enemy_PlayCardList.Count - 1; i >= 0; i--)
            {
                    enemy_PlayCardList[i].GetComponent<CardMovement>().ChangeOrder(count);
                    count += 2;
            }
        }
    }

    private void OrderLayerDiscard(ref List<GameObject> discardList)
    {       
        //loop through discard list
        for (int i=0; i < discardList.Count; i++)
        {
            discardList[i].GetComponent<CardMovement>().ChangeOrder(11);
        }
        discardList[discardList.Count - 1].GetComponent<CardMovement>().ChangeOrder(13);
    }

    private void DiscardCard(GameObject cardObj, string target, ref List<GameObject> discardList, Transform discardTrans)
    {
      
        //add 1 to the discard pool
        statManagerScript.UpdateDiscard(target, 1);

        //add card to list of discards
        discardList.Add(cardObj);

        //card has now been played
        cardObj.GetComponent<CardMovement>().isPlayed = true;

        //move the card's position to discard pile
        var obj = (GameObject)Instantiate(TempObj, new Vector3(discardTrans.position.x, discardTrans.position.y, discardTrans.position.z), Quaternion.Euler(90, 90, 0));
        cardObj.GetComponent<CardMovement>()._targetTransform = obj.transform;
        //arrange cards in hand
        handManagerScript.Call_SetPositionsInHand(target);
        handManagerScript.Call_UpdateCardPositionsInHand(target);

        OrderLayerDiscard(ref discardList);
    }

    public void Call_TakeDamage(GameObject cardObj, string target)
    {
        if(target == "player")
        {
            TakeDamage(cardObj, target, ref player_TrashCardList, playerTempDisplay, playerTrash);
        }
        else if(target == "enemy")
        {
            TakeDamage(cardObj, target, ref enemy_TrashCardList, enemyTempDisplay, enemyTrash);
        }
    }

    private void TakeDamage(GameObject cardObj, string target, ref List<GameObject> trashList, Transform tempDisplay, Transform trashTrans)
    {
        //add card to list of trashed cards
        trashList.Add(cardObj);

        //subtract from total cards
        statManagerScript.UpdateCardsInDeck(target, -1,-1);

        //add card to list of trashed cards
        trashList.Add(cardObj);

        //card has now been played
        trashList[trashList.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        StartCoroutine(TempDisplay(cardObj, tempDisplay, trashTrans));

        //damage player
        statManagerScript.UpdateHealth(target, -1,0);
        //check if burnt card was a Sigil card
        if (cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled)
        {
            statManagerScript.UpdateSigils(target, -1);
        }
    }

    public IEnumerator TempDisplay(GameObject card, Transform tempDisplay, Transform targetTrans)
    {
        //change the card's order in layer
        card.GetComponent<CardMovement>().ChangeOrder(100);

        //move the card's position to player's temp display
        var obj = (GameObject)Instantiate(TempObj, new Vector3(tempDisplay.position.x, tempDisplay.position.y, tempDisplay.position.z), Quaternion.Euler(90, 0, 0));
        card.GetComponent<CardMovement>()._targetTransform = obj.transform;

        yield return new WaitForSecondsRealtime(1);

        //move the card's position to target position
        obj = (GameObject)Instantiate(TempObj, new Vector3(targetTrans.position.x, targetTrans.position.y, targetTrans.position.z), Quaternion.Euler(90, targetTrans.eulerAngles.y, 0));
        card.GetComponent<CardMovement>()._targetTransform = obj.transform;       

        if(targetTrans.position == playerTrash.position || targetTrans.position == enemyTrash.position)
        {
            soundManagerScript.PlaySound_BurnCard();
            bigFireball.Play();
        }
        yield return new WaitForSecondsRealtime(0.5f);

        //change order in layer
        card.GetComponent<CardMovement>().ChangeOrder(0);
    }

    public void Call_TrashCard(GameObject cardObj, string target)
    {
        if(target == "player")
        {
            TrashCard(cardObj, target, ref player_TrashCardList, playerTrash);
        }
        else if(target == "enemy")
        {
            TrashCard(cardObj, target, ref enemy_TrashCardList, enemyTrash);
        }
    }

    public void TrashCard(GameObject cardObj, string target, ref List<GameObject> trashList, Transform trashTrans)
    {
        bigFireball.Play();
        soundManagerScript.PlaySound_BurnCard();

        //add 1 to the burn pool
        statManagerScript.UpdateBurn(target, 1);

        //subtract from total cards
        statManagerScript.UpdateCardsInDeck(target,0, -1);

        //add card to list of trashed cards
        trashList.Add(cardObj);

        //card has now been played
        trashList[trashList.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        //move the card's position to trash pile
        var obj = (GameObject)Instantiate(TempObj, new Vector3(trashTrans.position.x, trashTrans.position.y, trashTrans.position.z), Quaternion.Euler(90, 90, 0));
        cardObj.GetComponent<CardMovement>()._targetTransform = obj.transform;

        //damage player
        statManagerScript.UpdateHealth(target, -1,0);

        //check if burnt card was a Sigil card
        if (cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled)
        {
            statManagerScript.UpdateSigils(target, -1);
        }
        //check if card burnt was a kindling card
        if (cardObj.GetComponent<CardMovement>().isKindling)
        {
            statManagerScript.UpdateKindling(target, 0, -1);
        }

        //arrange cards in hand
        handManagerScript.Call_SetPositionsInHand(target);
        handManagerScript.Call_UpdateCardPositionsInHand(target);

        StartCoroutine(OrderChangeDelay(cardObj));
    }

    IEnumerator OrderChangeDelay(GameObject card)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        //change order in layer
        card.GetComponent<CardMovement>().ChangeOrder(0);
    }

    public void Call_PlayCard(GameObject cardObj, string target)
    {
        if (target == "player")
        {
            PlayCard(cardObj, ref player_numCardsInPlay, ref player_PlayAreaPositions, ref player_x, ref player_PlayCardList, "player", player_playAreaTrans);
        }
        else if (target == "enemy")
        {
            PlayCard(cardObj, ref enemy_numCardsInPlay, ref enemy_PlayAreaPositions, ref enemy_x, ref enemy_PlayCardList, "enemy", enemy_playAreaTrans);
        }
    }

    private void PlayCard(GameObject cardObj, ref int numCardsInPlay, ref List<float> playAreaPositions, ref float x, ref List<GameObject> playList, string target, Transform playAreaTrans)
    {
        numCardsInPlay++;
        playAreaPositions.Add(0f);
        newCardPos = 0;
        //check if number of cards is odd or even
        if (numCardsInPlay > 1)
        {
            multiplier = (numCardsInPlay - 1f) / 2;

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
            for (int i = 0; i < numCardsInPlay; i++)
            {
                newCardPos = multiplier * x;
                playAreaPositions[i] = newCardPos;
                multiplier -= 1f;
            }
        }
        else
        {
            playAreaPositions[0] = 0;
        }

        //add the card to list
        playList.Add(cardObj);

        //card has now been played
        playList[playList.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        //assign positions to cards
        for (int i = 0; i < playList.Count; i++)
        {
            var obj = (GameObject)Instantiate(TempObj, new Vector3(playAreaPositions[i], playAreaTrans.position.y, playAreaTrans.position.z), Quaternion.Euler(90, 0, 0));

            playList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;

        }

        handManagerScript.Call_SetPositionsInHand(target);
        handManagerScript.Call_UpdateCardPositionsInHand(target);
  
        ReorderPlayAreaLayers(target);

    }

    public void Call_DiscardPlayArea(string target)
    {
        if(target == "player")
        {
            DiscardPlayArea(ref player_numCardsInPlay, ref player_PlayCardList, ref player_DiscardCardList, playerDiscard);
        }
        else if(target == "enemy")
        {
            DiscardPlayArea(ref enemy_numCardsInPlay, ref enemy_PlayCardList, ref enemy_DiscardCardList, enemyDiscard);
        }
    }

    private void DiscardPlayArea(ref int numCardsInPlay, ref List<GameObject> playList, ref List<GameObject> discardList, Transform discardTrans)
    {
        //check if there are cards present in the play area
        if (numCardsInPlay > 0)
        {
            //loop through play area list and add to discard list
            for (int i = 0; i < playList.Count; i++)
            {
                //disable burn halos
                playList[i].transform.Find("BurnBorder").GetComponent<SpriteRenderer>().enabled = false;

                discardList.Add(playList[i]);

                //change target position of each card to the discard pile
                var obj = (GameObject)Instantiate(TempObj, new Vector3(discardTrans.position.x, discardTrans.position.y, discardTrans.position.z), Quaternion.Euler(90, 90, 0));
                playList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;
            }
            //clear play list
            playList.Clear();
            numCardsInPlay = 0;
        }

        //count how many status effects are in the discard
        for(int i=0; i<player_DiscardCardList.Count; i++)
        {
            if (player_DiscardCardList[i].GetComponent<CardObj>().CardType == "status")
            {
                statManagerScript.UpdateNumStatusCards(0, 1);
            }
        }
    }

    public void Call_RenewDeck(string target)
    {
        if(target == "player")
        {
            RenewDeck(ref player_DiscardCardList, playerDeck, ref player_DeckCardList);
        }else if(target == "enemy")
        {
            RenewDeck(ref enemy_DiscardCardList, enemyDeck, ref enemy_DeckCardList);
        }
    }

    private void RenewDeck(ref List<GameObject> discardList, Transform deckTrans, ref List<GameObject> deckList)
    {
        //check if there are cards present in the discard pile
        if (discardList.Count > 0)
        {
            //loop through play area list and add to deck list
            for (int i = 0; i < discardList.Count; i++)
            {
                deckList.Add(discardList[i]);

                //change target position of each card to the deck
                var obj = (GameObject)Instantiate(TempObj, new Vector3(deckTrans.position.x, deckTrans.position.y, deckTrans.position.z), Quaternion.Euler(90, 0, 0));
                discardList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;
            }
            //clear play list
            discardList.Clear();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
