using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardObj : MonoBehaviour {

    //card stats
    public string CardName;
    public int DiscardCost;
    public int BurnCost;
    public string FlavourText;
    public int Attack;
    public int Defense;

    public int HasSigil;

    public int IsOnArrival;
    public int OA_IsChoice;
    public int OA_NumDeckDrawTop;
    public int OA_TempDefenseIncrease;

    public int IsUndying;
    public int IsStack;
    public int IsUntapped;

    public string DiscardEffect;
    public int DE_NumDeckDrawRandom;
    public int DE_NumFree;
    public int DE_IsHighestValAttack;
    public int DE_NumPoisons;
    public int DE_NumDiscardDrawChoice;
    public int DE_NumDeckDrawTop;
    public int DE_AddAttack;
    public int DE_SelectDiscardAddAttack;

    public string BurnEffect;
    public int BE_NumDeckDrawChoice;
    public int BE_RegainSigil;
    public int BE_NumWoundsDiscard;
    public int BE_NumPoisons;
    public int BE_NumDeckDrawTop;
    public int BE_IsOption;
    public int BE_AddDefense;
    public int BE_DrawAttackValOfBurnt;
    public int BE_HasReaction;
    public int BE_DrawDefenseValOfBurnt;

    public string Image_Name;
    public int NumInDeck;
    public string CardType;

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
        attack_Text = this.gameObject.transform.Find("Attack").GetComponent<TextMeshPro>();
        defence_Text = this.gameObject.transform.Find("Defense").GetComponent<TextMeshPro>();

        cardName_Text.text = CardName;
        discardCost_Text.text = DiscardCost.ToString();
        burnCost_Text.text = BurnCost.ToString();
        discardEffect_Text.text = DiscardEffect;
        burnEffect_Text.text = BurnEffect;
        attack_Text.text = Attack.ToString();
        defence_Text.text = Defense.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
