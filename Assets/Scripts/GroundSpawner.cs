using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [Header("Segment Prefabs")]
    public GameObject groundSegmentPrefab;
    public GameObject holeSegmentPrefab;

    [Header("Generation Settings")]
    public float segmentWidth = 10f;
    public int initialSegments = 6;
    public float spawnOffset = 10f;
    public float groundY = -2f;

    [Header("Probabilities")]
    [Range(0f, 1f)] public float holeChance = 0.2f;

    [Header("Safe Start")]
    public int safeStartSegments = 8;

    [Header("Cleanup")]
    public float destroyX = -20f;

    private float nextSpawnX;
    private float cameraRightEdge;
    private int spawnedSegmentsCount;
    private readonly List<GameObject> activeSegments = new List<GameObject>();
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        cameraRightEdge = mainCamera.transform.position.x + camWidth;

        // Empezamos bastante a la izquierda para que el jugador ya tenga suelo
        nextSpawnX = mainCamera.transform.position.x - camWidth - (segmentWidth * 2f);
        spawnedSegmentsCount = 0;

        // Generamos suelo inicial
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment(i == 0 ? groundSegmentPrefab : GetNextSegmentPrefab());
        }
    }

    void Update()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        cameraRightEdge = mainCamera.transform.position.x + camWidth;

        // Si el último segmento ya se acerca al borde derecho, generamos otro
        if (activeSegments.Count > 0)
        {
            int maxSpawnsPerFrame = 3;

            for (int s = 0; s < maxSpawnsPerFrame; s++)
            {
                GameObject lastSegment = activeSegments[activeSegments.Count - 1];

                if (lastSegment == null)
                    break;

                float spawnTriggerX = cameraRightEdge + spawnOffset;
                float lastSegmentRightEdge = lastSegment.transform.position.x + (segmentWidth * 0.5f);

                if (lastSegmentRightEdge >= spawnTriggerX)
                    break;

                SpawnSegment(GetNextSegmentPrefab());
            }
        }

        CleanupOldSegments();
    }

    GameObject GetNextSegmentPrefab()
    {
        // Evitar agujeros al inicio de la partida
        if (spawnedSegmentsCount < safeStartSegments)
        {
            return groundSegmentPrefab;
        }

        // Evitar dos agujeros seguidos
        if (activeSegments.Count > 0)
        {
            GameObject lastSegment = activeSegments[activeSegments.Count - 1];

            if (lastSegment != null && lastSegment.name.Contains("HoleSegment"))
            {
                return groundSegmentPrefab;
            }
        }

        return Random.value < holeChance ? holeSegmentPrefab : groundSegmentPrefab;
    }

    void SpawnSegment(GameObject segmentPrefab)
    {
        float spawnX;

        if (activeSegments.Count == 0)
        {
            spawnX = nextSpawnX + (segmentWidth * 0.5f);
        }
        else
        {
            GameObject lastSegment = activeSegments[activeSegments.Count - 1];
            float lastSegmentRightEdge = lastSegment.transform.position.x + (segmentWidth * 0.5f);
            spawnX = lastSegmentRightEdge + (segmentWidth * 0.5f);
        }

        Vector3 spawnPosition = new Vector3(spawnX, groundY, 0f);
        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);

        activeSegments.Add(newSegment);
        nextSpawnX = spawnX + (segmentWidth * 0.5f);
        spawnedSegmentsCount++;
    }

    void CleanupOldSegments()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float destroyLimit = mainCamera.transform.position.x - camWidth - 10f;

        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            if (activeSegments[i] == null)
            {
                activeSegments.RemoveAt(i);
                continue;
            }

            float segmentRightEdge = activeSegments[i].transform.position.x + (segmentWidth * 0.5f);

            if (segmentRightEdge < destroyLimit)
            {
                Destroy(activeSegments[i]);
                activeSegments.RemoveAt(i);
            }
        }
    }
}