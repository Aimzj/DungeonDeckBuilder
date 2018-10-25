using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {

    [SerializeField]
    public List<GameObject> deck = new List<GameObject>();
    [SerializeField]
    public List<GameObject> discardPile;
    [SerializeField]
    private List<GameObject> burnPile;
    [SerializeField]
    private List<GameObject> inHand;

    EnemyHand mainHand;

    int deckSize;
    int discardSize;
    int handSize;
    int burnSize;
    public int test;

    private void Start()
    {
        setUp();
        mainHand = GetComponent<EnemyHand>();
        DrawCard(3);
       // mainHand.TurnStart();
    }

    void setUp()
    {
        for (int i = 0; i <= deck.Count - 1; i ++)
        {
            deck[i] = Instantiate(deck[i], new Vector3(0f, 10f, 0f), Quaternion.Euler(90, 0, 0));
        }
    }
    public void AddDiscard(GameObject UsedCard)
    {
        discardPile.Add(UsedCard);
    }

    public List<GameObject> DiscDeck()
    {
        List<GameObject> dupeDiscardPile;
        dupeDiscardPile = discardPile;
        burnPile.Clear();
        return (burnPile);
        
    }

    public void BurnCard(GameObject UsedCard)
    {
        burnPile.Add(UsedCard);
    }

    public void Reshuffle()
    {
        //spiderDeck.deck = new List<GameObject>();
        deck = discardPile;
        print("RELOAD");
        discardPile = new List<GameObject>();
    }

    public void  DrawCard(int drawNum)
    {
        
        for(int i = 0; i < drawNum; i ++)
        {
            if(deck.Count <= 0)
            {
                Reshuffle();
            }
            else
            {
                GameObject newCard = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);
                
                mainHand.Draw(newCard);
            }
           
        }
        
        //return newCard;
    }

 
}
