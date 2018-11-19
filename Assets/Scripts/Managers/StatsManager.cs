using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour {
    public TextMeshProUGUI endGameText, endGameReasonText;

    public TextMeshProUGUI playerHealth, enemyHealth, buttonText, playerSigils, enemySigils, playerCycleTokens, enemyCycleTokens, playerKindling, 
        enemyKindling, playerPhase, enemyPhase, playerCardsInDeck, enemyCardsInDeck, playerDiscard, enemyDiscard, playerBurn, enemyBurn, statusDeck, statusDiscard;

    public int totalHealth_player, numHealth_player, numCardsInDeck_player, totalCards_player, numCycleTokens_player, numDiscard_player, numBurn_player, numSigilsRemaining_player, numKindling_player, numTotalKinding_player, numStatusDeck, numStatusDiscard;
    public int totalHealth_enemy, numHealth_enemy, numCardsInDeck_enemy, totalCards_enemy, numCycleTokens_enemy, numDiscard_enemy, numBurn_enemy, numSigilsRemaining_enemy, numKindling_enemy, numTotalKindling_enemy;

    public string phase_player, phase_enemy;

    public Transform sword, attackGem, defenseGem;
    public TextMeshPro attackText, defenseText;
    public int numAttack, numDefense;

    private GameManager gameManagerScript;

    public Canvas gameOverCanvas;
    void Start () {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    public void UpdateNumStatusCards(int numDeck, int numDiscard)
    {
        numStatusDeck += numDeck;
        numStatusDiscard += numDiscard;

        statusDeck.text = numStatusDeck.ToString();
        statusDiscard.text = numStatusDiscard.ToString();
    }

    private void RotateSword(float angle)
    {
        sword.GetComponent<TargetRotation>().targetAngle = new Vector3(90f, 0f, angle);

        Vector3 tempPos = new Vector3(attackGem.position.x, attackGem.position.y, attackGem.position.z);
        attackGem.GetComponent<TargetPosition>().targetPos = new Vector3(defenseGem.position.x, defenseGem.position.y, defenseGem.position.z);
        defenseGem.GetComponent<TargetPosition>().targetPos = tempPos;
    }

    //Kindling changes when cards are added or removed from deck and when the kindled cards are burnt
    public void UpdateKindling(string target, int numInDeck, int numTotal)
    {
        if (target == "player")
        {
            numKindling_player += numInDeck;
            numTotalKinding_player += numTotal;
            playerKindling.text =numKindling_player + "/" + numTotalKinding_player;
        }
        else if (target == "enemy")
        {
            numKindling_enemy += numInDeck;
            numTotalKindling_enemy += numTotal;
            enemyKindling.text = numKindling_enemy + "/" + numTotalKindling_enemy;
        }
    }

    public void ClearAttack()
    {
        numAttack = 0;
        attackText.text = numAttack.ToString();
    }
    public void ClearDefense()
    {
        numDefense = 0;
        defenseText.text = numDefense.ToString();
    }

    public void SetPhase(string target, string phase)
    {
        if (target == "player")
        {
            phase_player = phase;

            if(phase == "waiting")
                playerPhase.text = phase;
            else
                playerPhase.text = phase+ " phase";

            if (phase == "action")
            {
                buttonText.text = "End Turn";

                //player is attacking, enemy defending
                RotateSword(180f);
            }
            else if(phase == "reaction")
            {
                buttonText.text = "React";
            }
            else
            {
                buttonText.text = "waiting...";
            }

        }
        else if (target == "enemy")
        {
            phase_enemy = phase;

            if (phase == "waiting")
                enemyPhase.text = phase;
            else
                enemyPhase.text = phase + " phase";

            if (phase == "action")
            {
                //enemy is attacking, player is defending
                RotateSword(0f);
            }
        }
    }

    public void UpdateHealth(string target, int num, int total)
    {
        if(target == "player")
        {
            numHealth_player += num;
            totalHealth_player += total;
            playerHealth.text = numHealth_player+ "/" +totalHealth_player;

            if (numHealth_player <= 0)
            {
                gameOverCanvas.enabled = true;
                //PLAYER LOSES
                endGameText.text = "You Lost!";
                endGameReasonText.text = "You ran out of health";
                gameManagerScript.EndLevel(1);
            }
        }
        else if (target == "enemy")
        {
            numHealth_enemy += num;
            totalHealth_enemy += total;
            enemyHealth.text = numHealth_enemy +"/" + totalHealth_enemy;

            if (numHealth_enemy <= 0)
            {
                gameOverCanvas.enabled = true;
                //PLAYER WINS
                endGameText.text = "You Win!";
                endGameReasonText.text = "You depleted the enemy's health";
                gameManagerScript.EndLevel(0);
            }
        }
    }

    public void UpdateDefense(string target, int num)
    {
        numDefense += num;
        defenseText.text = numDefense.ToString();
    }

    public void UpdateAttack(string target, int num)
    {
        numAttack += num;
        attackText.text = numAttack.ToString();
    }

    /*public void SetTotalCards(string target, int num, int cardsInPlay)
    {
        if (target == "player")
        {
            totalCards_player = num;
            numCardsInDeck_player = num;
            playerCardsInDeck.text = (numCardsInDeck_player-cardsInPlay) + "/" + totalCards_player;
        }
        else if (target == "enemy")
        {
            totalCards_enemy = num;
            numCardsInDeck_enemy = num;
            enemyCardsInDeck.text = (numCardsInDeck_enemy-cardsInPlay) + "/" + totalCards_enemy;
        }
    }*/

    public void UpdateCardsInDeck(string target, int numCards, int numTotal)
    {
        if (target == "player")
        {
            numCardsInDeck_player += numCards;
            totalCards_player += numTotal;
            playerCardsInDeck.text =  numCardsInDeck_player + "/"+totalCards_player;

            if (totalCards_player <= 0)
            {
                gameOverCanvas.enabled = true;
                //PLAYER WINS
                endGameText.text = "You Lost!";
                endGameReasonText.text = "You burnt all of your cards";
                gameManagerScript.EndLevel(1);
            }
        }
        else if (target == "enemy")
        {
            numCardsInDeck_enemy += numCards;
            totalCards_enemy += numTotal;
            enemyCardsInDeck.text = numCardsInDeck_enemy + "/" + totalCards_enemy;

            if (totalCards_enemy <= 0)
            {
                gameOverCanvas.enabled = true;
                //PLAYER WINS
                endGameText.text = "You Win!";
                endGameReasonText.text = "You burnt all of the enemy's cards";
                gameManagerScript.EndLevel(0);
            }
        }
    }

    public void UpdateCycleTokens(string target, int num)
    {
        if (target == "player")
        {
            numCycleTokens_player += num;
            playerCycleTokens.text = numCycleTokens_player.ToString();
        }
        else if (target == "enemy")
        {
            numCycleTokens_enemy += num;
            enemyCycleTokens.text = numCycleTokens_enemy.ToString();
        }
    }

    public void UpdateDiscard(string target, int num)
    {
        if (target == "player")
        {
            numDiscard_player += num;
            playerDiscard.text = numDiscard_player.ToString();
        }
        else if (target == "enemy")
        {
            numDiscard_enemy += num;
            enemyDiscard.text = numDiscard_enemy.ToString();
        }
    }

    public void UpdateBurn(string target, int num)
    {
        if (target == "player")
        {
            numBurn_player += num;
            playerBurn.text = numBurn_player.ToString();
        }
        else if (target == "enemy")
        {
            numBurn_enemy += num;
            enemyBurn.text = numBurn_enemy.ToString();
        }
    }

    public void UpdateSigils(string target, int num)
    {
        if (target == "player")
        {
            numSigilsRemaining_player += num;
            playerSigils.text = numSigilsRemaining_player.ToString();

            if (numSigilsRemaining_player <= 0)
            {
                gameOverCanvas.enabled = true;
                //PLAYER LOSES
                endGameText.text = "You Lost!";
                endGameReasonText.text = "All of your sigil cards were burned";
                gameManagerScript.EndLevel(1);
            }
        }
        else if (target == "enemy")
        {
            numSigilsRemaining_enemy += num;
            enemySigils.text = numSigilsRemaining_enemy.ToString();

            if (numSigilsRemaining_enemy <= 0)
            {
                gameOverCanvas.enabled = true;
                //PLAYER WINS
                endGameText.text = "You Win!";
                endGameReasonText.text = "You burned all of the enemy's sigil cards";
                gameManagerScript.EndLevel(0);
            }
        }
    }

}
