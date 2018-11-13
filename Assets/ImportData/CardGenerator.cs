using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardGenerator : MonoBehaviour {
    //script loops through excel spreadsheet and seperates cards into their different decks

    //transforms of decks
    private Transform playerDeckTrans, enemyDeckTrans, poisonDeckTrans;

    private int numUniqueCards=20;

    public GameObject Card;

    private GameObject tempObj, cardObj;
    private CardObj cardScript;

    private HandManager handManagerScript;

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
        //status effect decks
        PoisonDeck,
        WoundDeck;


    // Use this for initialization
    void Start () {
        playerDeckTrans = GameObject.Find("Deck").GetComponent<Transform>();
        enemyDeckTrans = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        poisonDeckTrans = GameObject.Find("PoisonDeck").GetComponent<Transform>();

        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();

        InitialiseLevel(2);
       
    }


    public void InitialiseLevel(int level)
    {
        int player_health = 0;
        int player_cardsInDeck = 0;

        int enemy_health = 0;
        int enemy_cardsInDeck = 0;

        //looping through all unique cards
        for (int i = 1; i < numUniqueCards + 1; i++)
        {
            tempObj = (GameObject)Instantiate(Card, Vector3.zero, Quaternion.identity);
            //load data onto card
            cardScript = tempObj.GetComponent<CardObj>();
            Debug.Log("working: " + i.ToString());
            LoadMyData("card_" + i.ToString());

            int numSigils = cardScript.SigilNum;

            //set stats
            if (cardScript.CardType == "player_starting")
            {
            
                if (numSigils > 0)
                    statManagerScript.UpdateSigils("player", numSigils);
                player_health += cardScript.SigilNum * 5;
                player_cardsInDeck += cardScript.NumInDeck;
            }
            else if (level == 1
                && cardScript.CardType == "spider")
            {
            
                if (numSigils > 0)
                    statManagerScript.UpdateSigils("enemy", numSigils);
                enemy_health += cardScript.SigilNum * 5;
                enemy_cardsInDeck += cardScript.NumInDeck;
            }
            else if (level == 2
                && cardScript.CardType == "naga")
            {
               
                if (numSigils > 0)
                    statManagerScript.UpdateSigils("enemy", numSigils);
                enemy_health += cardScript.SigilNum * 5;
                enemy_cardsInDeck += cardScript.NumInDeck;
            }

            //seperate cards into decks
            for (int j = 0; j < cardScript.NumInDeck; j++)
            {
                cardObj = (GameObject)Instantiate(tempObj);
                if (cardScript.CardType == "player_starting")
                {
                    //check for Sigils
                    if (numSigils > 0)
                    {
                        //put a sigil on the card
                        cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled = true;
                        numSigils--;
                    }

                    cardObj.transform.position = playerDeckTrans.position;
                    cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                    cardObj.GetComponent<SpriteRenderer>().sprite = PlayerSprite_Front;
                    cardObj.GetComponent<CardMovement>().isEnemyCard = false;
                    PlayerDeck.Add(cardObj);
                }
                else if (level == 1 && cardScript.CardType == "spider")
                {
                    //check for Sigils
                    if (numSigils > 0)
                    {
                        //put a sigil on the card
                        cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled = true;
                        numSigils--;
                    }

                    cardObj.transform.position = enemyDeckTrans.position;
                    cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                    cardObj.GetComponent<SpriteRenderer>().sprite = SpiderSprite_Front;
                    cardObj.GetComponent<CardMovement>().isEnemyCard = true;
                    SpiderDeck.Add(cardObj);
                }
                else if(level==2 && cardScript.CardType=="naga")
                {
                   
                    //check for Sigils
                    if (numSigils > 0)
                    {
                        //put a sigil on the card
                        cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled = true;
                        numSigils--;
                    }

                    cardObj.transform.position = enemyDeckTrans.position;
                    cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                    cardObj.GetComponent<SpriteRenderer>().sprite = SpiderSprite_Front;
                    cardObj.GetComponent<CardMovement>().isEnemyCard = true;
                    NagaDeck.Add(cardObj);
                }
                else if (cardScript.CardType == "status")
                {
                    if (cardScript.CardName == "Poison")
                    {
                        cardObj.transform.position = poisonDeckTrans.position;
                        cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                        cardObj.GetComponent<CardMovement>().isEnemyCard = false;
                        PoisonDeck.Add(cardObj);
                    }
                }
                else
                {
                    print(cardScript.CardName+ " :" + cardScript.CardType+" :" +level.ToString());
                    cardObj.transform.position = new Vector3(1000,1000,1000);
                    cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);
                }
            }

            Destroy(tempObj);

        }

        //fill stats
        statManagerScript.SetHealth("player", player_health);
        statManagerScript.SetTotalCards("player", player_cardsInDeck,0);
        statManagerScript.SetHealth("enemy", enemy_health);
        statManagerScript.SetTotalCards("enemy", enemy_cardsInDeck,0);

        //Initialise decks in their respective scripts
        handManagerScript.InitialiseCards(level);

        //start the game
        gameManagerScript.StartGame(2);
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

        cardScript.DiscardEffect = my_container.GetData("discard_effect").ToString();

        cardScript.BurnEffect = my_container.GetData("burn_effect").ToString();

        cardScript.Image_Name = my_container.GetData("imageid").ToString();
        cardScript.NumInDeck = my_container.GetData("num_in_deck").ToInt();
        cardScript.CardType = my_container.GetData("type").ToString();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
