using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public int maxSlimeNum;
    public int currentSlimeNum;

    public int date; // 날짜
    public int maxDate;
    public int perLimit; // 다음날짜로 넘어가기 위한 최대 수
    public int toNextDay; // 다음날짜로 넘어가기까지 남은 수
    public float timeLimit; // 제한시간
    public float maxTimeLimit = 20;
    public float maxTimeLimitHard = 15;
    public float increaseMount; // 에너지 증가량

    public int overMaxSlime;

    public int slimeMaxHP;
    public int slimePlusHP;

    public bool isPaused; // 게임 일시정지용
    public bool isGameOver; // 게임 오버 확인용 
    public bool isGameClear; // 게임 클리어 확인용

    public Facility[] facilityList = new Facility[7];  // 0: 이동시설, 1: 보관소, 2: 에너지생산소, 3: 슬라임생산소, 4: 음식생산소, 5: 믹서기

    public GameObject slime;
    public GameObject limit;

    public SelectGameDifficulty GameDifficulty;

    public GameObject gameoverUI;
    public GameObject gameclearUI;

    public Image timeSlider;

    public EnergyManager energyManager;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    private void Awake()
    {
        GameDifficulty = GameObject.Find("SelectDifficulty").GetComponent<SelectGameDifficulty>();
        if (instance != this)
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            Instantiate(slime);
            slime.transform.position = facilityList[1].slimePos.position;
        }
        if (GameDifficulty.difficulty == Difficulty.Normal)
        {
            timeLimit = maxTimeLimit;

        }
        else if (GameDifficulty.difficulty == Difficulty.Hard)
        {
            timeLimit = maxTimeLimitHard;
        }
        maxSlimeNum = 15;
        date = 1;
        perLimit = 0;
        toNextDay = 5;
        increaseMount = 50;
        maxDate = 20;

        isGameClear = false;
        isGameOver = false;
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Update()
    {
        currentSlimeNum = facilityList[0].currentSlimeNum +
            facilityList[1].currentSlimeNum + 
            facilityList[2].currentSlimeNum + 
            facilityList[3].currentSlimeNum + 
            facilityList[4].currentSlimeNum + 
            facilityList[5].currentSlimeNum;
        overMaxSlime = currentSlimeNum + facilityList[3].currentSlimeNum - maxSlimeNum;
        TimeUpdate();
        GameOver();
        GameClear();
        ChangeTimeSliderColor();
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else if (!isPaused)
        {
            Time.timeScale = 1f;
        }

        if (isGameClear || isGameOver)
        {
            Time.timeScale = 0;
        }
    }


    public void TimeUpdate()
    {
        if (!isGameOver)
        {
            if (!isPaused)
            {
                if (date <= maxDate)
                {
                    timeLimit -= Time.deltaTime;
                    if (GameDifficulty.difficulty == Difficulty.Normal)
                    {
                        timeSlider.fillAmount = timeLimit / maxTimeLimit;
                    }
                    else if (GameDifficulty.difficulty == Difficulty.Hard)
                    {
                        timeSlider.fillAmount = timeLimit / maxTimeLimitHard;
                    }
                        
                    if (timeLimit <= 0)
                    {
                        perLimit++;
                        if (GameDifficulty.difficulty == Difficulty.Normal)
                        {
                            timeLimit = maxTimeLimit;
                        }
                        else if (GameDifficulty.difficulty == Difficulty.Hard)
                        {
                            timeLimit = maxTimeLimitHard;
                        }
                        energyManager.GatheringEFperDate();
                        timeSlider.fillAmount = 1.0f;
                    }
                    if (perLimit > toNextDay)
                    {
                        date++;
                        perLimit = 0;
                        energyManager.IncreaseMaxEperDate(increaseMount);
                        increaseMount += 50;
                        for (int i = 0; i < UIManager.instance.toNext.Length; i++)
                        {
                            UIManager.instance.toNext[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    public void ChangeTimeSliderColor()
    {
        if (perLimit < toNextDay)
        {
            timeSlider.color = new Color(95 / 255f, 205 / 255f, 228 / 255f);
            limit.SetActive(false);
        }
        else
        {
            timeSlider.color = new Color(227 / 255f, 80 / 255f, 80 / 255f);
            limit.SetActive(true);
        }
    }

    public void GameOver()
    {
        if (energyManager.currentEnergy < 0 || energyManager.currentFood < 0 || currentSlimeNum <= 0)
        {
            isGameOver = true;
            gameoverUI.SetActive(true);
        }
    }

    public void GameClear()
    {
        if (date > maxDate && !isGameOver)
        {
            isGameClear = true;
            gameclearUI.SetActive(true);
        }
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            isPaused = false;
        }
        else if (!isPaused)
        {
            isPaused = true;
        }
    }

}
