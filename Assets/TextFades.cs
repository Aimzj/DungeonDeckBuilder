using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFades : MonoBehaviour {
    
    public Text myText;
    float textTimer, textTimerLim, textPerc;

    [HideInInspector]
    public SubtitleFunctions myScript;
    public float lifeTimer, lifeTimerLim;

    bool visible;
    Color oriCol;

    // Use this for initialization
    void Start () {
        myScript = gameObject.GetComponent<SubtitleFunctions>();

        myText = gameObject.GetComponent<Text>();
        oriCol = myText.color;
	}
	
	// Update is called once per frame
	void Update () {

         
        if (visible)
        {
            Timer(ref textTimer, textTimerLim, ref textPerc);
            float percAlt = textPerc * textPerc;

            myText.color = Color.Lerp(oriCol,Color.clear,percAlt);
        }
        else if (!visible)
        {
            Timer(ref textTimer, textTimerLim, ref textPerc);
            float percAlt = textPerc * textPerc;

            myText.color = Color.Lerp(Color.clear, oriCol, percAlt);

        }

         

    }

    public void VisibleSwitch()
    {
        if (visible)
        {
            visible = true;
            textTimerLim = 0.5f;
        }
        else if (!visible)
        {
            visible = false;
            textTimerLim = 0.5f;
        }

        textTimer = 0f;
    }

    public void Timer(ref float timer, float timerLim, ref float perc)
    {
        if (timer < timerLim)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = timerLim;
        }

        perc = timer / timerLim;
    }
}
