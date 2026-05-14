using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    // Velocidad extra respecto al escenario
    public float extraSpeed = 2.5f;

    private DifficultyManager difficultyManager;

    void Start()
    {
        difficultyManager = FindFirstObjectByType<DifficultyManager>();
    }

    void Update()
    {
        float currentSpeed = extraSpeed;

        if (difficultyManager != null)
        {
            currentSpeed = difficultyManager.currentMoveSpeed + extraSpeed;
        }

        transform.position += Vector3.left * currentSpeed * Time.deltaTime;
    }
}