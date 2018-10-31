using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour {

    public TextMeshPro playerPhase, playerHealth, playerDefense, playerAttack, playerEssence, playerCardsInDeck, playerCycleTokens, playerDiscard, playerBurn, playerSigils;
    public TextMeshPro enemyPhase, enemyHealth, enemyDefense, enemyAttack, enemyEssence, enemyCardsInDeck, enemyCycleTokens, enemyDiscard, enemyBurn, enemySigils;
    public TextMeshPro endGameText, buttonText;

    public int totalHealth_player, numHealth_player, numDefense_player, numAttack_player, numEssence_player, numCardsInDeck_player, totalCards_player, numCycleTokens_player, numDiscard_player, numBurn_player, numSigilsRemaining_player;
    public int totalHealth_enemy, numHealth_enemy, numDefense_enemy, numAttack_enemy, numEssence_enemy, numCardsInDeck_enemy, totalCards_enemy, numCycleTokens_enemy, numDiscard_enemy, numBurn_enemy, numSigilsRemaining_enemy;

    public string phase_player, phase_enemy;

    void Start () {
    }

    public void ClearAttack()
    {
        numAttack_player = 0;
        numAttack_enemy = 0;
        enemyAttack.text = "Attack: 0";
        playerAttack.text = "Attack: 0";
    }
    public void ClearDefense()
    {
        numDefense_player = 0;
        numDefense_enemy = 0;
        enemyDefense.text = "Defense: 0";
        playerDefense.text = "Defense: 0";
    }

    public void SetPhase(string target, string phase)
    {
        if (target == "player")
        {
            phase_player = phase;

            if(phase == "waiting")
                playerPhase.text = phase.ToUpper();
            else
                playerPhase.text = phase.ToUpper() + " phase";

            if (phase == "action")
                buttonText.text = "End Turn";
            else if(phase == "reaction")
                buttonText.text = "REACT";
            else
                buttonText.text = "waiting...";

        }
        else if (target == "enemy")
        {
            phase_enemy = phase;

            if (phase == "waiting")
                enemyPhase.text = phase.ToUpper();
            else
                enemyPhase.text = phase.ToUpper() + " phase";
        }
    }

    public void SetHealth(string target, int num)
    {
        if (target == "player")
        {
            totalHealth_player = num;
            numHealth_player = num;
            playerHealth.text = "Health: " + numHealth_player+"/"+totalHealth_player;

        }
        else if (target == "enemy")
        {
            totalHealth_enemy = num;
            numHealth_enemy = num;
            enemyHealth.text = "Health: " + numHealth_enemy+"/"+totalHealth_enemy;
        }
    }

    public void UpdateHealth(string target, int num)
    {
        if(target == "player")
        {
            numHealth_player += num;
            playerHealth.text = "Health: " + numHealth_player+ "/" +totalHealth_player;

            if (numHealth_player <= 0)
            {
                //PLAYER LOSES
                endGameText.text = "YOU LOSE";
            }
        }
        else if (target == "enemy")
        {
            numHealth_enemy += num;
            enemyHealth.text = "Health: " + numHealth_enemy +"/" + totalHealth_enemy;

            if (numHealth_enemy <= 0)
            {
                //PLAYER WINS
                endGameText.text = "PLAYER WINS!";
            }
        }
    }

    public void UpdateDefense(string target, int num)
    {
        if (target == "player")
        {
            numDefense_player += num;
            playerDefense.text = "Defense: " + numDefense_player;

        }
        else if (target == "enemy")
        {
            numDefense_enemy += num;
            enemyDefense.text = "Defense: " + numDefense_enemy;
        }
    }

    public void UpdateAttack(string target, int num)
    {
        if (target == "player")
        {
            numAttack_player += num;
            playerAttack.text = "Attack: " + numAttack_player;

        }
        else if (target == "enemy")
        {
            numAttack_enemy += num;
            enemyAttack.text = "Attack: " + numAttack_enemy;
        }
    }

    public void UpdateEssence(string target, int num)
    {
        if (target == "player")
        {
            numEssence_player += num;
            playerEssence.text = "Essence: " + numEssence_player;

        }
        else if (target == "enemy")
        {
            numEssence_enemy += num;
            enemyEssence.text = "Essemce: " + numEssence_enemy;
        }
    }

    public void SetTotalCards(string target, int num)
    {
        if (target == "player")
        {
            totalCards_player = num;
            numCardsInDeck_player = num;
            playerCardsInDeck.text = "Cards In Deck: " + numCardsInDeck_player + "/" + totalCards_player;
        }
        else if (target == "enemy")
        {
            totalCards_enemy = num;
            numCardsInDeck_enemy = num;
            enemyCardsInDeck.text = "Cards In Deck: " + numCardsInDeck_enemy + "/" + totalCards_enemy;
        }
    }

    public void UpdateTotalCards(string target, int num)
    {
        if (target == "player")
        {
            totalCards_player += num;
            playerCardsInDeck.text = "Cards In Deck: " + numCardsInDeck_player + "/" + totalCards_player;

            if (totalCards_player <= 0)
            {
                //PLAYER WINS
                endGameText.text = "PLAYER WINS!";
            }

        }
        else if (target == "enemy")
        {
            totalCards_enemy += num;
            enemyCardsInDeck.text = "Cards In Deck: " + numCardsInDeck_enemy + "/" + totalCards_enemy;

            if (totalCards_enemy <= 0)
            {
                //PLAYER WINS
                endGameText.text = "PLAYER WINS!";
            }
        }
    }

    public void UpdateCardsInDeck(string target, int num)
    {
        if (target == "player")
        {
            numCardsInDeck_player += num;
            playerCardsInDeck.text = "Cards In Deck: " + numCardsInDeck_player + "/"+totalCards_player;
        }
        else if (target == "enemy")
        {
            numCardsInDeck_enemy += num;
            enemyCardsInDeck.text = "Cards In Deck: " + numCardsInDeck_enemy + "/" + totalCards_enemy;
        }
    }

    public void UpdateCycleTokens(string target, int num)
    {
        if (target == "player")
        {
            numCycleTokens_player += num;
            playerCycleTokens.text = "Cycle Tokens: " + numCycleTokens_player;
        }
        else if (target == "enemy")
        {
            numCycleTokens_enemy += num;
            enemyCycleTokens.text = "Cycle Tokens: " + numCycleTokens_enemy;
        }
    }

    public void UpdateDiscard(string target, int num)
    {
        if (target == "player")
        {
            numDiscard_player += num;
            playerDiscard.text = "Discard Pool: " + numDiscard_player;
        }
        else if (target == "enemy")
        {
            numDiscard_enemy += num;
            enemyDiscard.text = "Discard Pool: " + numDiscard_enemy;
        }
    }

    public void UpdateBurn(string target, int num)
    {
        if (target == "player")
        {
            numBurn_player += num;
            playerBurn.text = "Burn Pool: " + numBurn_player;
        }
        else if (target == "enemy")
        {
            numBurn_enemy += num;
            enemyBurn.text = "Burn Pool: " + numBurn_enemy;
        }
    }

    public void UpdateSigils(string target, int num)
    {
        if (target == "player")
        {
            print("playerLosesSigil: "+ numSigilsRemaining_player);
            numSigilsRemaining_player += num;
            playerSigils.text = "Sigils Remaining: " + numSigilsRemaining_player;

            if (numSigilsRemaining_player <= 0)
            {
                //PLAYER LOSES
                endGameText.text = "YOU LOSE";
            }
        }
        else if (target == "enemy")
        {
            numSigilsRemaining_enemy += num;
            enemySigils.text = "Sigils Remaining: " + numSigilsRemaining_enemy;

            if (numSigilsRemaining_enemy <= 0)
            {
                //PLAYER WINS
                endGameText.text = "PLAYER WINS!";
            }
        }
    }

}
