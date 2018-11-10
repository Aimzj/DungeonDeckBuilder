using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //card interactions
    public AudioSource DrawCard_Sound, PickUpCard_Sound, HoverCard_Sound, PlayCard_Sound;

	// Use this for initialization
	void Start () {
		
	}
	
    public void PlaySound_DrawCard()
    {
        DrawCard_Sound.Play();
    }

    public void PlaySound_PickUpCard()
    {
        PickUpCard_Sound.Play();
    }

    public void PlaySound_HoverCard()
    {
        HoverCard_Sound.Play();
    }

    public void PlaySound_PlayCard()
    {
        PlayCard_Sound.Play();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
