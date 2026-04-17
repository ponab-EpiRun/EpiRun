using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Prefabs")]
    public GameObject platformPrefab;

    [Header("Generation Settings")]
    public int initialPlatforms = 2;
    public float spawnOffset = 12f;
    public float minGapX = 18f;
    public float maxGapX = 30f;
    public float skipGapX = 20f;

    [Header("Spawn Frequency")]
    [Range(0f, 1f)] public float platformChance = 0.3f;

    [Header("Height Settings")]
    public float minY = -0.5f;
    public float maxY = 3.5f;
    public float maxHeightStep = 1f;
    public float firstPlatformMinY = 0f;
    public float firstPlatformMaxY = 1f;

    [Header("Sequence Settings")]
    public float sequenceResetGapX = 20f;

    [Header("Cleanup")]
    public float destroyOffset = 10f;

    private readonly List<GameObject> activePlatforms = new List<GameObject>();
    private Camera mainCamera;
    private float lastSpawnY;
    private float lastPlatformSpawnX;
    private float nextRequiredGapX;
    private bool hasSpawnedAnyPlatform;

    void Start()
    {
        mainCamera = Camera.main;
        lastSpawnY = Mathf.Clamp(firstPlatformMinY, minY, maxY);
        lastPlatformSpawnX = 0f;
        nextRequiredGapX = Random.Range(minGapX, maxGapX);
        hasSpawnedAnyPlatform = false;

        // Generamos plataformas iniciales
        for (int i = 0; i < initialPlatforms; i++)
        {
            TrySpawnPlatform(true);
        }
    }

    void Update()
    {
        // Si la plataforma más a la derecha ya se ha alejado suficiente, intentamos generar otra plataforma
        int maxSpawnsPerFrame = 2;

        for (int i = 0; i < maxSpawnsPerFrame; i++)
        {
            if (!ShouldTrySpawn())
                break;

            TrySpawnPlatform(false);
        }

        CleanupOldPlatforms();
    }

    bool ShouldTrySpawn()
    {
        float spawnX = GetSpawnX();
        GameObject rightmostPlatform = GetRightmostPlatform();

        if (rightmostPlatform == null)
            return true;

        float distanceFromRightmostToSpawn = spawnX - rightmostPlatform.transform.position.x;
        return distanceFromRightmostToSpawn >= nextRequiredGapX;
    }

    void TrySpawnPlatform(bool forceSpawn)
    {
        float spawnX = GetSpawnX();
        bool shouldSpawn = forceSpawn || Random.value < platformChance;

        if (shouldSpawn)
        {
            SpawnPlatform(spawnX);
            nextRequiredGapX = Random.Range(minGapX, maxGapX);
        }
        else
        {
            lastPlatformSpawnX = spawnX + skipGapX;
            nextRequiredGapX = Random.Range(minGapX, maxGapX) + skipGapX;
        }
    }

    void SpawnPlatform(float spawnX)
    {
        bool isFirstPlatformOfSequence = !hasSpawnedAnyPlatform || (spawnX - lastPlatformSpawnX) >= sequenceResetGapX;

        float spawnY;

        if (isFirstPlatformOfSequence)
        {
            float safeMinY = Mathf.Clamp(firstPlatformMinY, minY, maxY);
            float safeMaxY = Mathf.Clamp(firstPlatformMaxY, minY, maxY);
            spawnY = Random.Range(safeMinY, safeMaxY);
        }
        else
        {
            float minAllowedY = Mathf.Max(minY, lastSpawnY - maxHeightStep);
            float maxAllowedY = Mathf.Min(maxY, lastSpawnY + maxHeightStep);
            spawnY = Random.Range(minAllowedY, maxAllowedY);
        }

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        activePlatforms.Add(newPlatform);
        lastSpawnY = spawnY;
        lastPlatformSpawnX = spawnX;
        hasSpawnedAnyPlatform = true;
    }

    void CleanupOldPlatforms()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float destroyLimit = mainCamera.transform.position.x - camWidth - destroyOffset;

        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i] == null)
            {
                activePlatforms.RemoveAt(i);
                continue;
            }

            if (activePlatforms[i].transform.position.x < destroyLimit)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

    float GetSpawnX()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float cameraRightEdge = mainCamera.transform.position.x + camWidth;

        return cameraRightEdge + spawnOffset;
    }

    GameObject GetRightmostPlatform()
    {
        GameObject rightmost = null;
        float maxX = float.NegativeInfinity;

        for (int i = 0; i < activePlatforms.Count; i++)
        {
            if (activePlatforms[i] == null)
                continue;

            float x = activePlatforms[i].transform.position.x;

            if (x > maxX)
            {
                maxX = x;
                rightmost = activePlatforms[i];
            }
        }

        return rightmost;
    }
}