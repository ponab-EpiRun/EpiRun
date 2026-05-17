using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float lifeTime = 0.3f;

    private SpriteRenderer spriteRenderer;
    private Color startColor;
    private float startLifeTime;
    private DifficultyManager difficultyManager;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        startLifeTime = lifeTime;
        difficultyManager = FindFirstObjectByType<DifficultyManager>();
    }

    void Update()
    {
        float moveSpeed = 3f;

        if (difficultyManager != null)
            moveSpeed = difficultyManager.currentMoveSpeed;

        transform.position += Vector3.left * (moveSpeed * 1.8f) * Time.deltaTime;

        lifeTime -= Time.deltaTime;

        float alpha = lifeTime / startLifeTime;

        spriteRenderer.color = new Color(
            startColor.r,
            startColor.g,
            startColor.b,
            alpha
        );

        if (lifeTime <= 0f)
            Destroy(gameObject);
    }
}