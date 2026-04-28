using System.Collections;
using UnityEngine;

public class PlayerPowerController : MonoBehaviour
{
    public float yngviDuration = 6f;
    public float skuldDuration = 4f;
    public float lokiDuration = 5f;

    public float yngviEnergyMultiplier = 2f;
    public float skuldWorldSpeedMultiplier = 0.5f;

    public bool isYngviActive = false;
    public bool isLokiActive = false;

    private PowerMessageUI powerUI;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        powerUI = FindFirstObjectByType<PowerMessageUI>();
    }

    public void ActivatePower(PowerType powerType)
    {
        switch (powerType)
        {
            case PowerType.BalderProtection:
                playerController.hasBalderProtection = true;
                ShowPowerMessage("Protección de Balder");
                break;

            case PowerType.SleipnirStep:
                ShowPowerMessage("Paso de Sleipnir");
                ActivateSleipnir();
                break;

            case PowerType.YngviBlessing:
                ShowPowerMessage("Bendición de Yngvi");
                StartCoroutine(YngviRoutine());
                break;

            case PowerType.SkuldGaze:
                ShowPowerMessage("Mirada de Skuld");
                StartCoroutine(SkuldRoutine());
                break;

            case PowerType.LokiTrick:
                ShowPowerMessage("¡Jugarreta de Loki!");
                StartCoroutine(LokiRoutine());
                break;
        }
    }

    void ShowPowerMessage(string message)
    {
        if (powerUI != null)
        {
            powerUI.ShowMessage(message);
        }
        else
        {
            Debug.Log("Poder recogido: " + message);
        }
    }

    void ActivateSleipnir()
    {
        WorldScroller worldScroller = FindFirstObjectByType<WorldScroller>();

        if (worldScroller != null)
        {
            worldScroller.scrollSpeed *= 1.5f;
        }
    }

    IEnumerator YngviRoutine()
    {
        isYngviActive = true;

        yield return new WaitForSeconds(yngviDuration);

        isYngviActive = false;
    }

    IEnumerator SkuldRoutine()
    {
        Time.timeScale = skuldWorldSpeedMultiplier;

        yield return new WaitForSecondsRealtime(skuldDuration);

        Time.timeScale = 1f;
    }

    IEnumerator LokiRoutine()
    {
        isLokiActive = true;
        playerController.canDoubleJump = false;

        yield return new WaitForSeconds(lokiDuration);

        isLokiActive = false;
        playerController.canDoubleJump = true;
    }
}