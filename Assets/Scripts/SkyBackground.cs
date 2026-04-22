using UnityEngine;


//Script para mover el fondo del cielo
public class SkyBackground : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float backgroundWidth = 20f;
    public Transform otherBackground;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x <= otherBackground.position.x - backgroundWidth)
        {
            transform.position = new Vector3(
                otherBackground.position.x + backgroundWidth,
                transform.position.y,
                transform.position.z
            );
        }
    }
}