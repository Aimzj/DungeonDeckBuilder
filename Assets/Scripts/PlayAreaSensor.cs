using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaSensor : MonoBehaviour {
    private HandManager handManagerScript;
    private GameObject playArea;

    public bool cardIsPresent;

    Ray ray;
    RaycastHit hit;
    RaycastHit[] hits;
    // Use this for initialization
    void Start () {
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        playArea = GameObject.Find("PlayArea");

        cardIsPresent = false;
	}

    // Update is called once per frame
    void Update () {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray, 200);

        int len = hits.Length;
        int count = 0;
        bool isFound = false;

        foreach (RaycastHit hit in hits)
        {
            count++;
            if (hit.collider.name == "PlayArea")
            {
                isFound = true;

                Debug.Log(handManagerScript.isHoldingCard);
                if (handManagerScript.isHoldingCard)
                {
                    Debug.Log("hi");
                    playArea.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.35f, 0.5f);
                    cardIsPresent = true;
                }
                else
                {
                    playArea.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.35f, 0f);
                    cardIsPresent = false;
                }
                    
            }
            else if(count==len && !isFound)
            {
                playArea.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.35f, 0f);
                cardIsPresent = false;
            }
        }

    }
}
