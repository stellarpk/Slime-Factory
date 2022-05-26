using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGenerator : Facility
{
    public Text egSlimeNum;  
    public void Awake()
    {

        GameDifficulty = GameObject.Find("SelectDifficulty").GetComponent<SelectGameDifficulty>();
        level = 1;
        maxLevel = 10;
        if(GameDifficulty.difficulty == Difficulty.Normal)
        {
            generateTime = 7f;
            generateMount = 7;
        }
        else if(GameDifficulty.difficulty == Difficulty.Hard)
        {
            generateTime = 8f;
            generateMount = 5;
        }
        maxWorkSlime = 3;
        index = 0;
        delay = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        currentSlimeNum = slimeList.Count;
        slimeList.Capacity = maxWorkSlime;
        Display();
        Cost();
        SortByHpDown();
        Displaying();
        Work();
    }

    // +
    public void Requesting()
    {
        if (GameManager.instance.facilityList[1].slimeList.Count > 0 && currentSlimeNum + GameManager.instance.facilityList[0].currentSlimeNum < maxWorkSlime && GameManager.instance.facilityList[0].currentSlimeNum < GameManager.instance.facilityList[0].maxWorkSlime)
        {
            base.MoveSlime(GameManager.instance.facilityList[1].slimeList[0], Destination.EnergyGenerator, (int)Destination.Storage);
            index++;
        }
    }

    // -
    public void Sending()
    {
        if(GameManager.instance.facilityList[2].slimeList.Count > 0 && GameManager.instance.facilityList[0].currentSlimeNum < GameManager.instance.facilityList[0].maxWorkSlime)
        {
            base.MoveSlime(GameManager.instance.facilityList[2].slimeList[0], Destination.Storage, (int)Destination.EnergyGenerator);
            index--;
        }
    }

    public void Display()
    {
        egSlimeNum.text = currentSlimeNum.ToString() + " / " + maxWorkSlime.ToString();
    }

    // 체력 내림차순
    public void SortByHpDown()
    {
        slimeList.Sort(delegate (Slime slime1, Slime slime2)
        {
            if (slime1.currentHP > slime2.currentHP)
            {
                return 1;
            }

            else if (slime1.currentHP < slime2.currentHP)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        for (int i = 0; i < hpSlider.Length; i++)
        {
            hpSlider[i].gameObject.SetActive(true);
            if (slimeList.Count < hpSlider.Length)
            {
                for (int j = slimeList.Count; j < hpSlider.Length; j++)
                {
                    hpSlider[j].gameObject.SetActive(false);
                }
            }
            if (hpSlider[i].gameObject.activeSelf)
            {
                hpSlider[i].value = slimeList[i].currentHP / slimeList[i].maxHP;
                if (hpSlider[i].value < 0.25)
                {
                    hpColor[i].color = Color.red;
                }
                else
                {
                    hpColor[i].color = new Color(57 / 255f, 198 / 255f, 53 / 255f); ;
                }
            }

        }
    }

    public void Displaying()
    {
        base.DisplayUpgrade(level, maxLevel, maxWorkSlime, plusSlime, generateTime, generateMount, upgradeCost);
    }

    public void Cost()
    {
        base.UpgradeCostPerLevel();
    }

    public void LevelUP()
    {
        base.Upgrade();
    }

    public void Work()
    {
        base.Working();
    }
}
