using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleFunctions : MonoBehaviour {
    public float textLength = 2f;

    [HideInInspector]
    public Text myText;
    public float textTimer, textTimerLim;

    float textPerc;

    [HideInInspector]
    public SubtitleList myParent;
    //public float lifeTimer, lifeTimerLim;

    public float fadeInTimer = 0.5f, fadeOutTimer = 0.5f;
    public bool visible, turningOff = false;
    public float ttoTimer = 0f, ttoTimerLim = 0f;
    float ttoPerc; //tto stands for time till off
    Color oriCol;

    // Use this for initialization
    void Start()
    {
        myParent = transform.parent.GetComponent<SubtitleList>();

        myText = gameObject.GetComponent<Text>();
        oriCol = new Color (0.35f,0.35f,0.35f,1f);
        myText.color = Color.clear;
        turningOff = false;

        if (ttoTimerLim == 0f)
        {
            ttoTimerLim = textLength-0.25f; 
        }
        //visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {

         

        if (visible)
        {
            Timer(ref textTimer, textTimerLim, ref textPerc);
            float percAlt = textPerc * textPerc;

            myText.color = Color.Lerp(Color.clear, oriCol, percAlt);

            Timer(ref ttoTimer, ttoTimerLim, ref ttoPerc);

            if (ttoTimer ==ttoTimerLim && !turningOff)
            {
                VisibleSwitchOff();
            }

            
        }
        else if (!visible)
        {
            Timer(ref textTimer, textTimerLim, ref textPerc);
            float percAlt = textPerc * textPerc;

            myText.color = Color.Lerp(oriCol, Color.clear, percAlt);

            if (textTimer==textTimerLim)
            {
                turningOff = false;
                ttoTimer = 0f;
            }

        }

        

    }

    public void VisibleSwitchOn()
    {
        
        textTimer = 0f;
        visible = true;
        textTimerLim = fadeInTimer;
       

        
    }

    public void VisibleSwitchOff()
    {
        
            textTimer = 0f;
            visible = false;
            textTimerLim = fadeOutTimer;
        
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
