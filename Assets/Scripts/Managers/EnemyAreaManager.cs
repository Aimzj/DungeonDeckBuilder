using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaManager : MonoBehaviour {
    public List<GameObject> enemyCardList_Play;
    public List<GameObject> enemyCardList_Discard;
    public List<GameObject> enemyCardList_Trash;
    public List<GameObject> enemyCardList_Deck;

    //possible target positions
    private Transform enemyDeck;
    private Transform enemyDiscard;
    private Transform enemyTrash;

    private Transform enemyTempDisplay;

    private Transform enemyPlayAreaTrans;

    public GameObject TempObj;

    //list of x positions for the cards in the play area
    public List<float> EnemyPlayAreaPositions;
    //distance between the cards in the player's hand.
    private float x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    private float newCardPos, multiplier;

    private int numCardsInPlay;

    private EnemyHandManager enemyHandManagerScript;
    private StatsManager statManagerScript;
    // Use this for initialization
    void Start () {
        enemyDeck = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        enemyDiscard = GameObject.Find("EnemyDiscardPile").GetComponent<Transform>();
        enemyTrash = GameObject.Find("EnemyTrashPile").GetComponent<Transform>();

        enemyTempDisplay = GameObject.Find("EnemyTempDisplay").GetComponent<Transform>();

        enemyPlayAreaTrans = GameObject.Find("EnemyPlayArea").GetComponent<Transform>();

        enemyHandManagerScript = GameObject.Find("GameManager").GetComponent<EnemyHandManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();

        numCardsInPlay = 0;

        x = 4;
        boundaryValue = 5;
    }

    public void DiscardCard(GameObject cardObj)
    {
        //add 1 to the discard pool
        statManagerScript.UpdateDiscard("enemy", 1);

        //add card to list of discards
        enemyCardList_Discard.Add(cardObj);

        //card has now been played
        cardObj.GetComponent<CardMovement>().isPlayed = true;

        //move the card's position to discard pile
        var obj = (GameObject)Instantiate(TempObj, new Vector3(enemyDiscard.position.x, enemyDiscard.position.y, enemyDiscard.position.z), Quaternion.Euler(90, 90, 0));
        cardObj.GetComponent<CardMovement>()._targetTransform = obj.transform;

        //arrange cards in hand
        enemyHandManagerScript.SetCardPositionsInHand();
        enemyHandManagerScript.UpdateCardPositionsInHand();
    }

    public void TakeDamage(GameObject cardObj)
    {
        //add card to list of trashed cards
        enemyCardList_Trash.Add(cardObj);

        //subtract from total cards
        statManagerScript.UpdateTotalCards("enemy", -1);
        statManagerScript.UpdateCardsInDeck("enemy", -1);

        //add card to list of trashed cards
        enemyCardList_Trash.Add(cardObj);

        //card has now been played
        enemyCardList_Trash[enemyCardList_Trash.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        StartCoroutine(TempDisplayDamage(cardObj));
    }

    IEnumerator TempDisplayDamage(GameObject card)
    {
        //move the card's position to enemy's temp display
       // int origOrder = card.GetComponent<SpriteRenderer>().sortingOrder;
        card.GetComponent<CardMovement>().ChangeOrder(100);
        var obj = (GameObject)Instantiate(TempObj, new Vector3(enemyTempDisplay.position.x, enemyTempDisplay.position.y, enemyTempDisplay.position.z), Quaternion.Euler(90, 0, 0));
        card.GetComponent<CardMovement>()._targetTransform = obj.transform;

        yield return new WaitForSecondsRealtime(1);
        
        //move the card's position to enemy's trash pile
        obj = (GameObject)Instantiate(TempObj, new Vector3(enemyTrash.position.x, enemyTrash.position.y, enemyTrash.position.z), Quaternion.Euler(90, 90, 0));
        card.GetComponent<CardMovement>()._targetTransform = obj.transform;

        //damage enemy
        statManagerScript.UpdateHealth("enemy", -1);
        //check if burnt card was a Sigil card
        if (card.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled)
        {
            statManagerScript.UpdateSigils("enemy", -1);
        }
    }

    IEnumerator TempDisplayPlay(GameObject card)
    {
        print("Enemy Play temp display working");
        //move the card's position to enemy's temp display
        var obj = (GameObject)Instantiate(TempObj, new Vector3(enemyTempDisplay.position.x, enemyTempDisplay.position.y, enemyTempDisplay.position.z), Quaternion.Euler(90, 0, 0));
        card.GetComponent<CardMovement>()._targetTransform = obj.transform;

        yield return new WaitForSecondsRealtime(1);

        

    }

    public void TrashCard(GameObject cardObj)
    {
        //add 1 to the burn pool
        statManagerScript.UpdateBurn("enemy", 1);

        //subtract from total cards
        statManagerScript.UpdateTotalCards("enemy", -1);

        //add card to list of trashed cards
        enemyCardList_Trash.Add(cardObj);

        //card has now been played
        enemyCardList_Trash[enemyCardList_Trash.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        //move the card's position to trash pile
        var obj = (GameObject)Instantiate(TempObj, new Vector3(enemyTrash.position.x, enemyTrash.position.y, enemyTrash.position.z), Quaternion.Euler(90, 90, 0));
        cardObj.GetComponent<CardMovement>()._targetTransform = obj.transform;

        //damage enemy
        statManagerScript.UpdateHealth("enemy", -1);
        //check if burnt card was a Sigil card
        if (cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled)
        {
            statManagerScript.UpdateSigils("enemy", -1);
        }

        //arrange cards in hand
        enemyHandManagerScript.SetCardPositionsInHand();
        enemyHandManagerScript.UpdateCardPositionsInHand();
    }

    public void PlayCard(GameObject cardObj)
    {

        numCardsInPlay++;
        EnemyPlayAreaPositions.Add(0f);
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
                EnemyPlayAreaPositions[i] = newCardPos;
                multiplier -= 1f;
            }
        }
        else
        {
            EnemyPlayAreaPositions[0] = 0;
        }

        //add the card to list
        enemyCardList_Play.Add(cardObj);

        //card has now been played
        enemyCardList_Play[enemyCardList_Play.Count - 1].GetComponent<CardMovement>().isPlayed = true;

        //move the card's position to enemy's playArea
        //assign positions to cards
        for (int i = 0; i < enemyCardList_Play.Count; i++)
        {
            var obj = (GameObject)Instantiate(TempObj, new Vector3(EnemyPlayAreaPositions[i], enemyPlayAreaTrans.position.y, enemyPlayAreaTrans.position.z), Quaternion.Euler(90, 0, 0));

            enemyCardList_Play[i].GetComponent<CardMovement>()._targetTransform = obj.transform;
            print("move to target!");
        }

        enemyHandManagerScript.SetCardPositionsInHand();
        enemyHandManagerScript.UpdateCardPositionsInHand();


    }

    public void DiscardPlayArea()
    {
        //check if there are cards present in the play area
        if (numCardsInPlay > 0)
        {
            //loop through play area list and add to discard list
            for (int i = 0; i < enemyCardList_Play.Count; i++)
            {
                //disable burn halos
                enemyCardList_Play[i].transform.Find("BurnBorder").GetComponent<SpriteRenderer>().enabled = false;

                enemyCardList_Discard.Add(enemyCardList_Play[i]);

                //change target position of each card to the discard pile
                var obj = (GameObject)Instantiate(TempObj, new Vector3(enemyDiscard.position.x, enemyDiscard.position.y, enemyDiscard.position.z), Quaternion.Euler(90, 90, 0));
                enemyCardList_Play[i].GetComponent<CardMovement>()._targetTransform = obj.transform;
            }
            //clear play list
            enemyCardList_Play.Clear();
            numCardsInPlay = 0;
        }
    }

    public void RenewDeck()
    {
        //check if there are cards present in the discard pile
        if (enemyCardList_Discard.Count > 0)
        {
            //loop through play area list and add to deck list
            for (int i = 0; i < enemyCardList_Discard.Count; i++)
            {
                enemyCardList_Deck.Add(enemyCardList_Discard[i]);

                //change target position of each card to the deck
                var obj = (GameObject)Instantiate(TempObj, new Vector3(enemyDeck.position.x, enemyDeck.position.y, enemyDeck.position.z), Quaternion.Euler(90, 0, 0));
                enemyCardList_Discard[i].GetComponent<CardMovement>()._targetTransform = obj.transform;
            }
            //clear play list
            enemyCardList_Discard.Clear();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
