using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public EnergyManager energyManager;
    public EnergyGenerator energyGenerator;
    public SpawnSlime spawnSlime;
    public FoodFactory foodFactory;
    public Storage storage;

    public Text storageCurrentNum;
    public Text egSlimeNum;
    public Text ssSlimeNum;
    public Text ffSlimeNum;

    public Text SlimeNum;
    public Text EnergyNum;
    public Text FoodNum;
    public Text MaxSlimeNum;
    public Text MaxEnergyNum;
    public Text MaxFoodNum;

    public Text GatheringEnergy;
    public Text GatheringFood;

    public Text Date;
    public Text timeLimit;
    public Image[] toNext;

    private static UIManager m_instance;

    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayResource();
        GatheringResource();
        DisplayTime();
    }
    
    public void DisplayResource()
    {
        SlimeNum.text = GameManager.instance.currentSlimeNum.ToString();
        MaxSlimeNum.text = " / " + GameManager.instance.maxSlimeNum.ToString();
        EnergyNum.text = energyManager.currentEnergy.ToString();
        MaxEnergyNum.text = " / " + energyManager.maxEnergy.ToString();
        FoodNum.text = energyManager.currentFood.ToString();
        MaxFoodNum.text = " / " + energyManager.maxFood.ToString();
    }

    public void GatheringResource()
    {
        GatheringEnergy.text = "- "+Mathf.Round(EnergyManager.instance.maxEnergy * EnergyManager.instance.gatheringPerDate).ToString();
        GatheringFood.text = "- " + GameManager.instance.currentSlimeNum.ToString();
    }

    public void DisplayTime()
    {
        Date.text = GameManager.instance.date.ToString();
        timeLimit.text = GameManager.instance.timeLimit.ToString("F0");
        if(GameManager.instance.perLimit  < GameManager.instance.toNextDay)
        {
            timeLimit.color = new Color(95 / 255f, 205 / 255f, 228 / 255f);
        }
        else
        {
            timeLimit.color = new Color(227 / 255f, 80 / 255f, 80 / 255f);
        }
        for (int i = 0; i < GameManager.instance.perLimit; i++)
        {
            toNext[i].gameObject.SetActive(true);
        }
    }
}
