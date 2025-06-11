using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lamp : MonoBehaviour
{
    private bool isVisible = false;
    private Vector3 originalPos;
    public SpriteRenderer sr;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalPos = transform.position;
        sr.color = new Color(1, 1, 1, 0); // 初始透明
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        StopAllCoroutines();

        // 顯示鬼
        if (!isVisible){
            audioSource.Play();
            StartCoroutine(MoveAndFade(originalPos.x - 0.5f, 1f));
        }
        else{
            audioSource.Stop();
            StartCoroutine(MoveAndFade(originalPos.x, 0f));
        }

        isVisible = !isVisible;
    }

    System.Collections.IEnumerator MoveAndFade(float targetX, float targetAlpha)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        float startAlpha = sr.color.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float newX = Mathf.Lerp(startPos.x, targetX, t);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            transform.position = new Vector3(newX, startPos.y, startPos.z);
            sr.color = new Color(1, 1, 1, newAlpha);

            yield return null;
        }

        // 確保精確落點
        transform.position = new Vector3(targetX, startPos.y, startPos.z);
        sr.color = new Color(1, 1, 1, targetAlpha);
    }
}
