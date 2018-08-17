using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName ="Card")]
public class NewBehaviourScript : ScriptableObject {
    public string CardName;
    public string Description;

    public Sprite Artwork;

    public int Cost;
    public int Attack;
    public int Defense;
}
