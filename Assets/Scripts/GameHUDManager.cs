using UnityEngine;
using TMPro;

public class GameHUDManager : MonoBehaviour
{
    [Header("HUD Texts")]
    public TextMeshPro timeText;
    public TextMeshPro scoreText;
    public TextMeshPro energyText;

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

    void Start()
    {
        currentEnergy = maxEnergy;
        FindTexts();
        UpdateHUD();
    }

    void FindTexts()
    {
        if (timeText == null)
            timeText = GameObject.Find("TimeText")?.GetComponent<TextMeshPro>();

        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshPro>();

        if (energyText == null)
            energyText = GameObject.Find("EnergyText")?.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        if (isGameOver)
            return;

        gameTime += Time.deltaTime;

        // Score base por tiempo + bonus por gemas
        totalScore = Mathf.FloorToInt(gameTime * scorePerSecond) + bonusScore;

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
        bonusScore += amount;
        UpdateHUD();
    }

    public void StopHUD()
    {
        isGameOver = true;
    }
}