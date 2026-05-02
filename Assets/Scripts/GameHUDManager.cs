using UnityEngine;
using TMPro;

public class GameHUDManager : MonoBehaviour
{
    [Header("HUD Texts")]
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text energyText;

    [Header("Score Settings")]
    public int scorePerSecond = 10;

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float energyDrainPerSecond = 5f;

    private float gameTime;
    private int bonusScore;
    private int totalScore;

    private float currentEnergy;
    private bool isGameOver = false;

    private DifficultyManager difficultyManager;

    void Start()
    {
        currentEnergy = maxEnergy;

        difficultyManager = FindFirstObjectByType<DifficultyManager>();

        FindTexts();
        UpdateHUD();
    }

    void FindTexts()
    {
        if (timeText == null)
            timeText = GameObject.Find("TimeText")?.GetComponent<TMP_Text>();

        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();

        if (energyText == null)
            energyText = GameObject.Find("EnergyText")?.GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (isGameOver)
            return;

        gameTime += Time.deltaTime;

        float multiplier = 1f;

        if (difficultyManager != null)
            multiplier = difficultyManager.scoreMultiplier;

        // Score base por tiempo + bonus por gemas
        totalScore = Mathf.FloorToInt(gameTime * scorePerSecond * multiplier) + bonusScore;

        currentEnergy -= energyDrainPerSecond * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (timeText != null)
            timeText.text = "Tiempo: " + Mathf.FloorToInt(gameTime) + "s";

        if (scoreText != null)
            scoreText.text = "Puntuación: " + totalScore;

        if (energyText != null)
            energyText.text = "Energía: " + Mathf.FloorToInt(currentEnergy) + "%";
    }

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UpdateHUD();
    }

    public void AddScore(int amount)
    {
        float multiplier = 1f;

        if (difficultyManager != null)
            multiplier = difficultyManager.scoreMultiplier;

        bonusScore += Mathf.RoundToInt(amount * multiplier);
        UpdateHUD();
    }

    public void StopHUD()
    {
        isGameOver = true;
    }
}