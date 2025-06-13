using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventListener : MonoBehaviour
{
    public GameObject endScreen;
    private void Start()
    {
        // 訂閱 GameManager 的事件
        GameManager.Instance.OnPauseChanged += HandlePauseChanged;
    }

    private void OnDisable()
    {
        // 取消訂閱（避免記憶體洩漏）
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPauseChanged -= HandlePauseChanged;
        }
    }

    private void HandlePauseChanged(bool isPaused)
    {
        // 根據是否暫停，顯示或隱藏此 UI 面板
        if(endScreen != null){
            endScreen.SetActive(isPaused);

            GameObject winText = endScreen.transform.Find("WinText").gameObject;
            GameObject failedText = endScreen.transform.Find("FailedText").gameObject;

            winText.SetActive(GameManager.Instance.isWin);
            failedText.SetActive(GameManager.Instance.isDeath);
        }
    }
}
