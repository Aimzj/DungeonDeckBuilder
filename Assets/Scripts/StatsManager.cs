using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour {

    private TextMeshPro playerHealth, playerDefense, playerAttack, playerEssence, playerCardsInDeck, playerCycleTokens, playerDiscard, playerBurn;
    private TextMeshPro enemyHealth, enemyDefense, enemyAttack, enemyEssence, enemyCardsInDeck, enemyCycleTokens, enemyDiscard, enemyBurn;

    private int totalHealth_player, numHealth_player, numDefense_player, numAttack_player, numEssence_player, numCardsInDeck_player, totalCards_player, numCycleTokens_player, numDiscard_player, numBurn_player;
    private int totalHealth_enemy, numHealth_enemy, numDefense_enemy, numAttack_enemy, numEssence_enemy, numCardsInDeck_enemy, totalCards_enemy, numCycleTokens_enemy, numDiscard_enemy, numBurn_enemy;

    void Start () {
        playerHealth = GameObject.Find("PlayerHealth").GetComponent<TextMeshPro>();
        playerDefense = GameObject.Find("PlayerDefense").GetComponent<TextMeshPro>();
        playerAttack = GameObject.Find("PlayerAttack").GetComponent<TextMeshPro>();
        playerEssence = GameObject.Find("PlayerEssence").GetComponent<TextMeshPro>();
        playerCardsInDeck = GameObject.Find("PlayerCardsInDeck").GetComponent<TextMeshPro>();
        playerCycleTokens = GameObject.Find("PlayerCycleTokens").GetComponent<TextMeshPro>();
        playerDiscard = GameObject.Find("PlayerDiscard").GetComponent<TextMeshPro>();
        playerBurn = GameObject.Find("PlayerBurn").GetComponent<TextMeshPro>();

        enemyHealth = GameObject.Find("EnemyHealth").GetComponent<TextMeshPro>();
        enemyDefense = GameObject.Find("EnemyDefense").GetComponent<TextMeshPro>();
        enemyAttack = GameObject.Find("EnemyAttack").GetComponent<TextMeshPro>();
        enemyEssence = GameObject.Find("EnemyEssence").GetComponent<TextMeshPro>();
        enemyCardsInDeck = GameObject.Find("EnemyCardsInDeck").GetComponent<TextMeshPro>();
        enemyCycleTokens = GameObject.Find("EnemyCycleTokens").GetComponent<TextMeshPro>();
        enemyDiscard = GameObject.Find("EnemyDiscard").GetComponent<TextMeshPro>();
        enemyBurn = GameObject.Find("EnemyBurn").GetComponent<TextMeshPro>();
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

        }else if (target == "enemy")
        {
            numHealth_enemy += num;
            enemyHealth.text = "Health: " + numHealth_enemy +"/" + totalHealth_enemy;
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

    void Update () {
		
	}
}
