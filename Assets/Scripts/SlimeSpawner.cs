using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;

    // Tiempo antes de empezar a generar slimes
    public float startSpawningAfter = 30f;

    // Tiempo entre slimes
    public float minSpawnTime = 4f;
    public float maxSpawnTime = 8f;

    [Header("Spawn Position")]
    public float spawnOffsetX = 15f;
    public float rayStartY = 5f;
    public float rayDistance = 20f;
    public float slimeYOffset = -0.2f;

    private float gameTimer;
    private float spawnTimer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        ResetTimer();
    }

    void Update()
    {
        gameTimer += Time.deltaTime;

        if (gameTimer < startSpawningAfter)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnSlime();
            ResetTimer();
        }
    }

    void SpawnSlime()
    {
        if (slimePrefab == null)
        {
            Debug.LogError("Falta asignar slimePrefab en el objeto: " + gameObject.name);
            enabled = false;
            return;
        }

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float cameraRightEdge = mainCamera.transform.position.x + camWidth;

        float spawnX = cameraRightEdge + spawnOffsetX;

        Vector2 rayOrigin = new Vector2(spawnX, rayStartY);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance);

        if (hit.collider == null)
            return;

        if (!hit.collider.CompareTag("Ground"))
            return;

        Vector3 spawnPosition = new Vector3(
            spawnX,
            hit.point.y + slimeYOffset,
            0f
        );

        Instantiate(slimePrefab, spawnPosition, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}