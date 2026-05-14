using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    // Velocidad extra respecto al escenario
    public float extraSpeed = 1.5f;

    private MoveLeft referenceMover;

    void Start()
    {
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