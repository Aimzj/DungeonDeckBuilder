using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName ="Card")]
public class NewBehaviourScript : ScriptableObject {
    public string CardName;
    public string Description;

    //BOOLEANS
    public bool isDiscardEffect;
    public bool isBurnEffect;
    public bool isFree = false;
    public bool hasSigil = false;
    public bool useHighestVal_Attack = false;

    //ART
    public Sprite Artwork;

    //COSTS
    public int DiscardCost;
    public int BurnCost;
    public int Attack;
    public int Defense;

    //DISCARD EFFECTS
    public int numDiscard_Deck;
    public int numDiscard_Hand;
    public int numDrawCards;
    public int numDiscardDraw;
    public int numPoisonsPlace;
    public int numPickupFromDiscard;

    //BURN EFFECTS
    public int numBurn_Deck;
    public int numBurn_Hand;
    public bool canUnsear = false;

    public enum type {Player, Enemy};

    public type CardType = type.Player;
}
