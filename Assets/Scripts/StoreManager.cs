using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour {

    //temporary card position
    public GameObject TempObj;

    //cards
    public List<GameObject> cardList_Deck, cardList_Trash;

    //deck positions
    private Transform playerDeck, playerTrash;

    //positions for store card categories
    private Transform rare1;
    private Transform legendary1, legendary2;
    private Transform mythic1, mythic2, mythic3;

    //card prefab
    public GameObject CardObj;

    //cardholders
    private GameObject rare1_Card, legendary1_Card, legendary2_Card, mythic1_Card, mythic2_Card, mythic3_Card;

    //raycasting
    Ray ray;
    RaycastHit hit;
    RaycastHit[] hits;

    void Start () {

        playerDeck = GameObject.Find("Deck").GetComponent<Transform>();
        playerTrash = GameObject.Find("Trash").GetComponent<Transform>();

        rare1 = GameObject.Find("Rare1").GetComponent<Transform>();
        legendary1 = GameObject.Find("Legendary1").GetComponent<Transform>();
        legendary2 = GameObject.Find("Legendary2").GetComponent<Transform>();
        mythic1 = GameObject.Find("Mythic1").GetComponent<Transform>();
        mythic2 = GameObject.Find("Mythic2").GetComponent<Transform>();
        mythic3 = GameObject.Find("Mythic3").GetComponent<Transform>();

        //instantiate cards and put into place holders
        rare1_Card = (GameObject) Instantiate(CardObj, rare1.position, Quaternion.identity);
        legendary1_Card = (GameObject)Instantiate(CardObj, legendary1.position, Quaternion.identity);
        legendary2_Card = (GameObject)Instantiate(CardObj, legendary2.position, Quaternion.identity);
        mythic1_Card = (GameObject)Instantiate(CardObj, mythic1.position, Quaternion.identity);
        mythic2_Card = (GameObject)Instantiate(CardObj, mythic2.position, Quaternion.identity);
        mythic3_Card = (GameObject)Instantiate(CardObj, mythic3.position, Quaternion.identity);

    }
	
	// Update is called once per frame
	void Update () {

        //raycasts down into the scene and checks whether the player clicked on a store card
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, 200);

            int len = hits.Length;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.name == "Rare1")
                {
                    //add the card to the player deck
                    cardList_Deck.Add(rare1_Card);

                    //change target position of rare card to the player deck
                    var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDeck.position.x, playerDeck.position.y, playerDeck.position.z), Quaternion.identity);
                    rare1_Card.GetComponent<CardMovement>()._targetTransform = obj.transform;
                }
                else if(hit.collider.name == "Legendary1")
                {
                    //add the card to the player deck
                    cardList_Deck.Add(legendary1_Card);

                    //change target position of rare card to the player deck
                    var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDeck.position.x, playerDeck.position.y, playerDeck.position.z), Quaternion.identity);
                    legendary1_Card.GetComponent<CardMovement>()._targetTransform = obj.transform;
                }
                else if(hit.collider.name == "Legendary2")
                {
                    //add the card to the player deck
                    cardList_Deck.Add(legendary2_Card);

                    //change target position of rare card to the player deck
                    var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDeck.position.x, playerDeck.position.y, playerDeck.position.z), Quaternion.identity);
                    legendary2_Card.GetComponent<CardMovement>()._targetTransform = obj.transform;
                }
                else if(hit.collider.name == "Mythic1")
                {
                    //add the card to the player deck
                    cardList_Deck.Add(mythic1_Card);

                    //change target position of rare card to the player deck
                    var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDeck.position.x, playerDeck.position.y, playerDeck.position.z), Quaternion.identity);
                    mythic1_Card.GetComponent<CardMovement>()._targetTransform = obj.transform;
                }
                else if(hit.collider.name == "Mythic2")
                {
                    //add the card to the player deck
                    cardList_Deck.Add(mythic2_Card);

                    //change target position of rare card to the player deck
                    var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDeck.position.x, playerDeck.position.y, playerDeck.position.z), Quaternion.identity);
                    mythic2_Card.GetComponent<CardMovement>()._targetTransform = obj.transform;
                }
                else if(hit.collider.name == "Mythic3")
                {
                    //add the card to the player deck
                    cardList_Deck.Add(mythic3_Card);

                    //change target position of rare card to the player deck
                    var obj = (GameObject)Instantiate(TempObj, new Vector3(playerDeck.position.x, playerDeck.position.y, playerDeck.position.z), Quaternion.identity);
                    mythic3_Card.GetComponent<CardMovement>()._targetTransform = obj.transform;
                }
            }
        }
        
    }
}
