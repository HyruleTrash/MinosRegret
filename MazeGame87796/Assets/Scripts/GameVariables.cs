using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameVariables : MonoBehaviour
{
    public int Coins = 0;
    public int HatsMerged = 0;
    public int PowerUpsCollected = 0;
    public float PlayTime = 0f;

    public int Gamestage = 0;

    public static GameVariables instance;

    float time = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (PlayTime == 0)
        {
            time += Time.deltaTime;
        }
    }

    public void SetPlayTime()
    {
        PlayTime = time;
    }
}
