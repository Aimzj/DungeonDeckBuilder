using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHandManager : MonoBehaviour {
    //possible target positions
    private Transform enemyDeck, enemyDiscard, enemyTrash;
    private Transform displayPos;

    public GameObject CardObj;
    public GameObject TempObj;

    public List<GameObject> enemyDeckList;

    //list of x positions for the cards in the enemy's hand
    public List<float> HandPositions;
    //distance between the cards in the enemy's hand.
    private float x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    public int numCardsInHand;

    private float newCardPos, multiplier;

    //sounds
    public SoundManager soundScript;

    //hand size
    private int maxHandSize;

    public bool isExceedingHandSize;
    private TextMeshProUGUI limitExceeded_Text;
    private SpriteRenderer limit_halo;

    public CardGenerator cardGenScript;
    private EnemyAreaManager enemyAreaManagerScript;
    public StatsManager statManagerScript;

    private Spider spiderScript;

    void Start () {
        enemyDeck = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        enemyDiscard = GameObject.Find("EnemyDiscardPile").GetComponent<Transform>();
        enemyTrash = GameObject.Find("EnemyTrashPile").GetComponent<Transform>();

        spiderScript = GameObject.Find("GameManager").GetComponent<Spider>();

        displayPos = GameObject.Find("EnemyTempDisplay").GetComponent<Transform>();

        soundScript = GameObject.Find("SoundMaker").GetComponent<SoundManager>();
        cardGenScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();
        enemyAreaManagerScript = GameObject.Find("GameManager").GetComponent<EnemyAreaManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();

        limitExceeded_Text = GameObject.Find("HandLimitNotice").GetComponent<TextMeshProUGUI>();
        limitExceeded_Text.enabled = false;
        limit_halo = GameObject.Find("HandLimit_Halo").GetComponent<SpriteRenderer>();
        limit_halo.enabled = false;

        numCardsInHand = 0;

        x = 4;
        boundaryValue = 5;

        maxHandSize = 10;
    }

    public void InitialiseCards()
    {
        if(cardGenScript == null )
        {
            Debug.Log("EMPTY CARDGENSCRIPT");
        }else if(cardGenScript.SpiderDeck == null)
        {
            print("EMPTY SPIDER DECK");
        }
        else
        {
            enemyDeckList = cardGenScript.SpiderDeck;
        }
        

        //shuffle the card list
        Shuffle(ref enemyDeckList);
    }

    //shuffling cards
    public void Shuffle(ref List<GameObject> cardList)
    {
        int len = cardList.Count;
        Random rand = new Random();

        for (int i = 0; i < len; i++)
        {
            Swap(ref cardList, i, i + Random.Range(0, len - i));
        }
    }

    //swapping two positions in List
    private void Swap(ref List<GameObject> cards, int pos1, int pos2)
    {
        GameObject temp = cards[pos1];
        cards[pos1] = cards[pos2];
        cards[pos2] = temp;
    }

    public void RemoveCardFromHand(int pos)
    {
        numCardsInHand--;
        HandPositions.RemoveAt(pos);
        enemyDeckList.RemoveAt(pos);

        //loop through all cards after the removed card and change their pos value
        //check if the card removed was the last card
        if (pos != enemyDeckList.Count)
        {
            for (int i = pos; i < enemyDeckList.Count; i++)
            {
                enemyDeckList[i].GetComponent<CardMovement>().posInHand = i;
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

        SetCardPositionsInHand();
        UpdateCardPositionsInHand();
    }

    public void AddCardToHand()
    {
        numCardsInHand++;
        HandPositions.Add(0f);
        newCardPos = 0;

        SetCardPositionsInHand();
        UpdateCardPositionsInHand();
    }

    public IEnumerator DrawCards(int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            //check if there are enough cards left in deck
            if (enemyDeckList.Count - numCards >= 0)
            {
                Debug.Log("Draw enemy card!");
                //play sound
                soundScript.PlaySound_DrawCard();

                //add a card to the hand
                AddCardToHand();

                statManagerScript.UpdateCardsInDeck("enemy", -1);

                //card is now in the enemy's hand
                enemyDeckList[numCardsInHand - 1].GetComponent<CardMovement>().isInHand = true;

                //set the position(index) of the card in the hand
                enemyDeckList[numCardsInHand - 1].GetComponent<CardMovement>().posInHand = numCardsInHand - 1;

                //change the card's layering
                SetLayeringInHand();

                //ON ARRIVAL
                if(enemyDeckList[numCardsInHand - 1].GetComponent<CardObj>().CardName == "Lethargy")
                {
                    print("LETHARGY");
                    spiderScript.Lethargy();

                    enemyDeckList[numCardsInHand - 1].GetComponent<CardMovement>().PlayEnemyCard();

                }

                //UpdateCardPositionsInHand();

                yield return new WaitForSecondsRealtime(0.3f);
            }
            else
            {

                RemakeDeck();
                StartCoroutine(DrawCards(1));
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

    public void RemakeDeck()
    {
        //gain a cycle token
        statManagerScript.UpdateCycleTokens("enemy", 1);

        Debug.Log("Shuffle discard");
        //Need to shuffle the discard pile and reform the playerdeck
        Shuffle(ref enemyAreaManagerScript.enemyCardList_Discard);

        int count = enemyDeckList.Count;
        //add shuffled discard pile to playerDeck
        for (int j = 0; j < enemyAreaManagerScript.enemyCardList_Discard.Count; j++)
        {
            enemyDeckList.Add(enemyAreaManagerScript.enemyCardList_Discard[j]);

            //change target position of each card to the enemy's deck
            var obj = (GameObject)Instantiate(TempObj, new Vector3(enemyDeck.position.x, enemyDeck.position.y, enemyDeck.position.z), Quaternion.Euler(90, 0, 0));
            enemyDeckList[count].GetComponent<CardMovement>()._targetTransform = obj.transform;
            count++;
        }

        statManagerScript.SetTotalCards("enemy", enemyDeckList.Count);
        enemyAreaManagerScript.enemyCardList_Discard.Clear();
    }

    //loop through all cards and assign Layers
    private void SetLayeringInHand()
    {
        int count = 0;
        for (int i = 0; i < enemyDeckList.Count; i++)
        {
            if (enemyDeckList[i].GetComponent<CardMovement>().isInHand)
            {
                enemyDeckList[i].GetComponent<SpriteRenderer>().sortingOrder = 15 + count * 2;
                TextMeshPro tMP = enemyDeckList[i].transform.Find("Title").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = enemyDeckList[i].transform.Find("DiscardCost").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = enemyDeckList[i].transform.Find("BurnCost").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = enemyDeckList[i].transform.Find("DiscardEffect").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = enemyDeckList[i].transform.Find("BurnEffect").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = enemyDeckList[i].transform.Find("AttackCost").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;
                tMP = enemyDeckList[i].transform.Find("DefenseCost").GetComponent<TextMeshPro>();
                tMP.sortingOrder = 16 + count * 2;

                count++;
            }

        }

    }

    public IEnumerator DamageEnemy(int value)
    {
        int chosenIndex;
        List<GameObject> tempDeckPileList = new List<GameObject>();
        //create a new list that contains only the cards in the deck pile
        //loop through all cards
        for (int i = 0; i < enemyDeckList.Count; i++)
        {
            //if the card is not being held and has not been played (in the deck pile)
            if (!enemyDeckList[i].GetComponent<CardMovement>().isInHand
                && !enemyDeckList[i].GetComponent<CardMovement>().isPlayed)
            {
                tempDeckPileList.Add(enemyDeckList[i]);
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
                enemyAreaManagerScript.TakeDamage(tempDeckPileList[chosenIndex]);
                tempDeckPileList.RemoveAt(chosenIndex);
            }
            else
            {
                RemakeDeck();
                yield return new WaitForSecondsRealtime(0.2f);
                StartCoroutine(DamageEnemy(1));
            }
            yield return new WaitForSecondsRealtime(1.1f);

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
                HandPositions[i] = newCardPos;
                multiplier -= 1f;
            }
        }
        else
        {
            if (numCardsInHand != 0)
                HandPositions[0] = 0;
        }
    }

    public void UpdateCardPositionsInHand()
    {
        //assign positions to cards
        Debug.Log("numCards in Enemy hand: " + numCardsInHand);
        for (int i = 0; i < numCardsInHand; i++)
        {
            CardMovement cardScript = enemyDeckList[i].GetComponent<CardMovement>();

            var obj = (GameObject)Instantiate(TempObj, new Vector3(HandPositions[i], 0f, 10f), Quaternion.Euler(90, 0, 0));

            enemyDeckList[i].GetComponent<CardMovement>()._targetTransform = obj.transform;

        }
    }

    // Update is called once per frame
    void Update () {
       /* if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(DamageEnemy(5));
        }*/
    }
}
