using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Destination { Moving, Storage, EnergyGenerator, SpawnSlime, FoodFactory, Mixer, Trash };

public class Facility : MonoBehaviour
{
    public Slider[] hpSlider;
    public Image[] hpColor;
    public Text levelText;
    public Text maxSlimeText;
    public Text plusSlimeText;
    public Text generateRateText;
    public Text upgradeCostText;

    public Transform gTxtT;
    public Transform gTxtT2;
    public Transform slimePos;

    public Transform[] portalPos;
    public Transform CenterPos;

    protected int level; // �ǹ� ����
    protected int maxLevel;
    public int maxWorkSlime; // ���� ���ϴ� �η� ��
    public int currentSlimeNum; // �ִ� ���ϴ� �η� ��

    public SelectGameDifficulty GameDifficulty;

    private Animator animator;

    protected int upgradeCost;
    protected int plusSlime;

    public float generateTime; // ���� �ֱ�
    public List<Slime> slimeList;
    public List<Slime> generate;
    public int generateMount; // ���귮

    public int index;

    public bool over; 

    public int mixerE;
    public int mixerF;

    protected float delay;

    public GameObject slimePrefab;

    public GameObject generateText;
    public GameObject generateText2;

    public Image upgradeResource;

    public Button upgradeBTN;
    private void Start()
    {
        animator = GetComponent<Animator>();
        slimeList = new List<Slime>();
        maxLevel = 10;
    }

    // ����Ʈ���� ����
    public void MoveSlime(Slime slime, Destination destination, int start)
    {
        GameManager.instance.facilityList[start].slimeList.Remove(slime);
        GameManager.instance.facilityList[0].MoveSlime(slime, destination, delay);
        // UI ����
    }


    // �̵��ü����� ���
    public void MoveSlime(Slime slime, Destination destination, float moveTime)
    {
        StartCoroutine(Moving(slime, destination, moveTime));
    }

    public IEnumerator Moving(Slime slime, Destination destination, float moveTime)
    {
        slime.isMoving = true;
        slimeList.Add(slime);
        yield return new WaitForSeconds(moveTime);
        slime.gameObject.transform.localPosition = portalPos[(int)destination - 1].position;
        slimeList.Remove(slime);
        GameManager.instance.facilityList[(int)destination].RecieveSlime(slime);
        switch ((int)destination)
        {
            case 1:
                slime.gameObject.tag = "Storage";
                break;
            case 2:
                slime.gameObject.tag = "EnergyGenerator";
                break;
            case 3:
                slime.gameObject.tag = "SpawnSlime";
                break;
            case 4:
                slime.gameObject.tag = "FoodFactory";
                break;
            case 5:
                slime.gameObject.tag = "Mixer";
                break;
        }
        slime.isMoving = false;
        switch (slime.isWorking)
        {
            case true:
                slime.isWorking = false;
                break;
            case false:
                slime.isWorking = true;
                break;
        }
    }

    // ����Ʈ �߰�
    public void RecieveSlime(Slime slime)
    {
        slimeList.Add(slime);
        // UI ����
    }

    public void DisplayUpgrade(int level, int maxLevel, int maxWorkSlime, int plusSlime, float generateTime, int generateMount, int upgradeCost)
    {
        levelText.text = level.ToString() + " / " + maxLevel.ToString();
        maxSlimeText.text = "�ִ� �ϲ� �� " + maxWorkSlime.ToString();
        plusSlimeText.text = " + " + plusSlime.ToString();
        generateRateText.text = generateTime.ToString() + "�ʿ� �η´� " + generateMount.ToString() + "����";
        upgradeCostText.text = upgradeCost.ToString();
        if (level >= maxLevel)
        {
            upgradeCostText.text = "���׷��̵� �Ϸ�";
        }
    }

