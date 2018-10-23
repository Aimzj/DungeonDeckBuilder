using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour {
    //script loops through excel spreadsheet and seperates cards into their different decks
    public int numUniqueCards=7;

    public GameObject Card;

    private GameObject cardObj;
    private CardObj cardScript;
    

	// Use this for initialization
	void Start () {
        //looping through all unique cards
        for(int i = 1; i < numUniqueCards+1; i++)
        {
            //Instantiate a card object
            cardObj = (GameObject)Instantiate(Card, Vector3.zero, Quaternion.identity);
            cardScript = cardObj.GetComponent<CardObj>();
            LoadMyData("card_"+i.ToString());
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

        cardScript.HasSigil = my_container.GetData("has_sigil").ToInt();

        cardScript.HasEvent = my_container.GetData("has_event").ToInt();

        cardScript.IsOnArrival = my_container.GetData("is_on_arrival").ToInt();
        cardScript.OA_IsChoice = my_container.GetData("OA_is_choice").ToInt();
        cardScript.OA_NumDeckDrawTop = my_container.GetData("OA_num_deck_draw_top").ToInt();
        cardScript.OA_TempDefenseIncrease = my_container.GetData("OA_temp_defense_increase").ToInt();

        cardScript.IsUndying = my_container.GetData("is_undying").ToInt();
        cardScript.IsStack = my_container.GetData("is_stack").ToInt();
        cardScript.IsUntapped = my_container.GetData("is_untapped").ToInt();

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
        cardScript.NumInPlayerDeck = my_container.GetData("num_in_player_deck").ToInt();
        cardScript.NumInEnemyDeck = my_container.GetData("num_in_enemy_deck").ToInt();
        cardScript.NumInOtherDeck = my_container.GetData("num_in_other_deck").ToInt();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
