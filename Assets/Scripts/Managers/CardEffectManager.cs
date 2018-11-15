using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardEffectManager : MonoBehaviour {
    private StatsManager statManagerScript;
    private HandManager handManagerScript;
    private AreaManager areaManagerScript;

    private Transform tempDisplayPlayer;
	// Use this for initialization
	void Start () {
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();

        tempDisplayPlayer = GameObject.Find("PlayerTempDisplay").GetComponent<Transform>();
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
        if (Card.Attack > 0 && statManagerScript.phase_player == "action")
        {
            statManagerScript.UpdateAttack("player", Card.Attack);
        }

        //DEFEND
        if (Card.Defense > 0 && statManagerScript.phase_player=="reaction")
        {
            statManagerScript.UpdateDefense("player", Card.Defense);
        }

        //play card effects
        if(Card.CardName=="Advanced Guard")
        {
            //Action - Draw 1 card 
            //Reaction - gain +2 def to this card until it goes to the discard
            if (statManagerScript.phase_player == "action")
            {
                StartCoroutine(handManagerScript.DrawCards(1, "player"));
            }
            else if(statManagerScript.phase_player == "reaction")
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
            attack += areaManagerScript.player_DiscardCardList[areaManagerScript.player_DiscardCardList.Count - 1].GetComponent<CardObj>().BurnCost;

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

        }


        if (isBurn)
        {
            //play effects with burn
            if(Card.CardName=="Focused Strike")
            {
                //add 3 attack to the card
                playedCard.transform.Find("AttackCost").GetComponent<TextMeshPro>().text = (Card.Attack + 3).ToString();
                statManagerScript.UpdateAttack("player", 3);
            }
            else if(Card.CardName == "Advanced Guard")
            {
                //draw 4 cards
                StartCoroutine(handManagerScript.DrawCards(4, "player"));
            }
            else if (Card.CardName == "Naga Red Eye Gem")
            {
                //Regain a sigil card from the burnt pile

                //loop through the trash pile and return the first sigil card encountered to the player's hand
            }
            else if(Card.CardName == "Lucky Charm")
            {
                //Draw one card of your choice from your deck
            }
            else if(Card.CardName=="Second Wind")
            {
                //Action - Draw your burnt card's attack value x2 from your deck.
                //Reaction - Draw your burnt card's defence value x2 from your deck 
            }
            else if (Card.CardName == "Fireball")
            {
                //Burn the top two cards of your opponents deck
            }
        }
    }

    private void PlayBurn(GameObject cardObj)
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
