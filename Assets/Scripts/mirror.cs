using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirror : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float fadeDuration = 1f;
    private AudioSource audioSource;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        StartCoroutine(FadeTo(0.6f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeTo(0f));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = sprite.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null;
        }

        // 最後再強制設定為目標 alpha，避免浮點誤差
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, targetAlpha);
    }

    void OnMouseDown()
    {
        count++;
        if(count == 1){
            audioSource.Play();
            FadeIn();
        }
        if(count == 2){
            count = 0;
            audioSource.Stop();
            FadeOut();
        }
    }
}
