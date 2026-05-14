using UnityEngine;


public class FlyingEnemyMovement : MonoBehaviour
{
    // Velocidad extra respecto al escenario
    public float extraSpeed = 2f;

    private MoveLeft referenceMover;

    void Start()
    {
        // Cogemos cualquier objeto que use MoveLeft como referencia
        referenceMover = FindFirstObjectByType<MoveLeft>();
    }

    void Update()
    {
        float currentSpeed = extraSpeed;

        if (referenceMover != null)
        {
            currentSpeed += referenceMover.speed;
        }

        transform.position += Vector3.left * currentSpeed * Time.deltaTime;
    }
}