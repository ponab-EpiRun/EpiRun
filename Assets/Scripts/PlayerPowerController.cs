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

    public GameObject afterImagePrefab;

    private SpriteRenderer playerSprite;
    private float afterImageTimer;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        difficultyManager = FindFirstObjectByType<DifficultyManager>();
        powerUI = FindFirstObjectByType<PowerMessageUI>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
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
                isYngviActive = true;

                if (playerController.yngviHalo != null)
                    playerController.yngviHalo.SetActive(true);

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

    public float GetYngviEnergyMultiplier()
    {
        if (isYngviActive)
        {
            isYngviActive = false;

            if (playerController.yngviHalo != null)
                playerController.yngviHalo.SetActive(false);

            return yngviEnergyMultiplier;
        }

        return 1f;
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

    void Update()
    {
        if (difficultyManager.moveSpeedMultiplier > 1f)
        {
            afterImageTimer -= Time.deltaTime;

            if (afterImageTimer <= 0f)
            {
                SpawnAfterImage();
                afterImageTimer = 0.08f;
            }
        }
    }

    void SpawnAfterImage()
    {
        GameObject img = Instantiate(
            afterImagePrefab,
            transform.position,
            Quaternion.identity
        );

        img.transform.localScale = transform.localScale;

        SpriteRenderer sr = img.GetComponent<SpriteRenderer>();

        sr.sprite = playerSprite.sprite;
        sr.flipX = playerSprite.flipX;

        sr.color = new Color(0.5f, 0.8f, 1f, 0.5f);

        sr.sortingLayerID = playerSprite.sortingLayerID;
        sr.sortingOrder = playerSprite.sortingOrder - 1;
    }

}