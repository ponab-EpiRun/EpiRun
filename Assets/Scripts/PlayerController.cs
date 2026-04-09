using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Ajustar fuerza de salto
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool doubleJumpAvailable = false;

    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
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
        //Modificación para que el personaje caiga más rápido de lo que sube
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {   //Al tocar el suelo, se reinician los saltos
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJumpAvailable = false;
        }
        //Cambiamos el color del personaje si choca con un obstáculo/enemigo para probar la colisión
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy") )
        {
            sr.color = Color.red;
            //Rotamos en 90 para simular muerte
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}