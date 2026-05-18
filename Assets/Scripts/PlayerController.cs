using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Ajustar fuerza de salto
    public float jumpForce = 5f;

    [Header("Death")]
    public string deadPlayerLayerName = "DeadPlayer";

    // Tiempo antes de mostrar Game Over
    public float deathDelay = 9.2f;

    [Header("Powers")]
    public bool canDoubleJump = true;
    public bool hasBalderProtection = false;


    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool doubleJumpAvailable = false;
    private bool isDead = false;

    private SpriteRenderer sr;
    private Animator animator;
    private GameUIManager gameUIManager;

    public GameObject auraBalder;
    public GameObject yngviHalo;

    private CapsuleCollider2D playerCollider;
    private float startX;
    private float defaultGravityScale;
    public GameObject lokiEffect;




    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        gameUIManager = FindFirstObjectByType<GameUIManager>();

        startX = transform.position.x;
        defaultGravityScale = rb.gravityScale;

        if (yngviHalo != null)
            yngviHalo.SetActive(false);

        if (lokiEffect != null)
            lokiEffect.SetActive(false);

        // Estado inicial del Animator
        if (animator != null)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.speed = 1f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFlipping", false);
            animator.SetBool("isDead", false);
        }

        if (auraBalder != null)
            auraBalder.SetActive(false);
    }

    void Update()
    {
        // Si el personaje ha muerto, no permitimos más acciones
        if (isDead)
            return;

        // Actualizamos el parámetro de animación para el salto
        if (animator != null)
        {
            animator.SetBool("isJumping", !isGrounded);
        }

        // Salto normal desde el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            doubleJumpAvailable = true;

            if (animator != null)
            {
                animator.SetBool("isFlipping", false);
            }
        }
        // Doble salto en el aire
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && doubleJumpAvailable && canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            doubleJumpAvailable = false;

            if (animator != null)
            {
                animator.SetBool("isFlipping", true);
            }
        }

        // Modificación para que el personaje caiga más rápido de lo que sube
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        }

        // Evitamos que el jugador se quede atrás o avance por choques laterales
        if (!isDead)
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        if (auraBalder != null)
        {
            auraBalder.SetActive(hasBalderProtection);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Al tocar el suelo desde arriba, se reinician los saltos
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    doubleJumpAvailable = false;

                    if (animator != null)
                    {
                        animator.SetBool("isJumping", false);
                        animator.SetBool("isFlipping", false);
                    }

                    break;
                }
            }
        }

        // Muerte al chocar con obstáculo/enemigo
        if ((collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy")) && !isDead)
        {
            StompableEnemy stompableEnemy = collision.gameObject.GetComponent<StompableEnemy>();

            // Si el jugador cae encima de un enemigo pisable, lo elimina
            if (stompableEnemy != null && IsStompingEnemy(collision))
            {
                stompableEnemy.Die();

                // Pequeño rebote al pisar enemigo
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.6f);

                return;
            }

            // Protección de Balder: absorbe un impacto y destruye el obstáculo/enemigo
            if (hasBalderProtection)
            {
                hasBalderProtection = false;
                Destroy(collision.gameObject);
                return;
            }

            Die();
        }
    }

    bool IsStompingEnemy(Collision2D collision)
    {
        if (rb.linearVelocity.y > 0f)
            return false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    void Die()
    {
        isDead = true;
        int deadLayer = LayerMask.NameToLayer(deadPlayerLayerName);

        if (deadLayer != -1)
        {
            gameObject.layer = deadLayer;
        }
        // Activamos animación de muerte
        if (animator != null)
        {
            animator.speed = 1f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFlipping", false);
            animator.SetBool("isDead", true);
        }


        // Permitimos que el personaje caiga
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = defaultGravityScale;

        // Paramos todo lo que se mueve por script
        StopWorld();

        // Esperamos para mostrar Game Over
        StartCoroutine(ShowGameOverAfterDelay());
    }

    void StopWorld()
    {
        // Detiene todos los objetos que usan MoveLeft
        MoveLeft[] movers = FindObjectsByType<MoveLeft>(FindObjectsSortMode.None);

        for (int i = 0; i < movers.Length; i++)
        {
            movers[i].enabled = false;
        }

        // Detener spawners
        GroundSpawner ground = FindFirstObjectByType<GroundSpawner>();
        PlatformSpawner platform = FindFirstObjectByType<PlatformSpawner>();
        ObstacleSpawner obstacle = FindFirstObjectByType<ObstacleSpawner>();
        GemSpawner gem = FindFirstObjectByType<GemSpawner>();
        FlyingEnemySpawner flying = FindFirstObjectByType<FlyingEnemySpawner>();
        SlimeSpawner slime = FindFirstObjectByType<SlimeSpawner>();
        PowerSpawner power = FindFirstObjectByType<PowerSpawner>();

        if (ground != null) ground.enabled = false;
        if (platform != null) platform.enabled = false;
        if (obstacle != null) obstacle.enabled = false;
        if (gem != null) gem.enabled = false;
        if (flying != null) flying.enabled = false;
        if (slime != null) slime.enabled = false;
        if (power != null) power.enabled = false;
    }

    IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSecondsRealtime(deathDelay);

        if (gameUIManager != null)
        {
            gameUIManager.ShowGameOver();
        }
    }
}