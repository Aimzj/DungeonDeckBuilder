using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dummy : MonoBehaviour {

    private List<GameObject> dummyHand = new List<GameObject>();

    public int TurnCount = 0;

    private AreaManager areaManagerScript;
    private HandManager handManagerScript;
    private StatsManager statsManagerScript;
    private GameManager gameManagerScript;

    private void Start()
    {
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public IEnumerator Action()
    {
        updateEnemyHand();

        switch (TurnCount)
        {
            case 1:
                print(1);
                playCard("strike");
                break;

            case 3:
                print(3);
                playCard("strike");
                break;

            

        }
        yield return new WaitForSecondsRealtime(2);
        print("DONE WITH ENEMY REACTION");
        gameManagerScript.EndEnemyReact();
    }

    void playCard(string name)
    {
        print(3);
        for (int i = 0; i < dummyHand.Count; i++)
        {
            if (dummyHand[i].GetComponent<CardObj>().CardName == name)
            {
                dummyHand[i].GetComponent<CardMovement>().PlayEnemyCard();
                statsManagerScript.UpdateAttack("enemy", 1);

                //TRIGGER PROMPT FOR PLAYER
            }
        }
    }

    public IEnumerator Reaction()
    {
        print("DUMMY REACTION");
        updateEnemyHand();

        switch (TurnCount)
        {
            case 0:
                print(0);
                //TRIGGER PROMPT FOR PLAYER
                break;

            case 2:

                print(2);
                playCard("Lesser Guard");
                break;

          

        }
        yield return new WaitForSecondsRealtime(2);
        print("END ENEMY ACTION");
        gameManagerScript.EndEnemyTurn();
    }

    private void updateEnemyHand()
    {
        dummyHand.Clear();
        dummyHand = new List<GameObject>(handManagerScript.enemyHandlist);
    }
}
