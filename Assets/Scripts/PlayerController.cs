using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Ajustar fuerza de salto
    public float jumpForce = 5f;

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

            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
        }
        // Doble salto en el aire
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && doubleJumpAvailable)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            doubleJumpAvailable = false;

            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
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

        // Cambiamos el color del personaje si choca con un obstáculo/enemigo para probar la colisión
        if ((collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy")) && !isDead)
        {
            isDead = true;

            if (animator != null)
            {
                animator.SetBool("isDead", true);
                animator.SetBool("isJumping", false);
            }

            if (sr != null)
            {
                sr.color = Color.red;
            }

            if (gameUIManager != null)
            {
                gameUIManager.ShowGameOver();
            }
        }
    }
}