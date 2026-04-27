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
    public float energyDrainPerSecond = 30f;

    private float gameTime;
    private int score;
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
        {
            GameObject obj = GameObject.Find("TimeText");
            if (obj != null) timeText = obj.GetComponent<TextMeshPro>();
        }

        if (scoreText == null)
        {
            GameObject obj = GameObject.Find("ScoreText");
            if (obj != null) scoreText = obj.GetComponent<TextMeshPro>();
        }

        if (energyText == null)
        {
            GameObject obj = GameObject.Find("EnergyText");
            if (obj != null) energyText = obj.GetComponent<TextMeshPro>();
        }
    }

    void Update()
    {
        if (isGameOver)
            return;

        gameTime += Time.deltaTime;
        score = Mathf.FloorToInt(gameTime * scorePerSecond);

        currentEnergy -= energyDrainPerSecond * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

        if (currentEnergy <= 0f)
        {
            isGameOver = true;
            Debug.Log("GAME OVER - Sin energía");
        }

        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (timeText != null)
            timeText.text = "Tiempo: " + Mathf.FloorToInt(gameTime) + "s";

        if (scoreText != null)
            scoreText.text = "Puntuación: " + score;

        if (energyText != null)
            energyText.text = "Energía: " + Mathf.FloorToInt(currentEnergy) + "%";
    }

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
    }

    public void StopHUD()
    {
        isGameOver = true;
    }
}