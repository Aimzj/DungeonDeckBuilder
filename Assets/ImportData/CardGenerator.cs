using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour {
    //script loops through excel spreadsheet and seperates cards into their different decks
    public int numUniqueCards=13;

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

        cardScript.Card_Name = my_container.GetData("name").ToString();
        cardScript.Discard_Cost = my_container.GetData("discard_cost").ToInt();
        cardScript.Burn_Cost = my_container.GetData("burn_cost").ToInt();
        cardScript.Flavour_Text = my_container.GetData("flavour_text").ToString();
        cardScript.Attack = my_container.GetData("attack").ToInt();
        cardScript.Defense = my_container.GetData("defense").ToInt();
        cardScript.Has_Sigil = my_container.GetData("has_sigil").ToInt();

        cardScript.Effect = my_container.GetData("effect").ToString();
        cardScript.E_num_deck_draw_top = my_container.GetData("EFFECT_num_deck_draw_top").ToInt();

        cardScript.Discard_Effect = my_container.GetData("discard_effect").ToString();
        cardScript.DE_Num = my_container.GetData("DISCARD_num").ToInt();
        cardScript.DE_Num_Deck_Draw_Random = my_container.GetData("DISCARD_num_deck_draw_random").ToInt();
        cardScript.DE_Num_Free = my_container.GetData("DISCARD_num_free").ToInt();
        cardScript.DE_Is_Highest_Val_Attack = my_container.GetData("DISCARD_is_highest_val_attack").ToInt();
        cardScript.DE_Num_Poisons = my_container.GetData("DISCARD_num_poisons").ToInt();
        cardScript.DE_Num_Discard_Draw_Choice = my_container.GetData("DISCARD_num_discard_draw_choice").ToInt();
        cardScript.DE_Num_Deck_Draw_Top = my_container.GetData("DISCARD_num_deck_draw_top").ToInt();
        cardScript.DE_Add_Attack = my_container.GetData("DISCARD_add_attack").ToInt();

        cardScript.Burn_Effect = my_container.GetData("burn_effect").ToString();
        cardScript.BE_Num = my_container.GetData("BURN_num").ToInt();
        cardScript.BE_Num_Deck_Draw_Choice = my_container.GetData("BURN_num_deck_draw_choice").ToInt();
        cardScript.BE_Can_Unsear = my_container.GetData("BURN_can_unsear").ToInt();
        cardScript.BE_Num_Wounds_Discard = my_container.GetData("BURN_num_wounds_discard").ToInt();
        cardScript.BE_Num_Poisons = my_container.GetData("BURN_num_poisons").ToInt();
        cardScript.BE_Num_Deck_Draw_Top = my_container.GetData("BURN_num_deck_draw_top").ToInt();
        cardScript.BE_Is_Option = my_container.GetData("BURN_is_option").ToInt();
        cardScript.BE_Add_Defense = my_container.GetData("BURN_add_defense").ToInt();
        cardScript.Image_Name = my_container.GetData("imageid").ToString();

        Debug.Log("done");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
