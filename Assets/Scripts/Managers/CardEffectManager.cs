using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardEffectManager : MonoBehaviour {
    private StatsManager statManagerScript;
    private HandManager handManagerScript;
    private AreaManager areaManagerScript;

    private Transform tempDisplayPlayer;
    private Transform playerDeckTrans;
	// Use this for initialization
	void Start () {
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();

        tempDisplayPlayer = GameObject.Find("PlayerTempDisplay").GetComponent<Transform>();
        playerDeckTrans = GameObject.Find("Deck").GetComponent<Transform>();

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

        //DEFEND
        if(statManagerScript.phase_player == "reaction")
        {
            statManagerScript.UpdateDefense("player", Card.Defense);
            
        }


        //play card effects
        if(Card.CardName=="Advanced Guard")
        {
            //Reaction - if played after a card with a defense value higher than 1, add +2 defense to this card
            GameObject lastPlayedCard = areaManagerScript.player_PlayCardList[areaManagerScript.player_PlayCardList.Count - 1];
            if(statManagerScript.phase_player == "reaction"
                && lastPlayedCard.GetComponent<CardObj>().Defense > 1)
            {
                //Card.Defense += 2;
                playedCard.transform.Find("DefenseCost").GetComponent<TextMeshPro>().text = Card.Defense.ToString();
                statManagerScript.UpdateDefense("player", 2);
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
        }
        else if (Card.CardName == "Pact of Maggots")
        {
            //Undying: at the end of a turn shuffle your burn pile and draw one card from it.

        }
        else if(Card.CardName == "Health Potion")
        {
            //Recover 5 burnt cards. This card is burnt immediately after use

        }
        else if(Card.CardName == "Eternal Will")
        {
            //Reaction: You and your opponent gather and shuffle your discard pile, hand and deck and draw cards equal to your previous hadn count. Draw 1 card.

        }

    }

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
            cardObj.GetComponent<CardMovement>().PlayPlayerCard();
        }
    }

    public void PlayBurn(GameObject playedCard)
    {
        CardObj Card = playedCard.GetComponent<CardObj>();

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
            //Draw two random cards from your deck
            int rand = Random.Range(0, handManagerScript.playerDeckList.Count - 1);
            StartCoroutine(handManagerScript.DrawCards(2, "player"));
        }
        else if (Card.CardName == "Second Wind")
        {
            //Action - Draw your burnt card's attack value x2 from your deck.
            //Reaction - Draw your burnt card's defence value x2 from your deck 
        }
        else if (Card.CardName == "Fireball")
        {
            //Burn the top two cards of your opponents deck
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
