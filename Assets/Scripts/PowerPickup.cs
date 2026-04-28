using UnityEngine;

public class PowerPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("La runa ha tocado a: " + collision.gameObject.name);

        PlayerPowerController powerController = collision.GetComponent<PlayerPowerController>();

        if (powerController != null)
        {
            PowerType randomPower = GetRandomPower();
            powerController.ActivatePower(randomPower);
            Destroy(gameObject);
        }
    }

    PowerType GetRandomPower()
    {
        int roll = Random.Range(1, 101);

        if (roll <= 30)
            return PowerType.BalderProtection;

        if (roll <= 55)
            return PowerType.SleipnirStep;

        if (roll <= 75)
            return PowerType.YngviBlessing;

        if (roll <= 90)
            return PowerType.SkuldGaze;

        return PowerType.LokiTrick;
    }
}