    public void UpgradeCostPerLevel()
    {
        switch (level)
        {
            case 1:
                upgradeCost = 100;
                plusSlime = 3;  //3
                break;
            case 2:
                upgradeCost = 150;
                plusSlime = 6; //6
                break;
            case 3:
                upgradeCost = 340;
                plusSlime = 12; //12
                break;
            case 4:
                upgradeCost = 720;
                plusSlime = 12; //24
                break;
            case 5:
                upgradeCost = 1500;
                plusSlime = 18; //36
                break;
            case 6:
                upgradeCost = 3150;
                plusSlime = 24; // 54
                break;
            case 7:
                upgradeCost = 5200;
                plusSlime = 30; //78
                break;
            case 8:
                upgradeCost = 7600;
                plusSlime = 36; //108
                break;
            case 9:
                upgradeCost = 10000;
                plusSlime = 42; // 144
                break;
                // 186
        }
    }

    public void Upgrade()
    {
        if (level < maxLevel)
        {
            if (GameManager.instance.energyManager.currentEnergy >= upgradeCost)
            {
                GameManager.instance.energyManager.currentEnergy -= upgradeCost;
                maxWorkSlime += plusSlime;
                level++;
            }
        }
        if(level == maxLevel)
        {
            upgradeResource.gameObject.SetActive(false);
            plusSlimeText.gameObject.SetActive(false);
            upgradeBTN.interactable = false;
        }
    }

    public void Working()
    {
        if (slimeList.Count > 0)
        {
            animator.SetBool("work", true);
        }
        else
        {
            animator.SetBool("work", false);
        }
    }

    public void Spawning()
    {
        Instantiate(slimePrefab);
        if (GameManager.instance.facilityList[(int)Destination.SpawnSlime].over)
        {
            slimePrefab.tag = "Trash";
        }
    }

    // ���귮 �ؽ�Ʈ ǥ��
    public void DisplayGenerated(GameObject generateTxt, Destination destination)
    {
        
        generateTxt.GetComponent<MakeText>().generate = GameManager.instance.facilityList[(int)destination].generateMount;
        Instantiate(generateTxt);
        generateTxt.transform.position = GameManager.instance.facilityList[(int)destination].gTxtT.position;
    }

    public void DisplayGeneratedMixer(GameObject generateText1, GameObject generateText2, int mixerE, int mixerF)
    {
        
        generateText1.GetComponent<MakeText>().generate = mixerE;
        Instantiate(generateText1);
        generateText1.transform.position = GameManager.instance.facilityList[(int)Destination.Mixer].gTxtT.position;
        
        generateText2.GetComponent<MakeText>().generate = mixerF;
        Instantiate(generateText2);
        generateText2.transform.position = GameManager.instance.facilityList[(int)Destination.Mixer].gTxtT2.position;
    }

    public void TrashSlime(Slime slime)
    {
        if (slime.gameObject.tag == "Trash")
        {
            slime.currentHP -= (slime.maxHP / 1.5f) * Time.deltaTime;
        }
    }

    public void WhenSlimeDead(Slime slime)
    {
        slimeList.Remove(slime);
        Destroy(slime.gameObject);
    }

    public void SetSpawnedSlimePosition()
    {
        if (!GameManager.instance.facilityList[(int)Destination.SpawnSlime].over)
        {
            slimePrefab.transform.position = GameManager.instance.facilityList[(int)Destination.Storage].slimePos.position;
        }
        else
        {
            slimePrefab.transform.position = GameManager.instance.facilityList[(int)Destination.Trash].slimePos.position;
        }
    }

    public void LimitSpawnSlime()
    {
        // ���������� �ϲ� ��
        int limitedSlime = GameManager.instance.maxSlimeNum - GameManager.instance.currentSlimeNum;
        // ���� �뷮�� 0 �̻��϶�
        if (limitedSlime > 0)
        {
            over = false;
        }
        // ���� ������ ���� �ִ� �����Ӽ��� �ʰ� ���� ��
        // �ʰ��и� ����
        // ���� ���� �ϲ��� �ͼ��⿡�� ����� �ʰ��ع����� ��� ����.

        else
        {
            over = true;
        }
    }
}
