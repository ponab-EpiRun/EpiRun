using UnityEngine;

public class PlayerController : MonoBehaviour
{
        //Ajustar fuerza de salto
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isGrounded = false;
            }
        }


            void OnCollisionEnter2D(Collision2D collision)
            {               //Suelo = objeto Ground
                    if (collision.gameObject.name == "Ground")
                    {
                        isGrounded = true;
                    }
            }
}