using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float hpPercentage;

    public float delta;

    private float decreaseHpMount;

    public bool isWorking;
    public bool isMoving;
    public bool isCenter;
    public bool isDead;
    public bool cantGenerate = false;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.tag = "Storage";
        maxHP = GameManager.instance.slimeMaxHP;
        currentHP = maxHP;
        decreaseHpMount = 1f;

    }

    public void Start()
    {
        StartCoroutine(OnUpdate());
        sprite = GetComponent<SpriteRenderer>();
        if (!GameManager.instance.facilityList[(int)Destination.SpawnSlime].over)
        {
            GameManager.instance.facilityList[1].slimeList.Insert(0, this);
        }
        else
        {
            GameManager.instance.facilityList[6].slimeList.Insert(0, this);
        }
        isCenter = false;
    }

    // Update is called once per frame
    void Update()
    {
        Die();
        MoveToCenter();
        TurnBackFlip();
        maxHP = GameManager.instance.slimeMaxHP;
        hpPercentage = currentHP / maxHP;
        if (!isMoving)
        {
            isCenter = false;
        }
        GameManager.instance.facilityList[(int)Destination.Trash].TrashSlime(this);
    }

    public void DecreaseHP()
    {
        if (!GameManager.instance.isPaused)
        {
            if (isWorking)
            {
                if (gameObject.tag == "Mixer")
                {
                    currentHP -= (maxHP / 1f) * Time.deltaTime;
                }

                else
                {
                    currentHP -= decreaseHpMount * Time.deltaTime;
                }
            }
        }
    }

    public void RestoreHP()
    {
        if (!GameManager.instance.isPaused)
        {
            if (currentHP < maxHP)
            {
                currentHP += maxHP / 70 * Time.deltaTime;
                if (currentHP > maxHP)
                {
                    currentHP = maxHP;
                }
            }
        }
    }

    // while(true)를 통해 Working 코루틴을 계속 실행 시킴
    public IEnumerator OnUpdate()
    {
        while (true)
        {
            yield return StartCoroutine(Working());
        }
    }

    public void TurnBackFlip()
    {
        if (isMoving && sprite.flipX == true)
        {
            sprite.flipX = false;
        }
    }

    // isWorking값을 통해 코루틴 실행 및 종료
    public IEnumerator Working()
    {
        if (!GameManager.instance.isGameOver)
        {
            while (isWorking)
            {
                DecreaseHP();
                yield return null;
                if (gameObject.tag == "EnergyGenerator")
                {
                    GenerateEnergy();
                }
                if (gameObject.tag == "SpawnSlime")
                {
                    SpawningSlime();
                }
                if (gameObject.tag == "FoodFactory")
                {
                    GenerateFood();
                }
            }
            while (!isWorking)
            {
                RestoreHP();
                delta = 0;
                yield return null;
            }
        }
    }

    public void MoveToCenter()
    {
        if (isMoving)
        {
            if (!isCenter)
            {
                switch (gameObject.tag)
                {
                    case "Storage":
                        gameObject.transform.position = GameManager.instance.facilityList[1].CenterPos.position;
                        break;
                    case "EnergyGenerator":
                        gameObject.transform.position = GameManager.instance.facilityList[2].CenterPos.position;
                        break;
                    case "SpawnSlime":
                        gameObject.transform.position = GameManager.instance.facilityList[3].CenterPos.position;
                        break;
                    case "FoodFactory":
                        gameObject.transform.position = GameManager.instance.facilityList[4].CenterPos.position;
                        break;
                }
                isCenter = true;
            }
            transform.Translate(new Vector3(0, 10 * Time.deltaTime, 0));
        }
    }

    public void MovingFacility()
    {
        if (isMoving)
        {
            transform.Translate(new Vector3(0, 10 * Time.deltaTime, 0));
        }
    }

    public void Die()
    {
        if (currentHP <= 0)
        {
            switch (gameObject.tag)
            {
                case "EnergyGenerator":
                    GameManager.instance.facilityList[(int)Destination.EnergyGenerator].WhenSlimeDead(this);
                    break;
                case "SpawnSlime":
                    GameManager.instance.facilityList[(int)Destination.SpawnSlime].WhenSlimeDead(this);
                    break;
                case "FoodFactory":
                    GameManager.instance.facilityList[(int)Destination.FoodFactory].WhenSlimeDead(this);
                    break;
                case "Mixer":
                    GameManager.instance.facilityList[(int)Destination.Mixer].WhenSlimeDead(this);

                    float overE = EnergyManager.instance.maxEnergy - EnergyManager.instance.currentEnergy - GameManager.instance.facilityList[(int)Destination.Mixer].mixerE;
                    if (overE <= 0)
                    {
                        EnergyManager.instance.currentEnergy += (GameManager.instance.facilityList[(int)Destination.Mixer].mixerE + overE);
                    }
                    else
                    {
                        EnergyManager.instance.currentEnergy += GameManager.instance.facilityList[(int)Destination.Mixer].mixerE;

                    }
                    float overF = EnergyManager.instance.maxFood - EnergyManager.instance.currentFood - GameManager.instance.facilityList[(int)Destination.Mixer].mixerF;
                    if (overF <= 0)
                    {
                        EnergyManager.instance.currentFood += (GameManager.instance.facilityList[(int)Destination.Mixer].mixerF + overF);
                    }
                    else
                    {
                        EnergyManager.instance.currentFood += GameManager.instance.facilityList[(int)Destination.Mixer].mixerF;

                    }

                    GameManager.instance.facilityList[(int)Destination.Mixer].DisplayGeneratedMixer(GameManager.instance.facilityList[(int)Destination.Mixer].generateText,
                        GameManager.instance.facilityList[(int)Destination.Mixer].generateText2, GameManager.instance.facilityList[(int)Destination.Mixer].mixerE,
                        GameManager.instance.facilityList[(int)Destination.Mixer].mixerF);
                    break;
                case "Trash":
                    GameManager.instance.facilityList[(int)Destination.Trash].WhenSlimeDead(this);
                    break;

            }
        }
    }

    // 에너지 생산
    public void GenerateEnergy()
    {
        if (!GameManager.instance.isPaused || !GameManager.instance.isGameOver || !GameManager.instance.isGameClear)
        {
            delta += Time.deltaTime;
            if (delta > GameManager.instance.facilityList[(int)Destination.EnergyGenerator].generateTime)
            {
                delta = 0;
                GameManager.instance.facilityList[(int)Destination.EnergyGenerator].DisplayGenerated(GameManager.instance.facilityList[(int)Destination.EnergyGenerator].generateText, Destination.EnergyGenerator);

                float overE = EnergyManager.instance.maxEnergy - EnergyManager.instance.currentEnergy - GameManager.instance.facilityList[(int)Destination.EnergyGenerator].generateMount;
                if (overE <= 0)
                {
                    EnergyManager.instance.currentEnergy += (GameManager.instance.facilityList[(int)Destination.EnergyGenerator].generateMount + overE);
                }
                else
                {
                    EnergyManager.instance.currentEnergy += GameManager.instance.facilityList[(int)Destination.EnergyGenerator].generateMount;

                }
            }
        }
    }
    // 슬라임 생산
    public void SpawningSlime()
    {
        if (!GameManager.instance.isPaused || !GameManager.instance.isGameOver || !GameManager.instance.isGameClear)
        {
            delta += Time.deltaTime;
            if (delta > GameManager.instance.facilityList[3].generateTime)
            {
                GameManager.instance.facilityList[(int)Destination.SpawnSlime].LimitSpawnSlime();
                GameManager.instance.facilityList[(int)Destination.SpawnSlime].DisplayGenerated(GameManager.instance.facilityList[(int)Destination.SpawnSlime].generateText, Destination.SpawnSlime);
                if (!GameManager.instance.facilityList[(int)Destination.SpawnSlime].over)
                {
                    delta = 0;
                    GameManager.instance.facilityList[(int)Destination.Storage].Spawning();
                    GameManager.instance.facilityList[(int)Destination.Storage].SetSpawnedSlimePosition();
                }
                else
                {
                    delta = 0;
                    GameManager.instance.facilityList[(int)Destination.Trash].Spawning();
                    GameManager.instance.facilityList[(int)Destination.Storage].SetSpawnedSlimePosition();
                }

            }
        }
    }
    // 음식 생산
    public void GenerateFood()
    {
        if (!GameManager.instance.isPaused || !GameManager.instance.isGameOver || !GameManager.instance.isGameClear)
        {
            delta += Time.deltaTime;
            if (delta > GameManager.instance.facilityList[4].generateTime)
            {
                delta = 0;
                GameManager.instance.facilityList[(int)Destination.FoodFactory].DisplayGenerated(GameManager.instance.facilityList[(int)Destination.FoodFactory].generateText, Destination.FoodFactory);
                float overF = EnergyManager.instance.maxFood - EnergyManager.instance.currentFood - GameManager.instance.facilityList[(int)Destination.FoodFactory].generateMount;
                if (overF <= 0)
                {
                    EnergyManager.instance.currentFood += (GameManager.instance.facilityList[(int)Destination.FoodFactory].generateMount + overF);
                }
                else
                {
                    EnergyManager.instance.currentFood += GameManager.instance.facilityList[(int)Destination.FoodFactory].generateMount;

                }
            }
        }
    }
}
