using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardGenerator : MonoBehaviour {
    //script loops through excel spreadsheet and seperates cards into their different decks

    //transforms of decks
    private Transform playerDeckTrans, enemyDeckTrans, poisonDeckTrans;

    private int numUniqueCards=11;

    public GameObject Card;

    private GameObject tempObj, cardObj;
    private CardObj cardScript;

    private HandManager handManagerScript;
    private EnemyHandManager enemyHandManagerScript;
    private GameManager gameManagerScript;
    private StatsManager statManagerScript;

    public Sprite PlayerSprite_Front, PlayerSprite_Back, SpiderSprite_Front, SpiderSprite_Back;
    
    //decks
    public List<GameObject> 
        //player deck
        PlayerDeck,
        //enemy decks
        SpiderDeck,
        NagaDeck,
        GemDeck,
        DragonDeck,
        //status effect decks
        PoisonDeck,
        BurnDeck,
        WoundDeck,
        HexDeck;

    // Use this for initialization
    void Start () {
        playerDeckTrans = GameObject.Find("Deck").GetComponent<Transform>();
        enemyDeckTrans = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        poisonDeckTrans = GameObject.Find("PoisonDeck").GetComponent<Transform>();

        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        enemyHandManagerScript = GameObject.Find("GameManager").GetComponent<EnemyHandManager>();

        int player_health = 0;
        int player_cardsInDeck = 0;

        int enemy_health = 0;
        int enemy_cardsInDeck = 0;

        //looping through all unique cards
        for(int i = 1; i < numUniqueCards+1; i++)
        {
            tempObj = (GameObject)Instantiate(Card, Vector3.zero, Quaternion.identity);
            //load data onto card
            cardScript = tempObj.GetComponent<CardObj>();
            Debug.Log("working: " + i.ToString());
            LoadMyData("card_" + i.ToString());

            int numSigils = cardScript.SigilNum;

            //set stats
            if(cardScript.CardType == "player_starting")
            {
                player_health += cardScript.SigilNum * 5;
                player_cardsInDeck += cardScript.NumInDeck;
            }
            else if(cardScript.CardType == "spider")
            {
                enemy_health += cardScript.SigilNum * 5;
                enemy_cardsInDeck += cardScript.NumInDeck;
            }

            //seperate cards into decks
            for (int j=0; j<cardScript.NumInDeck; j++)
            {
                cardObj = (GameObject)Instantiate(tempObj);
                if (cardScript.CardType == "player_starting")
                {
                    //check for Sigils
                    if(numSigils>0)
                    {
                        //put a sigil on the card
                        cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled = true;
                    }

                    cardObj.transform.position = playerDeckTrans.position;
                    cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                    cardObj.GetComponent<SpriteRenderer>().sprite = PlayerSprite_Front;
                    cardObj.GetComponent<CardMovement>().isEnemyCard = false;
                    PlayerDeck.Add(cardObj);
                }
                else if(cardScript.CardType == "spider")
                {
                    //check for Sigils
                    if (numSigils > 0)
                    {
                        //put a sigil on the card
                        cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled = true;
                    }
                    
                    cardObj.transform.position = enemyDeckTrans.position;
                    cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                    cardObj.GetComponent<SpriteRenderer>().sprite = SpiderSprite_Front;
                    cardObj.GetComponent<CardMovement>().isEnemyCard = true;
                    SpiderDeck.Add(cardObj);
                }
                else if(cardScript.CardType == "status")
                {
                    if(cardScript.CardName == "Poison")
                    {
                        cardObj.transform.position = poisonDeckTrans.position;
                        cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                        cardObj.GetComponent<CardMovement>().isEnemyCard = false;
                        PoisonDeck.Add(cardObj);
                    }
                }
            }

            Destroy(tempObj);

        }

        //fill stats
        statManagerScript.SetHealth("player", player_health);
        statManagerScript.SetTotalCards("player", player_cardsInDeck);
        statManagerScript.SetHealth("enemy", enemy_health);
        statManagerScript.SetTotalCards("enemy", enemy_cardsInDeck);

        //Initialise decks in their respective scripts
        handManagerScript.InitialiseCards();
        enemyHandManagerScript.InitialiseCards();

        //start the game
        gameManagerScript.StartGame();
    }



    public void LoadMyData(string cardReference)
    {
        ImportedDataContainer my_container = ImportData.GetContainer(cardReference);

        cardScript.CardName = my_container.GetData("name").ToString();
        cardScript.DiscardCost = my_container.GetData("discard_cost").ToInt();
        cardScript.BurnCost = my_container.GetData("burn_cost").ToInt();
        cardScript.FlavourText = my_container.GetData("flavour_text").ToString();
        cardScript.Attack = my_container.GetData("attack").ToInt();
        cardScript.Defense = my_container.GetData("defense").ToInt();

        cardScript.SigilNum = my_container.GetData("sigil_num").ToInt();

        cardScript.IsOnArrival = my_container.GetData("on_arrival").ToInt();
        cardScript.OA_IsChoice = my_container.GetData("OA_is_choice").ToInt();
        cardScript.OA_NumDeckDrawTop = my_container.GetData("OA_num_deck_draw_top").ToInt();
        cardScript.OA_TempDefenseIncrease = my_container.GetData("OA_temp_defense_increase").ToInt();

        cardScript.IsUndying = my_container.GetData("undying").ToInt();
        cardScript.IsStack = my_container.GetData("stack").ToInt();
        cardScript.IsUntapped = my_container.GetData("untapped").ToInt();

        cardScript.DiscardEffect = my_container.GetData("discard_effect").ToString();
        cardScript.DE_NumDeckDrawRandom = my_container.GetData("DISCARD_num_deck_draw_random").ToInt();
        cardScript.DE_NumFree = my_container.GetData("DISCARD_num_free").ToInt();
        cardScript.DE_IsHighestValAttack = my_container.GetData("DISCARD_is_highest_val_attack").ToInt();
        cardScript.DE_NumPoisons = my_container.GetData("DISCARD_num_poisons").ToInt();
        cardScript.DE_NumDiscardDrawChoice = my_container.GetData("DISCARD_num_discard_draw_choice").ToInt();
        cardScript.DE_NumDeckDrawTop = my_container.GetData("DISCARD_num_deck_draw_top").ToInt();
        cardScript.DE_AddAttack = my_container.GetData("DISCARD_add_attack").ToInt();
        cardScript.DE_SelectDiscardAddAttack = my_container.GetData("DISCARD_select_discard_add_attack").ToInt();

        cardScript.BurnEffect = my_container.GetData("burn_effect").ToString();
        cardScript.BE_NumDeckDrawChoice = my_container.GetData("BURN_num_deck_draw_choice").ToInt();
        cardScript.BE_RegainSigil = my_container.GetData("BURN_regain_sigil").ToInt();
        cardScript.BE_NumWoundsDiscard = my_container.GetData("BURN_num_wounds_discard").ToInt();
        cardScript.BE_NumPoisons = my_container.GetData("BURN_num_poisons").ToInt();
        cardScript.BE_NumDeckDrawTop = my_container.GetData("BURN_num_deck_draw_top").ToInt();
        cardScript.BE_IsOption = my_container.GetData("BURN_is_option").ToInt();
        cardScript.BE_AddDefense = my_container.GetData("BURN_add_defense").ToInt();
        cardScript.BE_AddDefense = my_container.GetData("BURN_add_defense").ToInt();
        cardScript.BE_DrawAttackValOfBurnt = my_container.GetData("BURN_draw_attack_val_of_burnt").ToInt();
        cardScript.BE_HasReaction = my_container.GetData("BURN_has_reaction").ToInt();
        cardScript.BE_DrawDefenseValOfBurnt = my_container.GetData("BURN_draw_defense_val_of_burnt").ToInt();

        cardScript.Image_Name = my_container.GetData("imageid").ToString();
        cardScript.NumInDeck = my_container.GetData("num_in_deck").ToInt();
        cardScript.CardType = my_container.GetData("type").ToString();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
