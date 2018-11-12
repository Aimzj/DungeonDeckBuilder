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

    public int SigilNum;
    public int Kindling;

    public string DiscardEffect;

    public string BurnEffect;

    public string Image_Name;
    public int NumInDeck;
    public string CardType;

    private TextMeshPro cardName_Text, discardCost_Text, burnCost_Text, 
                discardEffect_Text, burnEffect_Text, 
                flavourText_Text, attack_Text, defence_Text;
    // Use this for initialization
    void Start () {
        cardName_Text = this.gameObject.transform.Find("Title").GetComponent<TextMeshPro>();
        discardCost_Text = this.gameObject.transform.Find("DiscardCost").GetComponent<TextMeshPro>();
        burnCost_Text = this.gameObject.transform.Find("BurnCost").GetComponent<TextMeshPro>();
        discardEffect_Text = this.gameObject.transform.Find("DiscardEffect").GetComponent<TextMeshPro>();
        burnEffect_Text = this.gameObject.transform.Find("BurnEffect").GetComponent<TextMeshPro>();
        attack_Text = this.gameObject.transform.Find("AttackCost").GetComponent<TextMeshPro>();
        defence_Text = this.gameObject.transform.Find("DefenseCost").GetComponent<TextMeshPro>();

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
