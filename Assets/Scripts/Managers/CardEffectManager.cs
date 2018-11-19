using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardEffectManager : MonoBehaviour {
    private StatsManager statManagerScript;
    private HandManager handManagerScript;
    private AreaManager areaManagerScript;
    private CardGenerator cardGenScript;

    private Transform tempDisplayPlayer;
    private Transform playerDeckTrans;

    private Transform  enemyDeck;
    private Transform  enemyTempDisplay;
    private Transform playerTrash, enemyTrash;



    List<GameObject> playerTempDeck = new List<GameObject>();
    [SerializeField]
    List<GameObject> enemyTempDeck = new List<GameObject>();
    // Use this for initialization
    void Start () {
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        cardGenScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();

        tempDisplayPlayer = GameObject.Find("PlayerTempDisplay").GetComponent<Transform>();
        playerDeckTrans = GameObject.Find("Deck").GetComponent<Transform>();
        enemyDeck = GameObject.Find("EnemyDeck").GetComponent<Transform>();

        playerTrash = GameObject.Find("Trash").GetComponent<Transform>();
        enemyTrash = GameObject.Find("Trash").GetComponent<Transform>();
    }

    //play a card from the hand
    //check for all card stats and abilities
    public void PlayCard(GameObject playedCard, bool isBurn)
    {
        //retrieve card data
        CardObj Card = playedCard.GetComponent<CardObj>();

        //take off of the discard pool
        statManagerScript.UpdateDiscard("player", -Card.DiscardCost);

        if (isBurn && Card.BurnCost>0)
        {
            //take off of the burn pool
            statManagerScript.UpdateBurn("player", -Card.BurnCost);

            //give the card a burn halo whilst in play area
            playedCard.transform.Find("BurnBorder").GetComponent<SpriteRenderer>().enabled = true;
        }

        //check opponent

        //analyse card data

        //check if action or reaction phase

        //ATTACK
        if(statManagerScript.phase_player == "action")
        {
            statManagerScript.UpdateAttack("player", Card.Attack);
            
        }

        if (statManagerScript.phase_player == "waiting")
        {
            print("waiting, added defense");
            statManagerScript.UpdateDefense("player", Card.Defense);
        }

        //DEFEND
        if(statManagerScript.phase_player == "reaction")
        {
            statManagerScript.UpdateDefense("player", Card.Defense);
            
        }


        //play card effects
        if(Card.CardName=="Advanced Guard")
        {
            //Reaction - if played after a card with a defense value higher than 1, add +2 defense to this card
            if (areaManagerScript.player_PlayCardList.Count > 1)
            {
                GameObject lastPlayedCard = areaManagerScript.player_PlayCardList[areaManagerScript.player_PlayCardList.Count - 2];
                if (statManagerScript.phase_player == "reaction"
                    && lastPlayedCard.GetComponent<CardObj>().Defense > 1)
                {
                    //Card.Defense += 2;
                    playedCard.transform.Find("DefenseCost").GetComponent<TextMeshPro>().text = (Card.Defense + 2).ToString();
                    statManagerScript.UpdateDefense("player", 2);
                }
            }
            

        }
        else if(Card.CardName=="Naga Red Eye Gem")
        {
            //Add the attack and defence values as well as the discard and burn costs of the last card that you discarded to this cards attack
            int attack = 0;
            attack += areaManagerScript.player_DiscardCardList[areaManagerScript.player_DiscardCardList.Count - 1].GetComponent<CardObj>().Attack;
            attack += areaManagerScript.player_DiscardCardList[areaManagerScript.player_DiscardCardList.Count - 1].GetComponent<CardObj>().Defense;
            attack += areaManagerScript.player_DiscardCardList[areaManagerScript.player_DiscardCardList.Count - 1].GetComponent<CardObj>().DiscardCost;

            //Card.Attack += attack;
            playedCard.transform.Find("AttackCost").GetComponent<TextMeshPro>().text = (Card.Attack + attack).ToString();
            statManagerScript.UpdateAttack("player", attack);
        }
        else if (Card.CardName == "Inner Strength")
        {
            
            //gain 2x inner strength cards
            //Instantiate(Card, tempDisplayPlayer.transform.position, Quaternion.Euler(90, 0, 0));
            GameObject newCard  = Instantiate(playedCard, new Vector3(1000,1000,1000), Quaternion.Euler(90, 0, 0));
            int rand = Random.Range(0, handManagerScript.playerDeckList.Count - 1);

            handManagerScript.playerDeckList.Insert(rand, newCard);
            StartCoroutine(areaManagerScript.TempDisplay(newCard,tempDisplayPlayer,playerDeckTrans));
            statManagerScript.UpdateCardsInDeck("player", 1, 1);

            newCard = Instantiate(playedCard, new Vector3(1000, 1000, 1000), Quaternion.Euler(90, 0, 0));
           

            handManagerScript.playerDeckList.Insert(rand, newCard);
            StartCoroutine(areaManagerScript.TempDisplay(newCard, tempDisplayPlayer, playerDeckTrans));
            statManagerScript.UpdateCardsInDeck("player", 1, 1);

            for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
            {
                handManagerScript.playerHandList[i].GetComponent<CardMovement>().isPlayed = false;
                handManagerScript.playerHandList[i].GetComponent<CardMovement>().isInHand = true;
            }
        }
        else if (Card.CardName == "Pact of Maggots")
        {
            //Undying: at the end of a turn shuffle your burn pile and draw one card from it.

        }
        else if(Card.CardName == "Symbol of Faith")
        {
            symbolOfFaith();
           

        }
        else if(Card.CardName == "Eternal Will")
        {
            print("ETERNAL WILL");
            eternalWill();
           


        }
        else if (Card.CardName == "Lucky Charm")
        {
            //Draw two random cards from your deck
            int rand = Random.Range(0, handManagerScript.playerDeckList.Count - 1);
            StartCoroutine(handManagerScript.DrawCards(2, "player"));
        }
        else if(Card.CardName == "Second Wind")
        {
            //draw 1 card
            StartCoroutine(handManagerScript.DrawCards(1, "player"));
        }

    }

    void symbolOfFaith()
    {
        bool isfound = false;
        print("SYMBOL OF FAITH");
        //Recover 3 random cards from your discard pile into your hand. 
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(handManagerScript.DrawDiscard(1, "player"));
            handManagerScript.Shuffle(ref areaManagerScript.enemy_DiscardCardList);
        }

        //Discard 1 status effect from your deck. If you have none regain a card from the flames and discard it.
        for (int i = 0; i < handManagerScript.playerDeckList.Count; i ++)
        {
           
            if(!isfound && handManagerScript.playerDeckList[i].GetComponent<CardObj>().CardName == "Wound" || handManagerScript.playerDeckList[i].GetComponent<CardObj>().CardName == "Poison")
            {
                isfound = true;
                var tempCard = handManagerScript.playerDeckList[i];
                StartCoroutine(areaManagerScript.TempDisplay(tempCard, tempDisplayPlayer, playerTrash));
               
                areaManagerScript.Call_TrashCard(tempCard, "player");
                statManagerScript.UpdateCardsInDeck("player", -1, 1);
                handManagerScript.playerDeckList.RemoveAt(i);
            }
        }
        if(!isfound)
        {
            StartCoroutine(handManagerScript.DrawTrash(1, "player"));
        }

        //Reset bools for cards in hand
        for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
        {
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isPlayed = false;
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isInHand = true;
        }
        handManagerScript.ReorderHandLayers("player");
    }

    void eternalWill()
    {
        int enemyHandSize = handManagerScript.enemyHandlist.Count;
        int playerHandSize = handManagerScript.playerHandList.Count;
        //Reaction: You and your opponent gather and shuffle your discard pile, hand and deck and draw cards equal to your previous hadn count. Draw 1 card.
        if (statManagerScript.phase_player == "reaction")
        {
            //For enemy

            print("Eternal Will");
            //Add deck to temp deck
            for (int i = 0; i < handManagerScript.enemyDeckList.Count; i++)
            {
                //print("i is " + i);
                enemyTempDeck.Add(handManagerScript.enemyDeckList[i]);

            }
            //discard hand
            for (int i = 0; i < handManagerScript.enemyHandlist.Count; i++)
            {
                print("num discarded:  " + handManagerScript.enemyHandlist.Count);
              
                 handManagerScript.enemyHandlist[i].GetComponent<CardMovement>().DiscardEnemyCard();               
            }
            //Add discard list to tempdeck
            for (int i = 0; i < areaManagerScript.enemy_DiscardCardList.Count; i++)
            {
                enemyTempDeck.Add(areaManagerScript.enemy_DiscardCardList[i]);

            }

            // handManagerScript.enemyDeckList[i] = new List<GameObject>();
            //Recreate Deck
            for (int i = 0; i < enemyTempDeck.Count; i++)
            {

                //print("MAKE CARD");
                var enemyTempCard = Instantiate(enemyTempDeck[i], enemyDeck.position, Quaternion.Euler(90, 0, 0));
                handManagerScript.enemyDeckList.Add(enemyTempCard);


            }

          
            //FOR PLAYER
            //Add deck to tempdeck
            for (int i = 0; i < handManagerScript.playerDeckList.Count; i++)
            {

                playerTempDeck.Add(handManagerScript.playerDeckList[i]);

            }
   

            //Discard Hand
            for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
            {
                //playerTempDeck.Add(handManagerScript.playerHandList[i]);
                handManagerScript.playerHandList[i].GetComponent<CardMovement>().isInHand = false;
                handManagerScript.playerHandList[i].GetComponent<CardMovement>().isPlayed = true;
                areaManagerScript.Call_DiscardCard(handManagerScript.playerHandList[i], "player");
           

            }
            //Add discard to tempdeck
            for (int i = 0; i < areaManagerScript.player_DiscardCardList.Count; i++)
            {

                playerTempDeck.Add(areaManagerScript.player_DiscardCardList[i]);

            }

            // handManagerScript.enemyDeckList[i] = new List<GameObject>();
            //Generate new deck
            for (int i = 0; i < enemyTempDeck.Count; i++)
            {

                print("MAKE CARD");
                var playerTempCard = Instantiate(enemyTempDeck[i], enemyDeck.position, Quaternion.Euler(90, 0, 0));
                handManagerScript.playerDeckList.Add(playerTempCard);


            }
            print("Enemy Handsize is:" + enemyHandSize);
            handManagerScript.Shuffle(ref handManagerScript.enemyDeckList);
            handManagerScript.Shuffle(ref handManagerScript.playerDeckList);

            StartCoroutine(handManagerScript.DrawCards(enemyHandSize, "enemy"));
            StartCoroutine(handManagerScript.DrawCards(playerHandSize, "player"));

            //Reset bools in enemy hand
            for (int i = 0; i < handManagerScript.enemyHandlist.Count; i++)
            {
                handManagerScript.enemyHandlist[i].GetComponent<CardMovement>().isPlayed = false;
                handManagerScript.enemyHandlist[i].GetComponent<CardMovement>().isInHand = true;
            }

            //Reset bools in player hand
            for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
            {
                handManagerScript.playerHandList[i].GetComponent<CardMovement>().isPlayed = false;
                handManagerScript.playerHandList[i].GetComponent<CardMovement>().isInHand = true;
            }
        }
    }

    //UNDYING
    public int PlayUndyingPoisonEffects()
    {

        //POISON UNDYING EFFECT
        //loop through discard and count number of poisons
        int numPoisons = 0;
        for(int i=0; i<areaManagerScript.player_DiscardCardList.Count; i++)
        {
            if (areaManagerScript.player_DiscardCardList[i].GetComponent<CardObj>().CardName == "Poison")
            {
                numPoisons++;
            }
        }

        //discard a card from bottom of player's deck for every poison in discard
        int numDiscards = numPoisons / 2;

        StartCoroutine(handManagerScript.DiscardFromBottomOfDeck(numDiscards));

        return numDiscards;
    }

    public void PactOfMaggot()
    {
        //StartCoroutine(handManagerScript.DrawCards(1, "player"));
        if(areaManagerScript.player_DiscardCardList.Count > 0)
        {
            handManagerScript.Shuffle(ref areaManagerScript.player_DiscardCardList);
            StartCoroutine(handManagerScript.DrawDiscard(1, "player"));
        }

        //Reset Bools for player cards
        for (int i = 0; i < handManagerScript.playerHandList.Count; i++)
        {
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isPlayed = false;
            handManagerScript.playerHandList[i].GetComponent<CardMovement>().isInHand = true;
        }
    }

    //ON ARRIVAL
    public void PlayOnArrivalEffects(GameObject cardObj)
    {
        //retrieve card data
        CardObj Card = cardObj.GetComponent<CardObj>();

        if (Card.CardName == "Advanced Guard")
        {
            //draw 1 card
            StartCoroutine(handManagerScript.DrawCards(1, "player"));
        }
        if(Card.CardName == "Poison")
        {
            statManagerScript.UpdateNumStatusCards(-1, 0);
            cardObj.GetComponent<CardMovement>().PlayPlayerCard();
        }
        if(Card.CardName =="Wound")
        {
            statManagerScript.UpdateNumStatusCards(-1, 0);
            cardObj.GetComponent<CardMovement>().PlayPlayerCard();
            if (statManagerScript.phase_player== "Action")
            {
                statManagerScript.UpdateAttack("player",  Card.Attack);
            }
            else
            {
                statManagerScript.UpdateDefense("player",  Card.Defense);
            }



            }
    }

    public void PlayBurn(GameObject playedCard)
    {
        CardObj Card = playedCard.GetComponent<CardObj>();

        //subtract from the burn total
        int burnVal = playedCard.GetComponent<CardObj>().BurnCost;
        statManagerScript.UpdateBurn("player", -burnVal);

        //play effects with burn
        if (Card.CardName == "Focused Strike")
        {
            //add 3 attack to the card
            playedCard.transform.Find("AttackCost").GetComponent<TextMeshPro>().text = (Card.Attack + 3).ToString();
            statManagerScript.UpdateAttack("player", 3);
        }
        else if (Card.CardName == "Advanced Guard")
        {
            //draw 4 cards
            StartCoroutine(handManagerScript.DrawCards(4, "player"));
        }
        else if (Card.CardName == "Naga Red Eye Gem")
        {
            //Regain a sigil card from the burnt pile

            //loop through the trash pile and return the first sigil card encountered to the player's hand
            for(int i = 0; i< areaManagerScript.player_TrashCardList.Count; i++)
            {
                //if the sigil on the card is enabled and it is the player's card
                if (areaManagerScript.player_TrashCardList[i].transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled)
                {
                    //add the card to the player's hand
                    StartCoroutine(areaManagerScript.TempDisplay(areaManagerScript.player_TrashCardList[i], tempDisplayPlayer, playerDeckTrans));
                    handManagerScript.playerDeckList.Add(areaManagerScript.player_TrashCardList[i]);
                    
                    statManagerScript.UpdateCardsInDeck("player", 1, 1);

                    //remove the card from the trash pile
                    areaManagerScript.player_TrashCardList.RemoveAt(i);
                }
            }
            
        }
        else if (Card.CardName == "Lucky Charm")
        {
//          print("LUCKY CHARM");
            GameObject highestValCard = handManagerScript.playerDeckList[0];
            int index = 0;
            //Draw highest card based on phase
            if (statManagerScript.phase_player == "action")
            {
                
                for (int i = 0; i < handManagerScript.playerDeckList.Count;i++)
                {
                    print("LUCKY CHARM");
                    if (handManagerScript.playerDeckList[i].GetComponent<CardObj>().Attack > highestValCard.GetComponent<CardObj>().Attack)
                    {
                        highestValCard = handManagerScript.playerDeckList[i];
                        index = i;
                        print("SWAP");
                    }
                }
            }
            //Reaction - Draw your burnt card's defence value x2 from your deck 
            else if (statManagerScript.phase_player == "reaction")
            {
                for (int i = 0; i < handManagerScript.playerDeckList.Count; i++)
                {
                    if (handManagerScript.playerDeckList[i].GetComponent<CardObj>().Defense > highestValCard.GetComponent<CardObj>().Defense)
                    {
                        highestValCard = handManagerScript.playerDeckList[i];
                        index = i;
                    }
                }
            }
         
            //statManagerScript.UpdateCardsInDeck("player",-1,1);
            handManagerScript.playerDeckList.Add(handManagerScript.playerDeckList[index]);
            StartCoroutine(handManagerScript.DrawCards(1, "player"));
            handManagerScript.playerDeckList.RemoveAt(index);
            // StartCoroutine(areaManagerScript.TempDisplay(highestValCard, tempDisplayPlayer, tempDisplayPlayer));

            //handManagerScript.playerHandList.Add(highestValCard);
            handManagerScript.ReorderHandLayers("player");
        }
        else if (Card.CardName == "Second Wind")
        {
           
            //Action - Draw your burnt card's attack value x2 from your deck.
            if (statManagerScript.phase_player == "action")
            {
                var tempCard = areaManagerScript.player_TrashCardList[areaManagerScript.player_TrashCardList.Count - 1];
                StartCoroutine(handManagerScript.DrawCards(tempCard.GetComponent<CardObj>().Attack * 2, "player"));
            }
            //Reaction - Draw your burnt card's defence value x2 from your deck 
            else if (statManagerScript.phase_player == "reaction")
            {
                var tempCard = areaManagerScript.player_TrashCardList[areaManagerScript.player_TrashCardList.Count - 1];
                StartCoroutine(handManagerScript.DrawCards(tempCard.GetComponent<CardObj>().Defense * 2, "player"));
                //print("Draw " + tempCard.GetComponent<CardObj>().Defense );
            }

        }
        else if (Card.CardName == "Fireball")
        {
            print("FIREBALL");
            //var tempCard = areaManagerScript.enemy_DeckCardList[areaManagerScript.enemy_DeckCardList.Count - 1];
            StartCoroutine(handManagerScript.Call_TakeDamage(2, "enemy"));
            //tempCard = areaManagerScript.enemy_DeckCardList[areaManagerScript.enemy_DeckCardList.Count - 1];
            //tempCard.GetComponent<CardMovement>().TrashEnemyCard();*/


        }

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
