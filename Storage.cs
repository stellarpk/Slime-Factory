using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : Facility
{
    public Text sSlimeNum;
    public Text plusHPMount;
    public Text PlusMaxHP;
    public void Awake()
    {
        level = 1;
        maxLevel = 10;
        generateMount = 14;
    }

    public void Start()
    {

    }
    public void Update()
    {
        currentSlimeNum = slimeList.Count;
        Display();
        SortByHpUP();
        DisplayUpgrade();
        UpgradeCost();
    }

    public void Display()
    {
        sSlimeNum.text = currentSlimeNum.ToString();
    }

    // 체력 오름차순
    public void SortByHpUP()
    {

        slimeList.Sort(delegate (Slime slime1, Slime slime2)
        {
            if (slime1.currentHP > slime2.currentHP)
            {
                return -1;
            }

            else if (slime1.currentHP < slime2.currentHP)
            {
                return 1;
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
                for(int j = slimeList.Count; j < hpSlider.Length; j++)
                {
                    hpSlider[j].gameObject.SetActive(false);
                }
            }
            if (hpSlider[i].gameObject.activeSelf)
            {
                hpSlider[i].value = slimeList[i].hpPercentage;
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
    public void DisplayUpgrade()
    {
        levelText.text = level.ToString() + " / " + maxLevel.ToString();
        maxSlimeText.text = "최대 슬라임 수 "+GameManager.instance.maxSlimeNum.ToString();
        plusSlimeText.text = " + "+plusSlime.ToString();
        upgradeCostText.text = upgradeCost.ToString();
        plusHPMount.text = "슬라임 최대 체력 " + GameManager.instance.slimeMaxHP.ToString();
        PlusMaxHP.text = " + "+GameManager.instance.slimePlusHP.ToString();
        if (level >= maxLevel)
        {
            upgradeCostText.text = "업그레이드 완료";
        }
    }

    public void UpgradeCost()
    {
        switch (level)
        {
            case 1:
                upgradeCost = 100;
                plusSlime = 5;  //15
                GameManager.instance.slimePlusHP = 50; // 70
                break;
            case 2:
                upgradeCost = 150;
                plusSlime = 10; //20
                GameManager.instance.slimePlusHP = 70; // 120
                break;
            case 3:
                upgradeCost = 340;
                plusSlime = 20; //30
                GameManager.instance.slimePlusHP = 60; // 190
                break;
            case 4:
                upgradeCost = 720;
                plusSlime = 30; //50
                GameManager.instance.slimePlusHP = 60; // 250
                break;
            case 5:
                upgradeCost = 1200;
                plusSlime = 25;
                GameManager.instance.slimePlusHP = 60; // 310
                break;
            case 6:
                upgradeCost = 2150;
                plusSlime = 50;
                GameManager.instance.slimePlusHP = 70; // 370
                break;
            case 7:
                upgradeCost = 5200;
                plusSlime = 60;
                GameManager.instance.slimePlusHP = 60; // 440
                break;
            case 8:
                upgradeCost = 7600;
                plusSlime = 70;
                GameManager.instance.slimePlusHP = 60; // 500
                break;
            case 9:
                upgradeCost = 10000;
                plusSlime = 80;
                GameManager.instance.slimePlusHP = 60; // 560
                break;
                // 620
        }
    }

    public void UpgradeS()
    {
        if (level < maxLevel)
        {
            if (GameManager.instance.energyManager.currentEnergy >= upgradeCost)
            {
                GameManager.instance.energyManager.currentEnergy -= upgradeCost;
                GameManager.instance.maxSlimeNum += plusSlime;
                GameManager.instance.slimeMaxHP += GameManager.instance.slimePlusHP;
                for (int i = 0; i < GameManager.instance.facilityList.Length; i++)
                {
                    for (int j = 0; j < GameManager.instance.facilityList[i].slimeList.Count; j++)
                    {
                        GameManager.instance.facilityList[i].slimeList[j].currentHP += (GameManager.instance.slimePlusHP * GameManager.instance.facilityList[i].slimeList[j].hpPercentage);
                    }
                }
                level++;
            }
        }
        if (level == maxLevel)
        {
            upgradeResource.gameObject.SetActive(false);
            plusSlimeText.gameObject.SetActive(false);
            PlusMaxHP.gameObject.SetActive(false);
            upgradeBTN.interactable = false;
        }
    }
}
