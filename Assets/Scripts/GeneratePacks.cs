using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GeneratePacks : MonoBehaviour {

    // Use this for initialization
    public TextMeshProUGUI Title1, Title2, Title3;
    public Button card1_1, card1_2, card1_3, card1_4, card1_5;
    public Button card2_1, card2_2, card2_3, card2_4, card2_5;
    public Button card3_1, card3_2, card3_3, card3_4, card3_5;
    public Button Choose1, Choose2, Choose3;

    private Canvas packCanvas;
    private CardGenerator cardGenScript;

    public TextMeshProUGUI cardList;

    public List<GameObject> pack1, pack2, pack3;

    private Transform playerDeckPos;

    private StatsManager statsManagerScript;


    private int level;
    void Start () {
        level = 0;

        packCanvas = GameObject.Find("Pack_Canvas").GetComponent<Canvas>();
        cardGenScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();

        playerDeckPos = GameObject.Find("Deck").GetComponent<Transform>();
        statsManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
	}

    public void FindCurrentDeck()
    {
        level = PlayerPrefs.GetInt("Level");
        if (level == 2)
        {
            string prevPack = PlayerPrefs.GetString("Pack");

            if (prevPack == "Healing Pack")
            {
                for(int i=0; i<cardGenScript.HealingPack.Count; i++)
                {
                    cardGenScript.PlayerDeck.Add(cardGenScript.HealingPack[i]);
                }
            }
            else if (prevPack == "Reinforcement 1 Pack")
            {
                for (int i = 0; i < cardGenScript.ReinforcementPack1.Count; i++)
                {
                    cardGenScript.PlayerDeck.Add(cardGenScript.ReinforcementPack1[i]);
                }
            }
            else if (prevPack == "Reinforcement 1 Pack")
            {
                for (int i = 0; i < cardGenScript.ReinforcementPack2.Count; i++)
                {
                    cardGenScript.PlayerDeck.Add(cardGenScript.ReinforcementPack2[i]);
                }
            }
            else if (prevPack == "Ash Pack")
            {
                for (int i = 0; i < cardGenScript.AshPack.Count; i++)
                {
                    cardGenScript.PlayerDeck.Add(cardGenScript.AshPack[i]);
                }
            }
            else if (prevPack == "Necromancer Pack")
            {
                for (int i = 0; i < cardGenScript.NecromancerPack.Count; i++)
                {
                    cardGenScript.PlayerDeck.Add(cardGenScript.NecromancerPack[i]);
                }
            }
            else if (prevPack == "Arcane Pack")
            {
                for (int i = 0; i < cardGenScript.ArcanePack.Count; i++)
                {
                    cardGenScript.PlayerDeck.Add(cardGenScript.ArcanePack[i]);
                }
            }
            else if (prevPack == "Primus Pack")
            {
                for (int i = 0; i < cardGenScript.PrimusPack.Count; i++)
                {
                    cardGenScript.PlayerDeck.Add(cardGenScript.PrimusPack[i]);
                }
            }
        }

        List<GameObject> playerList = cardGenScript.PlayerDeck;

        int numGuard = 0, numStrike = 0, numAdvancedGuard = 0, numFStrike = 0, numNREG = 0, numEternalWill = 0, numInnerStrength = 0,
            numEchoInnerStrength = 0, numPOM = 0, numFireball = 0, numSOF = 0, numLuckyCharm = 0, numSecondWind = 0;
        for(int i = 0; i< playerList.Count; i++)
        {
            if(playerList[i].GetComponent<CardObj>().CardName == "Guard")
            {
                numGuard++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Strike")
            {
                numStrike++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Advanced Guard")
            {
                numAdvancedGuard++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Focused Strike")
            {
                numFStrike++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Naga Red Eye Gem")
            {
                numNREG++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Eternal Will")
            {
                numEternalWill++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Inner Strength")
            {
                numInnerStrength++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Inner Strength (Echo)")
            {
                numEchoInnerStrength++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Pact of Maggots")
            {
                numPOM++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Fireball")
            {
                numFireball++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Symbol of Faith")
            {
                numSOF++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Lucky Charm")
            {
                numLuckyCharm++;
            }
            else if (playerList[i].GetComponent<CardObj>().CardName == "Second Wind")
            {
                numSecondWind++;
            }
        }

        string finalList = "";
        if (numGuard > 0)
        {
            finalList += "x" + numGuard.ToString() + " Guard \n";
        }
        if(numStrike > 0)
        {
            finalList += "x" + numGuard.ToString() + " Strike \n";
        }
        if (numAdvancedGuard > 0)
        {
            finalList += "x" + numAdvancedGuard.ToString() + " Advanced Guard \n";
        }
        if (numFStrike > 0)
        {
            finalList += "x" + numFStrike.ToString() + " Focused Strike \n";
        }
        if (numNREG > 0)
        {
            finalList += "x" + numNREG.ToString() + " Naga Red Eye Gem \n";
        }
        if (numEternalWill > 0)
        {
            finalList += "x" + numEternalWill.ToString() + " Eternal Will \n";
        }
        if (numInnerStrength > 0)
        {
            finalList += "x" + numInnerStrength.ToString() + " Inner Strength \n";
        }
        if (numEchoInnerStrength > 0)
        {
            finalList += "x" + numEchoInnerStrength.ToString() + " Inner Strength (Echo) \n";
        }
        if (numPOM > 0)
        {
            finalList += "x" + numPOM.ToString() + " Pact of Maggots \n";
        }
        if (numFireball > 0)
        {
            finalList += "x" + numFireball.ToString() + " Fireball \n";
        }
        if (numSOF > 0)
        {
            finalList += "x" + numSOF.ToString() + " Symbol of Faith \n";
        }
        if (numLuckyCharm > 0)
        {
            finalList += "x" + numLuckyCharm.ToString() + " Lucky Charm \n";
        }
        if (numSecondWind > 0)
        {
            finalList += "x" + numSecondWind.ToString() + " Second Wind \n";
        }



        cardList.text = finalList;
    }

    public void StartPackSelection(int lvl)
    {
        level = lvl;
        packCanvas.enabled = true;
        GenPacks(lvl);
    }

	public void GenPacks(int lvl)
    {
        List<string> packNames = new List<string>();
        packNames.Add("Reinforcement 1");
        packNames.Add("Reinforcement 2");
        packNames.Add("Ash");
        packNames.Add("Arcane");
        packNames.Add("Primus");
        int random;

        packNames.Add("Healing Pack");

        //PACK 1
        random = Random.Range(0, packNames.Count - 1);
        if(packNames[random]=="Healing Pack")
        {
            SetHealingPack(Title1, card1_1, card1_2, card1_3, card1_4, card1_5);
            pack1 = cardGenScript.HealingPack;
        }
        else if (packNames[random] == "Reinforcement 1")
        {
            SetReinf1Pack(Title1, card1_1, card1_2, card1_3, card1_4, card1_5);
            pack1 = cardGenScript.ReinforcementPack1;
        }
        else if (packNames[random] == "Reinforcement 2")
        {
            SetReinf2Pack(Title1, card1_1, card1_2, card1_3, card1_4, card1_5);
            pack1 = cardGenScript.ReinforcementPack2;
        }
        else if (packNames[random] == "Ash")
        {
            SetAshPack(Title1, card1_1, card1_2, card1_3, card1_4, card1_5);
            pack1 = cardGenScript.AshPack;
        }
        else if (packNames[random] == "Necromancer")
        {
            SetNecroPack(Title1, card1_1, card1_2, card1_3, card1_4, card1_5);
            pack1 = cardGenScript.NecromancerPack;
        }
        else if (packNames[random] == "Arcane")
        {
            SetArcanePack(Title1, card1_1, card1_2, card1_3, card1_4, card1_5);
            pack1 = cardGenScript.ArcanePack;
        }
        else if (packNames[random] == "Primus")
        {
            SetPrimusPack(Title1, card1_1, card1_2, card1_3, card1_4, card1_5);
            pack1 = cardGenScript.PrimusPack;
        }

        packNames.RemoveAt(random);


        //PACK 2
        random = Random.Range(0, packNames.Count - 1);
        if (packNames[random] == "Healing Pack")
        {
            SetHealingPack(Title2, card2_1, card2_2, card2_3, card2_4, card2_5);
            pack2 = cardGenScript.HealingPack;
        }
        else if (packNames[random] == "Reinforcement 1")
        {
            SetReinf1Pack(Title2, card2_1, card2_2, card2_3, card2_4, card2_5);
            pack2 = cardGenScript.ReinforcementPack1;
        }
        else if (packNames[random] == "Reinforcement 2")
        {
            SetReinf2Pack(Title2, card2_1, card2_2, card2_3, card2_4, card2_5);
            pack2 = cardGenScript.ReinforcementPack2;
        }
        else if (packNames[random] == "Ash")
        {
            SetAshPack(Title2, card2_1, card2_2, card2_3, card2_4, card2_5);
            pack2 = cardGenScript.AshPack;
        }
        else if (packNames[random] == "Necromancer")
        {
            SetNecroPack(Title2, card2_1, card2_2, card2_3, card2_4, card2_5);
            pack2 = cardGenScript.NecromancerPack;
        }
        else if (packNames[random] == "Arcane")
        {
            SetArcanePack(Title2, card2_1, card2_2, card2_3, card2_4, card2_5);
            pack2 = cardGenScript.ArcanePack;
        }
        else if (packNames[random] == "Primus")
        {
            SetPrimusPack(Title2, card2_1, card2_2, card2_3, card2_4, card2_5);
            pack2 = cardGenScript.PrimusPack;
        }

        packNames.RemoveAt(random);

        //PACK 3
        random = Random.Range(0, packNames.Count - 1);
        if (packNames[random] == "Healing Pack")
        {
            SetHealingPack(Title3, card3_1, card3_2, card3_3, card3_4, card3_5);
            pack3 = cardGenScript.HealingPack;
        }
        else if (packNames[random] == "Reinforcement 1")
        {
            SetReinf1Pack(Title3, card3_1, card3_2, card3_3, card3_4, card3_5);
            pack3 = cardGenScript.ReinforcementPack1;
        }
        else if (packNames[random] == "Reinforcement 2")
        {
            SetReinf2Pack(Title3, card3_1, card3_2, card3_3, card3_4, card3_5);
            pack3 = cardGenScript.ReinforcementPack2;
        }
        else if (packNames[random] == "Ash")
        {
            SetAshPack(Title3, card3_1, card3_2, card3_3, card3_4, card3_5);
            pack3 = cardGenScript.AshPack;
        }
        else if (packNames[random] == "Necromancer")
        {
            SetNecroPack(Title3, card3_1, card3_2, card3_3, card3_4, card3_5);
            pack3 = cardGenScript.NecromancerPack;
        }
        else if (packNames[random] == "Arcane")
        {
            SetArcanePack(Title3, card3_1, card3_2, card3_3, card3_4, card3_5);
            pack3 = cardGenScript.ArcanePack;
        }
        else if (packNames[random] == "Primus")
        {
            SetPrimusPack(Title3, card3_1, card3_2, card3_3, card3_4, card3_5);
            pack3 = cardGenScript.PrimusPack;
        }

        packNames.RemoveAt(random);
    }
    
    private void SetHealingPack(TextMeshProUGUI Title, Button btn1, Button btn2, Button btn3, Button btn4, Button btn5)
    {
        Title.text = "Healing Pack";
        btn1.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Symbol of Faith";
        btn2.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Advanced Guard";
        btn3.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Guard";
        btn4.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x2 Strike (kindled)";
        btn5.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn5.transform.position = new Vector3(100, 100, 100);
    }

    private void SetReinf1Pack(TextMeshProUGUI Title, Button btn1, Button btn2, Button btn3, Button btn4, Button btn5)
    {
        Title.text = "Reinforcement 1 Pack";
        btn1.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x2 Advanced Guard";
        btn2.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Guard";
        btn3.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x2 Lucky Charm";
        btn4.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn4.transform.position = new Vector3(100, 100, 100);
        btn5.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn5.transform.position = new Vector3(100, 100, 100);
    }

    private void SetReinf2Pack(TextMeshProUGUI Title, Button btn1, Button btn2, Button btn3, Button btn4, Button btn5)
    {
        Title.text = "Reinforcement 2 Pack";
        btn1.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x2 Focused Strike";
        btn2.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Second Wind";
        btn3.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Strike";
        btn4.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Guard";
        btn5.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn5.transform.position = new Vector3(100, 100, 100);
    }

    private void SetAshPack(TextMeshProUGUI Title, Button btn1, Button btn2, Button btn3, Button btn4, Button btn5)
    {
        Title.text = "Ash Pack";
        btn1.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Fireball (sigil)";
        btn2.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Focused Strike";
        btn3.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x3 Strike (1/3 kindled)";
        btn4.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn4.transform.position = new Vector3(100, 100, 100);
        btn5.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn5.transform.position = new Vector3(100, 100, 100);
    }

    private void SetNecroPack(TextMeshProUGUI Title, Button btn1, Button btn2, Button btn3, Button btn4, Button btn5)
    {
        Title.text = "Necromancer Pack";
        btn1.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Pact of Maggots (sigil)";
        btn2.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x2 Guard";
        btn3.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x2 Strike (kindled)";
        btn4.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn4.transform.position = new Vector3(100, 100, 100);
        btn5.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn5.transform.position = new Vector3(100, 100, 100);
    }

    private void SetArcanePack(TextMeshProUGUI Title, Button btn1, Button btn2, Button btn3, Button btn4, Button btn5)
    {
        Title.text = "Arcane Pack";
        btn1.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "Inner Strength (sigil)";
        btn2.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x4 Guard (kindled)";
        btn3.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn3.transform.position = new Vector3(100, 100, 100);
        btn4.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn4.transform.position = new Vector3(100, 100, 100);
        btn5.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = " ";
        btn5.transform.position = new Vector3(100, 100, 100);
    }

    private void SetPrimusPack(TextMeshProUGUI Title, Button btn1, Button btn2, Button btn3, Button btn4, Button btn5)
    {
        Title.text = "Primus Pack";
        btn1.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Eternal Will (sigil)";
        btn2.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Strike";
        btn3.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Guard (kindled)";
        btn4.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x2 Advanced Guard (kindled)";
        btn5.transform.Find("Title1").GetComponent<TextMeshProUGUI>().text = "x1 Focused Strike";
    }

    public void ChoosePack1()
    {
        Choose1.enabled = false;
        Choose2.enabled = false;
        Choose3.enabled = false;

        level = PlayerPrefs.GetInt("Level");
        if (level == 1)
        {
            PlayerPrefs.SetString("Pack", Title1.text);
        }

        int numSigils = 0;
        int numKindling = 0;
        //loop through pack 1 and add to the deck list
        for (int i = 0; i< pack1.Count; i++)
        {
            cardGenScript.PlayerDeck.Add(pack1[i]);
            pack1[i].transform.position = playerDeckPos.position;
            //count sigils
            if (pack1[i].transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled)
            {
                numSigils++;
            }
            //count kindling
            if (pack1[i].GetComponent<CardMovement>().isKindling)
            {
                numKindling++;
            }
        }
        //update health
        statsManagerScript.UpdateHealth("player", numSigils * 5, numSigils * 5);
        //update num cards in deck
        statsManagerScript.UpdateCardsInDeck("player", pack1.Count, pack1.Count);
        //update kindling
        statsManagerScript.UpdateKindling("player", numKindling, numKindling);
        //update sigils
        statsManagerScript.UpdateSigils("player", numSigils);

        StartCoroutine(cardGenScript.ChosePack(level));
    }

    public void ChoosePack2()
    {
        Choose1.enabled = false;
        Choose2.enabled = false;
        Choose3.enabled = false;

        level = PlayerPrefs.GetInt("Level");
        if (level == 1)
        {
            PlayerPrefs.SetString("Pack", Title2.text);
        }

        int numSigils = 0;
        int numKindling = 0;
        //loop through pack 2 and add to the deck list
        for (int i = 0; i < pack2.Count; i++)
        {
            cardGenScript.PlayerDeck.Add(pack2[i]);
            pack2[i].transform.position = playerDeckPos.position;
            //count sigils
            if (pack2[i].transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled)
            {
                numSigils++;
            }
            //count kindling
            if (pack2[i].GetComponent<CardMovement>().isKindling)
            {
                numKindling++;
            }
        }
        //update health
        statsManagerScript.UpdateHealth("player", numSigils * 5, numSigils * 5);
        //update num cards in deck
        statsManagerScript.UpdateCardsInDeck("player", pack2.Count, pack2.Count);
        //update kindling
        statsManagerScript.UpdateKindling("player", numKindling, numKindling);
        //update sigils
        statsManagerScript.UpdateSigils("player", numSigils);

        StartCoroutine(cardGenScript.ChosePack(level));
    }

    public void ChoosePack3()
    {
        Choose1.enabled = false;
        Choose2.enabled = false;
        Choose3.enabled = false;

        level = PlayerPrefs.GetInt("Level");
        if (level == 1)
        {
            PlayerPrefs.SetString("Pack", Title3.text);
        }

        int numSigils = 0;
        int numKindling = 0;
        //loop through pack 3 and add to the deck list
        for (int i = 0; i < pack3.Count; i++)
        {
            cardGenScript.PlayerDeck.Add(pack3[i]);
            pack3[i].transform.position = playerDeckPos.position;
            //count sigils
            if (pack3[i].transform.Find("Sigil").GetComponent<SpriteRenderer>().enabled)
            {
                numSigils++;
            }
            //count kindling
            if (pack3[i].GetComponent<CardMovement>().isKindling)
            {
                numKindling++;
            }
        }
        //update health
        statsManagerScript.UpdateHealth("player", numSigils * 5, numSigils * 5);
        //update num cards in deck
        statsManagerScript.UpdateCardsInDeck("player", pack3.Count, pack3.Count);
        //update kindling
        statsManagerScript.UpdateKindling("player", numKindling, numKindling);
        //update sigils
        statsManagerScript.UpdateSigils("player", numSigils);

        StartCoroutine(cardGenScript.ChosePack(level));
    }
    // Update is called once per frame
    void Update () {
		
	}
}
