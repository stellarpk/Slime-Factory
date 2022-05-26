using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    private static EnergyManager m_instance;

    public float maxEnergy;
    public float currentEnergy;

    public float maxFood;
    public float currentFood;

    public float gatheringPerDate;

    public static EnergyManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<EnergyManager>();
            }
            return m_instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        maxEnergy = 150;
        currentEnergy = 150;
        maxFood = 50;
        currentFood = 30;
        gatheringPerDate = 0.165f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseMaxEperDate(float increase)
    {
        maxEnergy += increase;
    }

    public void GatheringEFperDate()
    {
        currentEnergy -= Mathf.Round(maxEnergy * gatheringPerDate);
        currentFood -= GameManager.instance.currentSlimeNum;
    }
}
