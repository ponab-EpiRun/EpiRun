using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float speed = 0.5f;
    public float resetPositionX = -20f;
    public float startPositionX = 20f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= resetPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}