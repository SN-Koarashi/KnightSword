using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool canDamage = false;
    public bool isPaused = false;
    public bool isDeath = false;
    public bool isWin = false;

    public event Action<bool> OnPauseChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切場景不銷毀
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPause(bool pause)
    {
        if (isPaused != pause)
        {
            isPaused = pause;

            // 觸發事件，通知訂閱者
            OnPauseChanged?.Invoke(isPaused);
        }
    }
}
