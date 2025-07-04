﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    [SerializeField] GameObject damageTextPrefab; // 指向你做好的 prefab
    // 假設你已經有指向 World Space Canvas 的 transform（例如在 Start() 指派或從 Inspector 拖入）
    [SerializeField] private Transform canvasTransform;

    public GameObject healthBar;
    public AudioClip hurtSound;
    public AudioClip blockSound;
    public Transform target; // 玩家（或任何目標）
    public float maxHealth = 150f;
    public float speed = 1f;
    public float pushForce = 15f;

    private float health;
    private float initialSpeed;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        initialSpeed = speed;
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || GameManager.Instance.isPaused) return;

        // 計算方向
        Vector3 direction = (target.position - transform.position).normalized;

        // 移動物件
        transform.position += direction * speed * Time.deltaTime;

        // 根據目標的位置決定是否翻轉
        if (spriteRenderer != null)
        {
            if (target.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true; // 面向左
            }
            else
            {
                spriteRenderer.flipX = false; // 面向右
            }
        }

        speed = initialSpeed * (1f + (1f - (health / maxHealth)));
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 確認撞到的是玩家（可用 Tag 判斷）
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // 從敵人指向玩家的方向
                Vector2 pushDir = collision.transform.position - transform.position;
                pushDir.y = 0f;
                pushDir.Normalize();

                // 施加水平彈開力
                playerRb.velocity = Vector2.zero; // 避免力道疊加
                playerRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);

                charactor playerScript = collision.gameObject.GetComponent<charactor>();
                if (playerScript != null)
                {
                    playerScript.TriggerKnockback();
                }
            }
        }
    }

    public void ShowDamage(int amount, Vector3 worldPosition)
    {
        // 產生傷害文字物件
        GameObject obj = Instantiate(damageTextPrefab, worldPosition, Quaternion.identity, canvasTransform);
        Vector3 pos = obj.transform.position;
        pos.z = 0f;
        obj.transform.position = pos;
        // 設定數值
        obj.GetComponent<DamageText>().SetDamage(amount);
        obj.GetComponent<DamageText>().SetActive(true);
    }

    public void TakeDamage(){
        if(GameManager.Instance.canDamage){
            int damage = Random.Range(10, 21); // 注意：上限是「不包含」，所以要填 21

            // 5% 暴擊機率
            if (Random.value <= 0.05f)
            {
                damage *= 2;
                Debug.Log("暴擊！傷害為：" + damage);
            }
            else
            {
                Debug.Log("一般攻擊，傷害為：" + damage);
            }

            ShowDamage(damage, gameObject.transform.position + new Vector3(0, 1f, 0));

            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthUI();

            animator.SetBool("isHurt", true);
            if(hurtSound != null){
                audioSource.PlayOneShot(hurtSound);
            }

            StartCoroutine(ResumeHurt());
        }
        else{
            if(blockSound != null){
                audioSource.PlayOneShot(blockSound);
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            GameObject LockerIcon = healthBar.transform.Find("Locker").gameObject;
            Slider healthSlider = healthBar.transform.Find("SliderBar").GetComponent<Slider>();
            healthSlider.value = health / maxHealth;

            LockerIcon.SetActive(!GameManager.Instance.canDamage);
        }
    }

    private IEnumerator ResumeHurt()
    {
        yield return new WaitForSeconds(0.35f);
        if(health <= 0){
            StartCoroutine(FadeOutAndDestroy());
        }
        else{
            animator.SetBool("isHurt", false);
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float fadeDuration = 1f; // 淡出秒數
        float elapsed = 0f;

        Color originalColor = sr.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        if(healthBar != null){
            healthBar.SetActive(false);
        }


        GameManager.Instance.isWin = true;
        GameManager.Instance.SetPause(true);
        Destroy(gameObject); // 最後刪除物件
    }

}
