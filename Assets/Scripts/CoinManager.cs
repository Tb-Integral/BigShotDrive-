using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private GameObject CloseCarScoreWindow;
    [SerializeField] private Transform LinesFolder;
    [SerializeField] private Movement bike;
    [SerializeField] private AudioManager audio;
    public int coins = 0;
    public float coolScore = 0;
    public float nitroScore = 0;
    public float timeElapsed = 0f;

    private string[] CloseCarLines = {"Это было близко!", "Круто!", "Учи других\nездить!", "Не Формула-1,\nно ты близок!", "Ювелирно!", "Ты чё, каскадёр?!", "Адреналин\nв норме!", "По лезвию ножа!" };

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        timer.text = UpdateTimerDisplay(timeElapsed);

        if (bike.IsNitroActive) nitroScore += 0.2f;
    }

    public void RefreshCoinText()
    {
        audio.PlayCoin();
        coinText.text = "$" + coins;
    }
    public void RefreshScoreText()
    {
        StartCoroutine(CarLines());
    }

    public int CoinEnd()
    {
        return coins;
    }

    public float CoolScoreEnd()
    {
        return coolScore;
    }

    public float NitroScoreEnd()
    {
        return nitroScore;
    }

    public float TimeScoreEnd()
    {
        return timeElapsed;
    }

    public string TimerEnd(float _time)
    {
        return UpdateTimerDisplay(_time);
    }

    private IEnumerator CarLines()
    {
        GameObject window = Instantiate(CloseCarScoreWindow, LinesFolder);

        Text textComponent = window.transform.Find("CarLine").GetComponent<Text>();
        textComponent.text = CloseCarLines[Random.Range(0, CloseCarLines.Length)];

        CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = window.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(1);

        RectTransform rect = window.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, -100);

        float duration = 2f;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            canvasGroup.alpha = 1f - t;

            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(window);
    }

    private string UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("Время: {0:00}:{1:00}", minutes, seconds);
    }


}
