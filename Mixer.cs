using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mixer : Facility
{
    public Text currentNum;
    public Text MixerE;
    public Text MixerF;

    public Text PMixerE;
    public Text PMixerF;


    public Text rateE;
    public Text rateF;

    public void Awake()
    {
        mixerE = 3;
        mixerF = 4;
        level = 1;
        maxLevel = 10;
        maxWorkSlime = 99;
        delay = 1.5f;
    }
    // Update is called once per frame
    void Update()
    {
        currentSlimeNum = slimeList.Count;
        SortByHpUP();
        UpgradeCost();
        DisplayUpgrade();
        Work();
    }

    public void DisplayUpgrade()
    {
        levelText.text = level.ToString() + " / " + maxLevel.ToString();
        upgradeCostText.text = upgradeCost.ToString();
        MixerE.text = "분해시 얻는 에너지량";
        MixerF.text = "분해시 얻는 음식량";
        PMixerE.text = " + 1";
        PMixerF.text = " + 1";
        rateE.text = "+" + mixerE.ToString();
        rateF.text = "+" + mixerF.ToString();

        if (level >= maxLevel)
        {
            upgradeCostText.text = "업그레이드 완료";
            PMixerE.gameObject.SetActive(false);
            PMixerF.gameObject.SetActive(false);
            upgradeBTN.interactable = false;
        }
    }

    public void Requesting()
    {
        if (GameManager.instance.facilityList[1].slimeList.Count > 0 && currentSlimeNum + GameManager.instance.facilityList[0].currentSlimeNum < maxWorkSlime && GameManager.instance.facilityList[0].currentSlimeNum < GameManager.instance.facilityList[0].maxWorkSlime)
        {
            base.MoveSlime(GameManager.instance.facilityList[1].slimeList[0], Destination.Mixer, (int)Destination.Storage);
            index++;
        }
    }

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

    public void UpgradeCost()
    {
        switch (level)
        {
            case 1:
                upgradeCost = 100;
                break;
            case 2:
                upgradeCost = 150;
                break;
            case 3:
                upgradeCost = 340;
                break;
            case 4:
                upgradeCost = 720;
                break;
            case 5:
                upgradeCost = 1200;
                break;
            case 6:
                upgradeCost = 2150;
                break;
            case 7:
                upgradeCost = 5200;
                break;
            case 8:
                upgradeCost = 7600;
                break;
            case 9:
                upgradeCost = 10000;
                break;
                // 620
        }
    }

    public void UpgradeM()
    {
        if (level < maxLevel)
        {
            if (GameManager.instance.energyManager.currentEnergy >= upgradeCost)
            {
                GameManager.instance.energyManager.currentEnergy -= upgradeCost;
                mixerE += 1;
                mixerF += 1;
                level++;
            }
        }
        if (level == maxLevel)
        {
            upgradeResource.gameObject.SetActive(false);
            upgradeBTN.interactable = false;
        }
    }

    public void Work()
    {
        base.Working();
    }

}
