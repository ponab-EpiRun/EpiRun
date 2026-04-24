using UnityEngine;

// Script para mover el fondo del cielo sin referencias a otro fondo
public class SkyBackground : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float backgroundWidth = 20f;

    void Update()
    {
        // Movimiento hacia la izquierda
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Cuando sale completamente por la izquierda, salta delante
        if (transform.position.x <= -backgroundWidth)
        {
            transform.position = new Vector3(
                transform.position.x + (backgroundWidth * 2f),
                transform.position.y,
                transform.position.z
            );
        }
    }
}