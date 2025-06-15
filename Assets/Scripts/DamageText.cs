using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text textFront;
    public Text textBack;
    public float floatUpSpeed = 1f;
    public float fadeDuration = 1f;
    public Vector3 floatOffset = new Vector3(0, 2f, 0);

    private float timer;
    private Color originalColorFront;
    private Color originalColorBack;

    void Start()
    {
        originalColorFront = textFront.color;
        originalColorBack = textBack.color;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetDamage(int amount)
    {
        textFront.text = amount.ToString();
        textBack.text = amount.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        // 漸漸往上飄
        transform.position += Vector3.up * floatUpSpeed * Time.deltaTime;

        // 漸漸變透明
        float alphaF = Mathf.Lerp(originalColorFront.a, 0, timer / fadeDuration);
        textFront.color = new Color(originalColorFront.r, originalColorFront.g, originalColorFront.b, alphaF);

        // 漸漸變透明
        float alphaB = Mathf.Lerp(originalColorBack.a, 0, timer / fadeDuration);
        textBack.color = new Color(originalColorBack.r, originalColorBack.g, originalColorBack.b, alphaB);

        // 時間到就自動刪除
        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
