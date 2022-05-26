using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnSlime : Facility
{
    public Text ssSlimeNum;

    public void Awake()
    {

        GameDifficulty = GameObject.Find("SelectDifficulty").GetComponent<SelectGameDifficulty>();
        level = 1;
        maxLevel = 10;
        maxWorkSlime = 3;
        generateMount = 1;

        if (GameDifficulty.difficulty == Difficulty.Normal)
        {
            generateTime = 7f;
        }
        else if (GameDifficulty.difficulty == Difficulty.Hard)
        {
            generateTime = 8f;
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
        SortByGenerateTime();
        Displaying();
        Work();
    }

    // +
    public void Requesting()
    {
        if (GameManager.instance.facilityList[1].slimeList.Count > 0 && currentSlimeNum + GameManager.instance.facilityList[0].currentSlimeNum < maxWorkSlime && GameManager.instance.facilityList[0].currentSlimeNum < GameManager.instance.facilityList[0].maxWorkSlime)
        {
            base.MoveSlime(GameManager.instance.facilityList[1].slimeList[0], Destination.SpawnSlime, (int)Destination.Storage);
            index++;
        }
    }

    // -
    public void Sending()
    {
        if (GameManager.instance.facilityList[3].slimeList.Count > 0 && GameManager.instance.facilityList[0].currentSlimeNum < GameManager.instance.facilityList[0].maxWorkSlime)
        {
            base.MoveSlime(GameManager.instance.facilityList[3].slimeList[0], Destination.Storage, (int)Destination.SpawnSlime);
            index--;
        }
    }

    public void Display()
    {
        ssSlimeNum.text = currentSlimeNum.ToString() + " / " + maxWorkSlime.ToString();
    }

    public void SortByGenerateTime()
    {
        generate = slimeList.ToList();
        generate.Sort(delegate (Slime slime1, Slime slime2)
        {
            if (slime1.delta > slime2.delta)
            {
                return -1;
            }
            else if (slime1.delta < slime2.delta)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        });
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
