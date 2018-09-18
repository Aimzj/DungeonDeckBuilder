using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSensor : MonoBehaviour {
    private HandManager handManagerScript;
    private GameObject playArea, trashArea, discardArea;

    private Transform trashPile, discardPile;

    public bool cardIsPresent;
    public bool isPlay, isDiscard, isTrash;

    Ray ray;
    RaycastHit hit;
    RaycastHit[] hits;
    // Use this for initialization
    void Start () {
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();

        playArea = GameObject.Find("PlayArea");
        playArea.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.35f, 0f);

        trashArea = GameObject.Find("TrashArea");
        trashArea.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0f);

        discardArea = GameObject.Find("DiscardArea");
        discardArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0f);

        cardIsPresent = false;

        isPlay = false;
        isTrash = false;
        isDiscard = false;
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

                if (handManagerScript.isHoldingCard)
                {
                    playArea.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.35f, 0.5f);
                    cardIsPresent = true;

                    isPlay = true;
                    isDiscard = false;
                    isTrash = false;
                }
                else
                {
                    playArea.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.35f, 0f);
                    cardIsPresent = false;

                    isPlay = false;
                }
                    
            }
            else if (hit.collider.name == "TrashArea")
            {
                isFound = true;

                if (handManagerScript.isHoldingCard)
                {
                    trashArea.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
                    cardIsPresent = true;

                    isPlay = false;
                    isDiscard = false;
                    isTrash = true;
                }
                else
                {
                    trashArea.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0f);
                    cardIsPresent = false;

                    isTrash = false;
                }

            }
            else if (hit.collider.name == "DiscardArea")
            {
                isFound = true;

                if (handManagerScript.isHoldingCard)
                {
                    discardArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.5f);
                    cardIsPresent = true;

                    isPlay = false;
                    isDiscard = true;
                    isTrash = false;
                }
                else
                {
                    discardArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0f);
                    cardIsPresent = false;

                    isDiscard = false;
                }

            }
            else if (count==len && !isFound)
            {
                playArea.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.35f, 0f);
                trashArea.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0f);
                discardArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0f);
                cardIsPresent = false;
            }
        }

    }
}
