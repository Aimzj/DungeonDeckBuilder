using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private TextMeshPro cardName_Text, discardCost_Text, burnCost_Text, 
                effect_Text, discardEffect_Text, burnEffect_Text, 
                flavourText_Text, attack_Text, defence_Text;
    // Use this for initialization
    void Start () {
        cardName_Text = this.gameObject.transform.Find("CardName").GetComponent<TextMeshPro>();
        discardCost_Text = this.gameObject.transform.Find("DiscardCost").GetComponent<TextMeshPro>();
        burnCost_Text = this.gameObject.transform.Find("BurnCost").GetComponent<TextMeshPro>();
        effect_Text = this.gameObject.transform.Find("Effect").GetComponent<TextMeshPro>();
        discardEffect_Text = this.gameObject.transform.Find("DiscardEffect").GetComponent<TextMeshPro>();
        burnEffect_Text = this.gameObject.transform.Find("BurnEffect").GetComponent<TextMeshPro>();
        //flavourText_Text = this.gameObject.transform.Find("CardName").GetComponent<TextMeshPro>();
        attack_Text = this.gameObject.transform.Find("Attack").GetComponent<TextMeshPro>();
        defence_Text = this.gameObject.transform.Find("Defense").GetComponent<TextMeshPro>();

        cardName_Text.text = Card_Name;
        discardCost_Text.text = Discard_Cost.ToString();
        burnCost_Text.text = Burn_Cost.ToString();
        effect_Text.text = Effect;
        discardEffect_Text.text = Discard_Effect;
        burnEffect_Text.text = Burn_Effect;
        attack_Text.text = Attack.ToString();
        defence_Text.text = Defense.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
