using UnityEngine;

//Usado para mover hacia la izquierda objetos, enemigos, cofres y gemas
public class MoveLeft : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}