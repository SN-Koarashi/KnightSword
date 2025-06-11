using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactor : MonoBehaviour
{
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public GameObject swordHitbox;      // 指向劍的碰撞區 GameObject
    public GameObject boundObject; // 拖曳限制用的物件進來
    public Transform foot;          // 角色腳的位置
    public BoxCollider2D attackCollider;
    public int health = 3;
    public float knockbackDuration = 0.3f; // 彈開持續時間
    public float hitboxDuration = 0.5f; // 啟用碰撞時間
    public float moveSpeed = 5f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private AudioSource audioSource;
    private Vector2 moveInput;
    private Vector2 targetPosition;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private float knockbackTimer = 0f;
    private float stillTimer = 0f;
    private float idleDelay = 0.1f;
    private bool isKnockedBack = false;
    private bool isWalking = false;
    private bool needPlayingAttackSound = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        targetPosition = rb.position;

        // 從指定的 GameObject 抓取邊界
        if (boundObject != null)
        {
            var bounds = boundObject.GetComponent<Collider2D>().bounds;
            minBounds = bounds.min;
            maxBounds = bounds.max;
        }
        else
        {
            Debug.LogWarning("未指定移動範圍的 GameObject！");
        }

        if (foot == null)
        {
            Debug.LogWarning("請指定腳的 Transform！");
        }

        animator.SetBool("isDragon", GameManager.Instance.isDragon);
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // 控制動畫狀態
        bool currentlyMoving = moveInput.sqrMagnitude > 0.01f;

        if (currentlyMoving)
        {
            stillTimer = 0f;
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            stillTimer += Time.deltaTime;
            if (isWalking && stillTimer > idleDelay)
            {
                isWalking = false;
                animator.SetBool("isWalking", false);
            }
        }

        // 控制左右翻轉
        if (moveInput.x > 0)
        {
            sprite.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            sprite.flipX = true;
        }

        if(attackCollider != null)
        {
            // 修改 BoxCollider2D 的 offset.x 為對應方向
            Vector2 offset = attackCollider.offset;
            offset.x = Mathf.Abs(offset.x) * (sprite.flipX ? -1 : 1);
            attackCollider.offset = offset;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isAttack", true);
            needPlayingAttackSound = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("isAttack", false);
            // needPlayingAttackSound = false;
        }
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                health--;

                if(health <= 0){
                    animator.SetBool("isDeath", true);

                    PauseGame();
                }
                else{
                    animator.SetBool("isHurt", false);
                }
            }
            return; // 彈開期間不控制位置
        }

        Vector2 nextPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        if (foot != null)
        {
            // 腳相對角色中心的偏移量
            Vector2 footOffset = (Vector2)foot.position - rb.position;

            // 腳下一步位置
            Vector2 footNextPos = nextPos + footOffset;

            // 限制腳的位置範圍
            footNextPos.x = Mathf.Clamp(footNextPos.x, minBounds.x, maxBounds.x);
            footNextPos.y = Mathf.Clamp(footNextPos.y, minBounds.y, maxBounds.y);

            // 因為腳不能超出範圍，所以角色位置要修正回去
            nextPos = footNextPos - footOffset;
        }
        else
        {
            // 沒指定腳就限制角色中心點移動範圍
            nextPos.x = Mathf.Clamp(nextPos.x, minBounds.x, maxBounds.x);
            nextPos.y = Mathf.Clamp(nextPos.y, minBounds.y, maxBounds.y);
        }

        rb.MovePosition(nextPos);
    }

    public void TriggerAttack(){
        StartCoroutine(ActivateHitbox());
    }

    public void TriggerKnockback()
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        animator.SetBool("isHurt", true);
        if(hurtSound != null){
            audioSource.PlayOneShot(hurtSound);
        }
    }

    public void PlayAttackSound(){
        if(attackSound != null){
            if(needPlayingAttackSound == true){
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    public void StopAttackSound(){
        if(attackSound != null){
            needPlayingAttackSound = false;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private IEnumerator ActivateHitbox()
    {
        if (swordHitbox == null) yield break;

        swordHitbox.SetActive(true); // 開啟碰撞區
        yield return new WaitForSeconds(hitboxDuration); // 等待 0.5 秒
        swordHitbox.SetActive(false); // 關閉碰撞區
    }
}
