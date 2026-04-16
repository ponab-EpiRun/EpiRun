using UnityEngine;

//Usado para eliminar los objetos que se salgan de la pantalla y optimizar el juego
public class DestroyOffscreen : MonoBehaviour
{
    public float destroyX = -15f;

    void Update()
    {
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}