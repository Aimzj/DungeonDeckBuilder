using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSensor : MonoBehaviour {
    private HandManager handManagerScript;
    private GameObject playArea, trashArea, discardArea;

  //  private ParticleSystem burnParticles;

    private Transform trashPile, discardPile;

    public bool cardIsPresent;
    public bool isPlay, isDiscard, isTrash;

    private Color playArea_StartColour;

    Ray ray;
    RaycastHit hit;
    RaycastHit[] hits;
    // Use this for initialization
    void Start() {
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();

        playArea = GameObject.Find("PlayArea");
        playArea_StartColour = playArea.GetComponent<SpriteRenderer>().color;
        playArea.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0f);

        trashArea = GameObject.Find("TrashArea");

        discardArea = GameObject.Find("DiscardArea");

      //  burnParticles = GameObject.Find("FireBall").GetComponent<ParticleSystem>();

        cardIsPresent = false;

        isPlay = false;
        isTrash = false;
        isDiscard = false;
    }

    private void OnMouseUp()
    { 
       // if(burnParticles.isPlaying)
       //     burnParticles.Stop();
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

                if (handManagerScript.isPlayerHoldingCard)
                {
                    playArea.GetComponent<SpriteRenderer>().color = playArea_StartColour;
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
              //  burnParticles.Stop();
            }
            else if (hit.collider.name == "TrashArea")
            {
                isFound = true;

                if (handManagerScript.isPlayerHoldingCard)
                {
                    trashArea.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
                    cardIsPresent = true;

                    isPlay = false;
                    isDiscard = false;
                    isTrash = true;
                }
                else
                {
                    trashArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                    cardIsPresent = false;

                    isTrash = false;
                }
             //   burnParticles.Stop();
            }
            else if (hit.collider.name == "DiscardArea")
            {
                isFound = true;

                if (handManagerScript.isPlayerHoldingCard)
                {
                    discardArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.5f);
                    cardIsPresent = true;

                    isPlay = false;
                    isDiscard = true;
                    isTrash = false;
                }
                else
                {
                    discardArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                    cardIsPresent = false;

                    isDiscard = false;
                }
            //    burnParticles.Stop();
            }
            else if (count==len && !isFound)
            {
                playArea.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.35f, 0f);
                trashArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                discardArea.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                cardIsPresent = false;

            //    burnParticles.Stop();
            }
        }

    }
}
