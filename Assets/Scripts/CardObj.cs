using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObj : MonoBehaviour {

    //card stats
    public string Card_Name;
    public int Discard_Cost;
    public int Burn_Cost;
    public string Flavour_Text;
    public int Attack;
    public int Defense;

    public int Has_Sigil;

    public string Effect;
    public int E_num_deck_draw_top;

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
    public int BE_Num_Deck_Draw_Top;
    public int BE_Is_Option;
    public int BE_Add_Defense;

    public string Image_Name;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
