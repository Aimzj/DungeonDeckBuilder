using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour {

    public string Card_Name;
    public int Discard_Cost;
    public int Burn_Cost;
    public string Flavour_Text;
    public int Attack;
    public int Defense;

    public int Has_Sigil;

    public string Discard_Effect;
    public int DE_Num;
    public int DE_Num_Deck_Draw_Random;
    public int DE_Num_Free;
    public int DE_Is_Highest_Val_Attack;
    public int DE_Num_Poisons;
    public int DE_Num_Discard_Draw_Choice;
    public int DE_Num_Deck_Draw_Top;
    public int DE_Add_Attack;

    public string Burn_Effect;
    public int BE_Num;
    public int BE_Num_Deck_Draw_Choice;
    public int BE_Can_Unsear;
    public int BE_Num_Wounds_Discard;
    public int BE_Num_Poisons;
    public int BE_Draw_Cards;
    public int BE_Is_Option;
    public int BE_Add_Defense;

    public string Image_Name;

	// Use this for initialization
	void Start () {
        LoadMyData();
        
    }

    public void LoadMyData()
    {
        ImportedDataContainer my_container = ImportData.GetContainer("card_1");

        Card_Name = my_container.GetData("name").ToString();
        Discard_Cost = my_container.GetData("discard_cost").ToInt();
        Burn_Cost = my_container.GetData("burn_cost").ToInt();
        Flavour_Text = my_container.GetData("flavour_text").ToString();
        Attack = my_container.GetData("attack").ToInt();
        Defense = my_container.GetData("defense").ToInt();
        Has_Sigil = my_container.GetData("has_sigil").ToInt();

        Discard_Effect = my_container.GetData("discard_effect").ToString();
        DE_Num = my_container.GetData("DISCARD_num").ToInt();
        DE_Num_Deck_Draw_Random = my_container.GetData("DISCARD_num_deck_draw_random").ToInt();
        DE_Num_Free = my_container.GetData("DISCARD_num_free").ToInt();
        DE_Is_Highest_Val_Attack = my_container.GetData("DISCARD_is_highest_val_attack").ToInt();
        DE_Num_Poisons = my_container.GetData("DISCARD_num_poisons").ToInt();
        DE_Num_Discard_Draw_Choice = my_container.GetData("DISCARD_num_discard_draw_choice").ToInt();
        DE_Num_Deck_Draw_Top = my_container.GetData("DISCARD_num_deck_draw_top").ToInt();
        DE_Add_Attack = my_container.GetData("DISCARD_add_attack").ToInt();

        Burn_Effect = my_container.GetData("burn_effect").ToString();
        BE_Num = my_container.GetData("BURN_num").ToInt();
        BE_Num_Deck_Draw_Choice = my_container.GetData("BURN_num_deck_draw_choice").ToInt();
        BE_Can_Unsear = my_container.GetData("BURN_can_unsear").ToInt();
        BE_Num_Wounds_Discard = my_container.GetData("BURN_num_wounds_discard").ToInt();
        BE_Num_Poisons = my_container.GetData("BURN_num_poisons").ToInt();
        BE_Draw_Cards = my_container.GetData("BURN_draw_cards").ToInt();
        BE_Is_Option = my_container.GetData("BURN_is_option").ToInt();
        BE_Add_Defense = my_container.GetData("BURN_add_defense").ToInt();
        Image_Name = my_container.GetData("imageid").ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
