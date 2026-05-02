using UnityEngine;

public class GemPickup : MonoBehaviour
{
    public float energyAmount = 25f;
    public int scoreAmount = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameHUDManager hud = FindFirstObjectByType<GameHUDManager>();

            if (hud != null)
            {
                hud.AddEnergy(energyAmount);
                hud.AddScore(scoreAmount);
            }

            Destroy(gameObject);
        }
    }
}