using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour {
     [SerializeField]
    DeckManager enemyMan;
    [SerializeField]
    public List<GameObject> inHand = new List<GameObject>();

    [SerializeField]
    protected int handRange = 10;

    int handSize = 0;

	// Use this for initialization
	void Start () {
        enemyMan = GetComponent<DeckManager>();
	}
	
	public void Draw(GameObject newCard)
    {
        handSize++;
        print("handsize is " + inHand.Count);
        
       //GameObject cardInhand = Instantiate(newCard, new Vector3(0f, 10f, 0f),Quaternion.Euler(90,0,0));
        inHand.Add(newCard);
        reOrderHand();
     
    }

    //Sort Cards
   public  void reOrderHand()
    {
        float x = 0;
        //Check through hand to sort cards
        for (int i = 0; i < inHand.Count-1; i++)
        {

            x = i * handRange / inHand.Count;
            //print(x);
            inHand[i].transform.position = new Vector3(x - 5, -0.81f, 7.04f);
        }
    }

   
}
