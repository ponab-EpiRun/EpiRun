using UnityEngine;

public class WorldScroller : MonoBehaviour
{
        //Ajustar velocidad de movimiento del terreno
    public float scrollSpeed = 3f;

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
    }
}