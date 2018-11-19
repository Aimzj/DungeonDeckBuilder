using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardGenerator : MonoBehaviour {
    //script loops through excel spreadsheet and seperates cards into their different decks

    //transforms of decks
    public Transform playerDeckTrans, enemyDeckTrans, poisonDeckTrans;
    public Transform extraCardsTrans;

    private int numUniqueCards=22;

    public GameObject Card;

    private GameObject tempObj, cardObj;
    private CardObj cardScript;

    private HandManager handManagerScript;

    private GameManager gameManagerScript;
    private StatsManager statManagerScript;
    private GeneratePacks generatePackScript;
    private Menu menuScript;

    public Sprite PlayerSprite_Front, PlayerSprite_Back, SpiderSprite_Front, SpiderSprite_Back, StatusSprite, NagaSprite;

    //FOR TUTORIAL
    private SetDecks setTutDecks;

    private Canvas packCanvas;

    //decks
    public List<GameObject>
        //player deck
        PlayerDeck,
        //enemy decks
        SpiderDeck,
        NagaDeck,
        //status effect decks
        PoisonDeck,
        WoundDeck,
        //packs
        ReinforcementPack1,
        ReinforcementPack2,
        HealingPack,
        AshPack,
        NecromancerPack,
        ArcanePack,
        PrimusPack;

    //Images on cards
    public Sprite strike, focStrike, guard, advGuard, nagaREG, luckCharm, secondWind, innerStrength, pactOfMaggots,
        firball, eternalWill, innerStrengthEcho, poison, wound, bite, skitter, lethargy, serpentScale, crushingBlow, callOfTheDeep, eldritchOath, symbolOfFaith;

    //Tutorial packs
    public List<GameObject> PlayerTutDeck, EnemyTutDeck;

    public List<GameObject> chosenPack1, chosenPack2;

    private int numPacks;

    public int level;

    // Use this for initialization
    void Start () {
        numPacks = 0;

        playerDeckTrans = GameObject.Find("Deck").GetComponent<Transform>();
        enemyDeckTrans = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        poisonDeckTrans = GameObject.Find("PoisonDeck").GetComponent<Transform>();

        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        menuScript = GameObject.Find("GameManager").GetComponent<Menu>();

        generatePackScript = GameObject.Find("PackManager").GetComponent<GeneratePacks>();
        packCanvas = GameObject.Find("Pack_Canvas").GetComponent<Canvas>();

        //For tutorial
        //setTutDecks = GameObject.Find("GameManager").GetComponent<SetDecks>();
        EnemyTutDeck = new List<GameObject>();
        PlayerTutDeck = new List<GameObject>();

        level = PlayerPrefs.GetInt("Level");

        InitialiseLevel(level);
    }

    private void InstantiateCard(ref GameObject temp, bool isKindling, ref List<GameObject> pack, int numCards, bool isSigil)
    {
        for(int i = 0; i<numCards; i++)
        {
            cardObj = (GameObject)Instantiate(tempObj);
            cardObj.transform.position = playerDeckTrans.position;
            cardObj.transform.rotation = Quaternion.Euler(90, 90, 0);

            if (isKindling)
            {
                cardObj.GetComponent<CardMovement>().isKindling = true;
                cardObj.transform.Find("Kindling").GetComponent<SpriteRenderer>().enabled = true;
            }

            if (isSigil)
            {
                cardObj.transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled = true;
            }

            pack.Add(cardObj);
        }
        
    }

    private void SetImage(ref GameObject card)
    {
        string name = card.GetComponent<CardObj>().CardName;
        if (name == "Strike")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = strike;
        else if(name == "Focused Strike")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = focStrike;
        else if (name == "Advanced Guard")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = advGuard;
        else if (name == "Naga Red Eye Gem")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = nagaREG;
        else if (name == "Guard")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = guard;
        else if (name == "Lucky Charm")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = luckCharm;
        else if (name == "Second Wind")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = secondWind;
        else if (name == "Inner Strength")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = innerStrength;
        else if (name == "Inner Strength (Echo)")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = innerStrengthEcho;
        else if (name == "Pact of Maggots")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = pactOfMaggots;
        else if (name == "Fireball")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = firball;
        else if (name == "Eternal Will")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = eternalWill;
        else if (name == "Symbol of Faith")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = symbolOfFaith;
        else if (name == "Poison")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = poison;
        else if (name == "Wound")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = wound;
        else if (name == "Skitter")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = skitter;
        else if (name == "Bite")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = bite;
        else if (name == "Lethargy")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = lethargy;
        else if (name == "Serpent's Scale")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = serpentScale;
        else if (name == "Crushing Blow")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = crushingBlow;
        else if (name == "Call of the Deep")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = callOfTheDeep;
        else if (name == "Eldritch Oath")
            card.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = eldritchOath;
    }

    public void InitialiseLevel(int level)
    {
        int player_health = 0;
        int player_cardsInDeck = 0;

        int enemy_health = 0;
        int enemy_cardsInDeck = 0;

        if(level == 0)
        {
            player_health = 10;
            enemy_health = 10;
            
        }
        //looping through all unique cardss
        for (int i = 1; i < numUniqueCards + 1; i++)
        {
            tempObj = (GameObject)Instantiate(Card, Vector3.zero, Quaternion.identity);
            //load data onto card
            cardScript = tempObj.GetComponent<CardObj>();
            LoadMyData("card_" + i.ToString());

            int numSigils = cardScript.SigilNum;
            int numKindling = cardScript.Kindling;

            //set stats

            SetImage(ref tempObj);

            if (cardScript.CardType == "player_starting" && level != 0)
            {

                if (numSigils > 0)
                    statManagerScript.UpdateSigils("player", numSigils);
                if (numKindling > 0)
                    statManagerScript.UpdateKindling("player", numKindling,numKindling);
                player_health += cardScript.SigilNum * 5;
                player_cardsInDeck += cardScript.NumInDeck;
            }
            else if (level == 0)
            {
                statManagerScript.UpdateSigils("enemy", 1);
                statManagerScript.UpdateKindling("enemy", 0,0);
                statManagerScript.UpdateSigils("player", 1);
                statManagerScript.UpdateKindling("player", 1,1);
                enemy_health = 10;
                enemy_cardsInDeck = 10;
                player_health = 10;
                player_cardsInDeck = 10;
            }
            else if (level == 1
                && cardScript.CardType == "spider")
            {
            
                if (numSigils > 0)
                    statManagerScript.UpdateSigils("enemy", numSigils);
                if (numKindling > 0)
                    statManagerScript.UpdateKindling("enemy", numKindling,numKindling);
                enemy_health += cardScript.SigilNum * 5;
                enemy_cardsInDeck += cardScript.NumInDeck;
            }
            else if (level == 2
                && cardScript.CardType == "naga")
            {
               
                if (numSigils > 0)
                    statManagerScript.UpdateSigils("enemy", numSigils);
                if (numKindling > 0)
                    statManagerScript.UpdateKindling("enemy", numKindling,numKindling);
                enemy_health += cardScript.SigilNum * 5;
                enemy_cardsInDeck += cardScript.NumInDeck;
            }

            //create card packs
            if (cardScript.CardType.Contains("player"))
            {
                //cardObj = (GameObject)Instantiate(tempObj);
                if (cardScript.CardName=="Advanced Guard")
                {
                    //x2 reinforcement1
                    InstantiateCard(ref tempObj, false, ref ReinforcementPack1,2, false);

                    //x1 healing pack
                    InstantiateCard(ref tempObj, false, ref HealingPack, 1, false);

                    //x1 primus (kindled)
                    InstantiateCard(ref tempObj,  true, ref PrimusPack, 1, false);

                    //TUTORIAL
                    if (level == 0)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            //insert 2 
                            cardObj = (GameObject)Instantiate(tempObj, enemyDeckTrans.position, Quaternion.Euler(enemyDeckTrans.eulerAngles));
                            cardObj.GetComponent<CardMovement>().isEnemyCard = true;
                            EnemyTutDeck.Insert(9, cardObj);
                        }
                    }
                    
                }
                if (cardScript.CardName == "Guard")
                {
                    //x1 reinforcement1
                    InstantiateCard(ref tempObj, false, ref ReinforcementPack1,1, false);

                    //x1 reinforcement2
                    InstantiateCard(ref tempObj, false, ref ReinforcementPack2, 1, false);

                    //x1 healing pack
                    InstantiateCard(ref tempObj, false, ref HealingPack, 1, false);

                    //x2 Necromancer
                    InstantiateCard(ref tempObj, false, ref NecromancerPack, 2, false);

                    //x4 arcane (4 kindled)
                    InstantiateCard(ref tempObj, true, ref ArcanePack, 4, false);

                    //x1 primus (kindled)
                    InstantiateCard(ref tempObj, true, ref PrimusPack, 1, false);

                    //TUTORIAL
                    if (level == 0)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            //insert 2 
                            cardObj = (GameObject)Instantiate(tempObj, playerDeckTrans.position, Quaternion.Euler(playerDeckTrans.eulerAngles));
                            PlayerTutDeck.Insert(12, cardObj);
                        }
                    }
                    
                }
                if(cardScript.CardName == "Lucky Charm")
                {
                    //x2 reinforcement1
                    InstantiateCard(ref tempObj, false, ref ReinforcementPack1,2, false);

                    //TUTORIAL
                    if (level == 0)
                    {
                            cardObj = (GameObject)Instantiate(tempObj);
                            cardObj.transform.position = playerDeckTrans.position;
                            cardObj.transform.rotation = playerDeckTrans.rotation;
                            PlayerTutDeck.Add(cardObj);
                    }
                    
                }
                if(cardScript.CardName == "Fireball")
                {
                    //x1 ash
                    InstantiateCard(ref tempObj, false, ref AshPack,1, true);
                }
                if(cardScript.CardName == "Focused Strike")
                {
                    //x2 reinforcement pack 2
                    InstantiateCard(ref tempObj, false, ref ReinforcementPack2,2, false);

                    //x1 ash
                    InstantiateCard(ref tempObj, false, ref AshPack,1, false);

                    //x1 primus
                    InstantiateCard(ref tempObj, false, ref PrimusPack, 1, false);

                    //TUTORIAL
                    if (level == 0)
                    {
                        for (int k = 0; k < 7; k++)
                        {
                            //add 7 to the front
                            cardObj = (GameObject)Instantiate(tempObj, playerDeckTrans.position, Quaternion.Euler(playerDeckTrans.eulerAngles));
                            PlayerTutDeck.Insert(0, cardObj);
                        }
                    }
                    
                }
                if(cardScript.CardName == "Strike")
                {
                    //x3 ash (1 kindling)
                    InstantiateCard(ref tempObj, true, ref AshPack,1, false);
                    InstantiateCard(ref tempObj, false, ref AshPack,2, false);

                    //x1 reinforcement2
                    InstantiateCard(ref tempObj, false, ref ReinforcementPack2, 1, false);

                    //x2 healing (2 kindled)
                    InstantiateCard(ref tempObj, true, ref HealingPack, 2, false);

                    //x2 necromancer (2 kindled)
                    InstantiateCard(ref tempObj, true, ref NecromancerPack, 2, false);

                    //x1 primus
                    InstantiateCard(ref tempObj, false, ref PrimusPack, 1, false);

                    //TUTORIAL
                    if (level == 0)
                    {
                        //player
                        for (int k = 0; k < 2; k++)
                        {
                            //add 2 after lucky charm
                            cardObj = (GameObject)Instantiate(tempObj, playerDeckTrans.position, Quaternion.Euler(playerDeckTrans.eulerAngles));
                            PlayerTutDeck.Add(cardObj);
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            //add 3 before lucky charm
                            cardObj = (GameObject)Instantiate(tempObj, playerDeckTrans.position, Quaternion.Euler(playerDeckTrans.eulerAngles));
                            PlayerTutDeck.Insert(0, cardObj);
                        }
                        //enemy
                        for (int k = 0; k < 13; k++)
                        {
                            //add 13 Strikes
                            cardObj = (GameObject)Instantiate(tempObj, enemyDeckTrans.position, Quaternion.Euler(enemyDeckTrans.eulerAngles));
                            cardObj.GetComponent<CardMovement>().isEnemyCard = true;
                            EnemyTutDeck.Add(cardObj);
                        }
                    }
                    
                }
                if(cardScript.CardName == "Second Wind")
                {
                    //x1 reinforcement2
                    InstantiateCard(ref tempObj, false, ref ReinforcementPack2, 1, false);
                }
                if(cardScript.CardName == "Symbol of Faith")
                {
                    //x1 healing pack
                    InstantiateCard(ref tempObj, false, ref HealingPack, 1, true);
                }
                if(cardScript.CardName == "Pact of Maggots")
                {
                    //x1 Necromancer
                    InstantiateCard(ref tempObj, false, ref NecromancerPack, 1, true);
                }
                if(cardScript.CardName == "Inner Strength")
                {
                    //x1 arcane
                    InstantiateCard(ref tempObj, false, ref ArcanePack, 1, true);
                }
                if(cardScript.CardName == "Eternal Will")
                {
                    //x1 primus pack
                    InstantiateCard(ref tempObj, false, ref PrimusPack, 1, true);
                }
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
                    else if (numKindling > 0)
                    {
                        //put the kindling mark on the card
                        cardObj.GetComponent<CardMovement>().isKindling = true;
                        cardObj.transform.Find("Kindling").GetComponent<SpriteRenderer>().enabled = true;
                        numKindling--;
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
                    }else if (numKindling > 0)
                    {
                        //put the kindling mark on the card
                        cardObj.GetComponent<CardMovement>().isKindling = true;
                        cardObj.transform.Find("Kindling").GetComponent<SpriteRenderer>().enabled = true;
                        numKindling--;
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
                    else if (numKindling > 0)
                    {
                        //put the kindling mark on the card
                        cardObj.GetComponent<CardMovement>().isKindling = true;
                        cardObj.transform.Find("Kindling").GetComponent<SpriteRenderer>().enabled = true;
                        numKindling--;
                    }

                    cardObj.transform.position = enemyDeckTrans.position;
                    cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                    cardObj.GetComponent<SpriteRenderer>().sprite = NagaSprite;
                    cardObj.GetComponent<CardMovement>().isEnemyCard = true;
                    NagaDeck.Add(cardObj);
                }
                else if (cardScript.CardType == "status")
                {
                    cardObj.GetComponent<SpriteRenderer>().sprite = StatusSprite;
                    if (cardScript.CardName == "Poison")
                    {
                        cardObj.transform.position = poisonDeckTrans.position;
                        cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                        cardObj.GetComponent<CardMovement>().isEnemyCard = false;
                        PoisonDeck.Add(cardObj);

                    }else if(cardScript.CardName == "Wound")
                    {
                        cardObj.transform.position = poisonDeckTrans.position;
                        cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);

                        cardObj.GetComponent<CardMovement>().isEnemyCard = false;
                        WoundDeck.Add(cardObj);
                    }
                }
                else
                {
                    cardObj.transform.position = new Vector3(1000,1000,1000);
                    cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);
                }
            }

            Destroy(tempObj);

        }

        //fill stats
        statManagerScript.UpdateHealth("player", player_health, player_health);
        statManagerScript.SetTotalCards("player", player_cardsInDeck,0);
        statManagerScript.UpdateHealth("enemy", enemy_health,enemy_health);
        statManagerScript.SetTotalCards("enemy", enemy_cardsInDeck,0);


        //start the game
        if(level > 0)
        {
            //make the player choose a deck before starting the game
            //if it is not the tutorial
            generatePackScript.StartPackSelection(level);
            generatePackScript.FindCurrentDeck();
        }
        else
        {
          handManagerScript.InitialiseCards(level);
          gameManagerScript.StartGame(level);
        }
        
        
    }

    public IEnumerator ChosePack(int level)
    {
        print("Start Level: " + level.ToString());
        numPacks++;
        //level = 0;
        //fade the screen to black
        StartCoroutine(menuScript.FadeOutFadeIn());
        yield return new WaitForSecondsRealtime(1.5f);
        packCanvas.enabled = false;

        //Initialise decks in their respective scripts
        handManagerScript.InitialiseCards(level);
        gameManagerScript.StartGame(level);
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
        cardScript.Kindling = my_container.GetData("kindling").ToInt();

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
