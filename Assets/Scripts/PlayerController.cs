using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Ajustar fuerza de salto
    public float jumpForce = 5f;

    // Tiempo antes de mostrar Game Over
    public float deathDelay = 9.2f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool doubleJumpAvailable = false;
    private bool isDead = false;

    private SpriteRenderer sr;
    private Animator animator;
    private GameUIManager gameUIManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        gameUIManager = FindFirstObjectByType<GameUIManager>();

        // Estado inicial del Animator
        if (animator != null)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.speed = 1f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isDead", false);
        }
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
        }
        // Doble salto en el aire
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && doubleJumpAvailable)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            doubleJumpAvailable = false;
        }

        // Modificación para que el personaje caiga más rápido de lo que sube
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Al tocar el suelo, se reinician los saltos
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJumpAvailable = false;

            if (animator != null)
            {
                animator.SetBool("isJumping", false);
            }
        }

        // Muerte al chocar con obstáculo/enemigo
        if ((collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy")) && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Activamos animación de muerte
        if (animator != null)
        {
            animator.speed = 1f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isDead", true);

            // Forzamos el estado de muerte desde el principio
           // animator.Play("Player_dying", 0, 0f);
        }

        // Detenemos gravedad y movimiento del jugador
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

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

        if (ground != null) ground.enabled = false;
        if (platform != null) platform.enabled = false;
        if (obstacle != null) obstacle.enabled = false;
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