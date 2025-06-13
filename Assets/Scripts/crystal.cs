using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystal : MonoBehaviour
{
    public SpriteRenderer sr;
    private Coroutine fadeCoroutine;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        sr.color = new Color(1, 1, 1, 0);

        if(GameManager.Instance.canDamage)
        {
            ShowCrystal();
            count = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        count++;
        if (count == 1)
        {
            ShowCrystal();
        }
        else if (count == 2)
        {
            HideCrystal();
        }
    }

    void ShowCrystal()
    {
        // 開始透明度循環
        if (fadeCoroutine == null)
            fadeCoroutine = StartCoroutine(FadeCycle());

        GameManager.Instance.canDamage = true;
    }

    void HideCrystal()
    {
        // 停止循環並重設
        count = 0;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        sr.color = new Color(1, 1, 1, 0); // 透明度歸零

        GameManager.Instance.canDamage = false;
    }

    IEnumerator FadeCycle()
    {
        float alphaMin = 0.3f;
        float alphaMax = 0.7f;
        float speed = 1.5f;  // 控制變化速度
        float alpha = alphaMin;
        bool increasing = true;

        while (true)
        {
            if (increasing)
            {
                alpha += Time.deltaTime * speed;
                if (alpha >= alphaMax)
                {
                    alpha = alphaMax;
                    increasing = false;
                }
            }
            else
            {
                alpha -= Time.deltaTime * speed;
                if (alpha <= alphaMin)
                {
                    alpha = alphaMin;
                    increasing = true;
                }
            }

            sr.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}
