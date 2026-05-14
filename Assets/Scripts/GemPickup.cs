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
            PlayerPowerController powerController = collision.GetComponent<PlayerPowerController>();

            if (hud != null)
            {
                float multiplier = 1f;

                if (powerController != null)
                {
                    multiplier = powerController.GetYngviEnergyMultiplier();
                }

                hud.AddEnergy(energyAmount * multiplier);
                hud.AddScore(scoreAmount);
            }

            Destroy(gameObject);
        }
    }
}