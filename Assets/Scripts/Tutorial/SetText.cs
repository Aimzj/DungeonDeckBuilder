using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetText : MonoBehaviour {

    string outputText = "";
    int turnCounter = 0;

    bool dummyAttack = false;
    bool isPlayerAction = false;
    private Dummy dummyScript;
    float timer = 3f;

	// Use this for initialization

	
	IEnumerator TriggerText()
    {
        switch(turnCounter)
        {
            case 1:
                outputText = "Each point of damage will destroy a card in your oppenent's deck";
                yield return new WaitForSecondsRealtime(3);
                outputText = "Burn them all to win!";
                break;
            case 2:
                if (!dummyAttack)
                outputText = "Now it's your opponent's turn, lets see what they do";
                else if(!dummyAttack)
                    outputText = "Play a guard to defend yourself";
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
            

     
    }
}
