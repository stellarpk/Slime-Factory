using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingSlime : Facility
{
    public Text plus;
    public void Awake()
    {
        level = 1;
        maxLevel = 5;
        maxWorkSlime = 2;
    }

    // Update is called once per frame
    void Update()
    {
        currentSlimeNum = slimeList.Count;
        Display();
        UpgradeCost();
    }

    public void UpgradeCost()
    {
        switch (level)
        {
            case 1:
                upgradeCost = 75;
                plusSlime = 1;  //2
                break;
            case 2:
                upgradeCost = 125;
                plusSlime = 1; //3
                break;
            case 3:
                upgradeCost = 300;
                plusSlime = 2; //4
                break;
            case 4:
                upgradeCost = 450;
                plusSlime = 3; //6
                break;
            case 5:
                upgradeCost = 625;
                plusSlime = 4; //9
                break;
            case 6:
                upgradeCost = 1250;
                plusSlime = 5; // 13
                break;
            case 7:
                upgradeCost = 2625;
                plusSlime = 6; //18
                break;
            case 8:
                upgradeCost = 5400;
                plusSlime = 7; //24
                break;
            case 9:
                upgradeCost = 10000;
                plusSlime = 9; // 31
                break;
                //40
        }
    }

    public void Display()
    {
        levelText.text = level.ToString() + " / " + maxLevel.ToString();
        maxSlimeText.text = "동시 이동 가능 수 " + maxWorkSlime.ToString();
        plus.text = " + ";
        plusSlimeText.text = plusSlime.ToString();
        upgradeCostText.text = upgradeCost.ToString();
        if (level >= maxLevel)
        {
            upgradeCostText.text = "업그레이드 완료";
            plusSlimeText.gameObject.SetActive(false);
            upgradeResource.gameObject.SetActive(false);
            plus.gameObject.SetActive(false);
            upgradeBTN.interactable = false;
        }
    }

    public void LevelUP()
    {
        base.Upgrade();
    }
}
