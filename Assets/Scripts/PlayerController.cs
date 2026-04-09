using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Ajustar fuerza de salto
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool doubleJumpAvailable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Al tocar el suelo, se reinician los saltos
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJumpAvailable = false;
        }
    }
}