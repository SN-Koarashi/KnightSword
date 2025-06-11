using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int health = 5;
    public Transform target; // 玩家（或任何目標）
    public float speed = 2f;
    public float pushForce = 15f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

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

    public void TakeDamage(){
        Debug.Log("Trigger damage");
        health--;

        animator.SetBool("isHurt", true);
        StartCoroutine(ResumeHurt());
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

        Destroy(gameObject); // 最後刪除物件
    }

}
