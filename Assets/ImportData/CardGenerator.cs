using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour {
    //script loops through excel spreadsheet and seperates cards into their different decks

    //transforms of decks
    private Transform playerDeckTrans, enemyDeckTrans, poisonDeckTrans;

    private int numUniqueCards=11;

    public GameObject Card;

    private GameObject tempObj, cardObj;
    private CardObj cardScript;

    private HandManager handManagerScript;
    
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

        //looping through all unique cards
        for(int i = 1; i < numUniqueCards+1; i++)
        {
            cardObj = (GameObject)Instantiate(Card, Vector3.zero, Quaternion.identity);
            //load data onto card
            cardScript = cardObj.GetComponent<CardObj>();
            Debug.Log("working: " + i.ToString());
            LoadMyData("card_" + i.ToString());

            //seperate cards into decks
            for (int j=0; j<cardScript.NumInDeck; j++)
            {
                tempObj = (GameObject)Instantiate(cardObj);
                if (cardScript.CardType == "player_starting")
                {
                    tempObj.transform.position = playerDeckTrans.position;
                    tempObj.transform.rotation = Quaternion.Euler(90, 0, 0);
                    PlayerDeck.Add(tempObj);
                }
                else if(cardScript.CardType == "spider")
                {
                    tempObj.transform.position = enemyDeckTrans.position;
                    tempObj.transform.rotation = Quaternion.Euler(90, 0, 0);
                    SpiderDeck.Add(tempObj);
                }
                else if(cardScript.CardType == "status")
                {
                    if(cardScript.CardName == "Poison")
                    {
                        tempObj.transform.position = poisonDeckTrans.position;
                        tempObj.transform.rotation = Quaternion.Euler(90, 0, 0);
                        PoisonDeck.Add(tempObj);
                    }
                }
            }

            Destroy(cardObj);

        }
        
        
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

       // cardScript.DiscardEffect = my_container.GetData("effect").ToString();
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
