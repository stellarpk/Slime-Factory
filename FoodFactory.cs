using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodFactory : Facility
{
    public Text ffSlimeNum;
    public Text maxFoodNum;
    public Text plusFoodText;

    public int plusMaxFood;

    public void Awake()
    {

        GameDifficulty = GameObject.Find("SelectDifficulty").GetComponent<SelectGameDifficulty>();
        level = 1;
        maxLevel = 10;
        maxWorkSlime = 3;

        if (GameDifficulty.difficulty == Difficulty.Normal)
        {
            generateTime = 7f;
            generateMount = 7;
        }
        else if (GameDifficulty.difficulty == Difficulty.Hard)
        {
            generateTime = 8f;
            generateMount = 6;
        }
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
            base.MoveSlime(GameManager.instance.facilityList[1].slimeList[0], Destination.FoodFactory, (int)Destination.Storage);
            index++;
        }
    }

    // -
    public void Sending()
    {
        if (GameManager.instance.facilityList[4].slimeList.Count > 0 && GameManager.instance.facilityList[0].currentSlimeNum < GameManager.instance.facilityList[0].maxWorkSlime)
        {
            base.MoveSlime(GameManager.instance.facilityList[4].slimeList[0], Destination.Storage, (int)Destination.FoodFactory);
            index--;
        }
    }

    public void Display()
    {
        ffSlimeNum.text = currentSlimeNum.ToString() + " / " + maxWorkSlime.ToString();
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
                    hpColor[i].color = new Color(57 / 255f, 198 / 255f, 53 / 255f);
                }
            }

        }
    }

    public void Displaying()
    {
        base.DisplayUpgrade(level, maxLevel, maxWorkSlime, plusSlime, generateTime, generateMount, upgradeCost);
        
        maxFoodNum.text = "ÃÖ´ë ½Ä·® " + GameManager.instance.energyManager.maxFood.ToString();
        plusFoodText.text = " + "+plusMaxFood.ToString();
    }

    public void Cost()
    {
        switch (level)
        {
            case 1:
                upgradeCost = 100;
                plusSlime = 3;  //3
                plusMaxFood = 10; // 50
                break;
            case 2:
                upgradeCost = 150;
                plusSlime = 6; //6
                plusMaxFood = 20;
                break;
            case 3:
                upgradeCost = 340;
                plusSlime = 12; //12
                plusMaxFood = 30;
                break;
            case 4:
                upgradeCost = 720;
                plusSlime = 12; //24
                plusMaxFood = 40;
                break;
            case 5:
                upgradeCost = 1500;
                plusSlime = 18; //36
                plusMaxFood = 50;
                break;
            case 6:
                upgradeCost = 3150;
                plusSlime = 24; // 54
                plusMaxFood = 60;
                break;
            case 7:
                upgradeCost = 5200;
                plusSlime = 30; //78
                plusMaxFood = 70;
                break;
            case 8:
                upgradeCost = 7600;
                plusSlime = 36; //108
                plusMaxFood = 80;
                break;
            case 9:
                upgradeCost = 10000;
                plusSlime = 42; // 144
                plusMaxFood = 90;
                break;
                // 186
        }
    }

    public void LevelUP()
    {
        if (level < maxLevel)
        {
            if (GameManager.instance.energyManager.currentEnergy >= upgradeCost)
            {
                GameManager.instance.energyManager.currentEnergy -= upgradeCost;
                GameManager.instance.energyManager.maxFood += plusMaxFood;
                maxWorkSlime += plusSlime;
                level++;
            }
        }
        if (level == maxLevel)
        {
            upgradeResource.gameObject.SetActive(false);
            plusFoodText.gameObject.SetActive(false);
            plusSlimeText.gameObject.SetActive(false);
            upgradeBTN.interactable = false;
        }
    }

    public void Work()
    {
        base.Working();
    }
}
