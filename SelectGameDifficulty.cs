using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum Difficulty { Normal, Hard};

public class SelectGameDifficulty : MonoBehaviour
{
    public Difficulty difficulty;
    public GameObject btnNormal;
    public GameObject btnHard;
    public Button normal;
    public Button hard;

    private void Awake()
    {
        var objs = FindObjectsOfType<SelectGameDifficulty>();
        if (objs.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        btnNormal = GameObject.Find("Normal");
        btnHard = GameObject.Find("Hard");
    }

    private void Start()
    {
        btnNormal.GetComponent<Button>().onClick.AddListener(NormalMode);
        btnHard.GetComponent<Button>().onClick.AddListener(HardMode);
    }

    public void Update()
    {
        if (btnNormal == null)
        {
            btnNormal = GameObject.Find("Normal");
        }
        if (btnHard == null)
        {
            btnHard = GameObject.Find("Hard");
        }

        if (btnNormal != null)
        {
            btnNormal.GetComponent<Button>().onClick.AddListener(NormalMode);
        }
        if (btnHard != null)
        {
            btnHard.GetComponent<Button>().onClick.AddListener(HardMode);
        }
    }

    public void NormalMode()
    {
        difficulty = Difficulty.Normal;
    }

    public void HardMode()
    {
        difficulty = Difficulty.Hard;
    }
}
