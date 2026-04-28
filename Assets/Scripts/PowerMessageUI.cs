using System.Collections;
using TMPro;
using UnityEngine;

public class PowerMessageUI : MonoBehaviour
{
    public TMP_Text powerText;
    public float showTime = 2f;

    Coroutine currentRoutine;

    void Start()
    {
        powerText.text = "";
    }

    public void ShowMessage(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        powerText.text = message;
        powerText.gameObject.SetActive(true);

        yield return new WaitForSeconds(showTime);

        powerText.text = "";
        powerText.gameObject.SetActive(false);
    }
}