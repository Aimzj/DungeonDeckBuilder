using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDecks : MonoBehaviour {
    private HandManager handManagerScript;
    private CardGenerator cardGeneratorScript;
    private Transform enemyDeckTransform,playerDeckTransform;


    public List<GameObject> PlayerCardList;
    public List<GameObject> EnemyCardList;


    // Use this for initialization
    void Start () {
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        cardGeneratorScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();
        enemyDeckTransform = GameObject.Find("EnemyDeck").GetComponent<Transform>();
        playerDeckTransform = GameObject.Find("Deck").GetComponent<Transform>();
    }
	
    public void setDecks()
    {
        handManagerScript.playerDeckList = new List<GameObject>(10);
        handManagerScript.enemyDeckList = new List<GameObject>(10);
        for (int i = 0; i <= PlayerCardList.Count -1; i ++)
        {
            print("SET DECKS");
            var newcard = Instantiate(PlayerCardList[i], playerDeckTransform.position, Quaternion.Euler(90, 0, 0));
            handManagerScript.playerDeckList[i] = newcard;
        }
        for (int i = 0; i <= EnemyCardList.Count -1; i++)
        {
            var newcard = Instantiate(PlayerCardList[i], enemyDeckTransform.position, Quaternion.Euler(90, 0, 0));
            handManagerScript.enemyDeckList[i] = newcard;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
