using System.Collections;
using TMPro;
using UnityEngine;

public class PowerMessageUI : MonoBehaviour
{
    public TMP_Text powerText;
    public float showTime = 2f;

    private Coroutine currentRoutine;

    void Start()
    {
        if (powerText != null)
        {
            powerText.text = "";
            powerText.enabled = false;
        }
    }

    public void ShowMessage(string message)
    {
        if (powerText == null)
            return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        powerText.enabled = true;
        powerText.text = message;

        yield return new WaitForSecondsRealtime(showTime);

        powerText.text = "";
        powerText.enabled = false;
    }
}