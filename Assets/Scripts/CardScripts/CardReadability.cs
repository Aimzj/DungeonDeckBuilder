using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReadability : MonoBehaviour {

    public bool inPlay; //any area that is not the deck, discard or burn pile
    public bool textVisible = false;//if the card should display text or not
    public float textTimer, textTimerLim = 0.5f, textPerc;// text visibility of card Timer, Timer End Point, Percentile value of completion of time that has passed
    public float IconTimer, IconTimerLim = 0.5f, IconPerc;// icon visibility of card Timer, Timer End Point, Percentile value of completion of time that has passed
    public SpriteRenderer cardLayer1;//layer1 = layer that display the icon.
    

	// Use this for initialization
	void Start () {
        cardLayer1 = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (textVisible)
        {
            Timer(ref textTimer, textTimerLim, ref textPerc);
            float percAlt = textPerc * textPerc;

            cardLayer1.color = Color.Lerp(Color.white,Color.clear,percAlt);

            IconTimer = IconTimerLim - textTimer;
            
        }
        else
        {
            Timer(ref IconTimer, IconTimerLim, ref IconPerc);
            float percAlt = IconPerc * IconPerc;

            cardLayer1.color = Color.Lerp(Color.clear, Color.white, IconPerc);

            textTimer = textTimerLim - IconTimer;
        }
		
	}

    private void OnMouseEnter()
    {
        if (inPlay)
        {
            textVisible = true;
        }
    }

    private void OnMouseExit()
    {
        if (inPlay)
        {
            textVisible = false;
        }
    }



    //A timer that counts up if the timer is below the timerlimit
    //if it is higher however timer is set to the timerlimit.
    public void Timer(ref float timer, float timerLim,ref float perc)
    {
        if (timer<timerLim)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = timerLim;
        }

        perc = timer / textTimerLim;
    }
}
