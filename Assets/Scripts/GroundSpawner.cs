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
    public float spawnOffset = 2f;
    public float groundY = -2f;

    [Header("Probabilities")]
    [Range(0f, 1f)] public float holeChance = 0.2f;

    [Header("Cleanup")]
    public float destroyX = -20f;

    private float nextSpawnX;
    private float cameraRightEdge;
    private readonly List<GameObject> activeSegments = new List<GameObject>();

    void Start()
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        cameraRightEdge = Camera.main.transform.position.x + camWidth;

        // Empezamos bastante a la izquierda para que el jugador ya tenga suelo
        nextSpawnX = Camera.main.transform.position.x - camWidth - (segmentWidth * 2f);

        // Generamos suelo inicial
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment(i == 0 ? groundSegmentPrefab : GetNextSegmentPrefab());
        }
    }

    void Update()
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;
        cameraRightEdge = Camera.main.transform.position.x + camWidth;

        // Si el último segmento ya se acerca al borde derecho, generamos otro
        if (activeSegments.Count > 0)
        {
            GameObject lastSegment = activeSegments[activeSegments.Count - 1];

            if (lastSegment != null)
            {
                float spawnTriggerX = cameraRightEdge + spawnOffset;
                float lastSegmentRightEdge = lastSegment.transform.position.x + (segmentWidth * 0.5f);

                if (lastSegmentRightEdge < spawnTriggerX)
                {
                    SpawnSegment(GetNextSegmentPrefab());
                }
            }
        }

        CleanupOldSegments();
    }

    GameObject GetNextSegmentPrefab()
    {
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
        Vector3 spawnPosition = new Vector3(nextSpawnX + (segmentWidth * 0.5f), groundY, 0f);
        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);

        activeSegments.Add(newSegment);
        nextSpawnX += segmentWidth;
    }

    void CleanupOldSegments()
    {
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            if (activeSegments[i] == null)
            {
                activeSegments.RemoveAt(i);
                continue;
            }

            float segmentRightEdge = activeSegments[i].transform.position.x + (segmentWidth * 0.5f);

            if (segmentRightEdge < destroyX)
            {
                Destroy(activeSegments[i]);
                activeSegments.RemoveAt(i);
            }
        }
    }
}