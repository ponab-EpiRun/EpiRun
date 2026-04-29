using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    // Adaptamos los tiempos
    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 3f;

    [Header("Spawn Position")]
    public float spawnOffsetX = 15f;
    public float obstacleY = -1.5f;

    [Header("Ground Check")]
    public float rayStartY = 5f;
    public float rayDistance = 20f;

    private float spawnTimer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        ResetTimer();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnObstacle();
            ResetTimer();
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Falta asignar obstaclePrefab en el objeto: " + gameObject.name);
            enabled = false;
            return;
        }

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float cameraRightEdge = mainCamera.transform.position.x + camWidth;

        float spawnX = cameraRightEdge + spawnOffsetX;

        Vector2 rayOrigin = new Vector2(spawnX, rayStartY);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance);

        Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red, 1f);

        if (hit.collider == null)
        {
            Debug.Log("No hay suelo debajo del spawn");
            return;
        }

        Debug.Log("Raycast toca: " + hit.collider.gameObject.name + " | Tag: " + hit.collider.tag);

        if (!hit.collider.CompareTag("Ground"))
        {
            Debug.Log("No spawnea porque no es Ground");
            return;
        }

        Vector3 spawnPosition = new Vector3(spawnX, obstacleY, 0f);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}