using System.Collections;
using UnityEngine;

public class PlayerPowerController : MonoBehaviour
{
    public float sleipnirDuration = 5f;
    public float yngviDuration = 6f;
    public float skuldDuration = 4f;
    public float lokiDuration = 5f;

    public float yngviEnergyMultiplier = 2f;
    public float skuldWorldSpeedMultiplier = 0.5f;

    public bool isYngviActive = false;
    public bool isLokiActive = false;

    private PowerMessageUI powerUI;
    private PlayerController playerController;
    private DifficultyManager difficultyManager;
    private Animator animator;

    private Coroutine sleipnirCoroutine;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        difficultyManager = FindFirstObjectByType<DifficultyManager>();
        powerUI = FindFirstObjectByType<PowerMessageUI>();
        animator = GetComponent<Animator>();
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
            powerUI.ShowMessage(message);
        else
            Debug.Log("Poder recogido: " + message);
    }

    void ActivateSleipnir()
    {
        if (sleipnirCoroutine != null)
            StopCoroutine(sleipnirCoroutine);

        sleipnirCoroutine = StartCoroutine(SleipnirRoutine());
    }

    IEnumerator SleipnirRoutine()
    {
        Debug.Log("Sleipnir ACTIVADO");

        if (difficultyManager != null)
        {
            difficultyManager.moveSpeedMultiplier = 2f;
            difficultyManager.scoreMultiplier = 3f;
        }

        if (animator != null)
        {
            animator.speed = 3f;
        }

        yield return new WaitForSeconds(sleipnirDuration);

        if (difficultyManager != null)
        {
            difficultyManager.moveSpeedMultiplier = 1f;
            difficultyManager.scoreMultiplier = 1f;
        }

        if (animator != null)
        {
            animator.speed = 1f;
        }

        Debug.Log("Sleipnir FINALIZADO");
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