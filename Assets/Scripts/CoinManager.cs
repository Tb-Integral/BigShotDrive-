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
    [SerializeField] private RectTransform canvas;
    public int coins = 0;
    public int score = 0;
    public float timeElapsed = 0f;

    private string[] CloseCarLines = {"��� ���� ������!", "�����!", "��� ������\n������!", "�� �������-1,\n�� �� ������!", "��������!", "�� ��, �������?!", "���������\n� �����!", "�� ������ ����!" };

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        timer.text = UpdateTimerDisplay(timeElapsed);
    }

    public void RefreshCoinText()
    {
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

    public int ScoreEnd()
    {
        return score;
    }

    public string TimerEnd(float _time)
    {
        return UpdateTimerDisplay(_time);
    }

    private IEnumerator CarLines()
    {
        GameObject window = Instantiate(CloseCarScoreWindow, canvas);

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
        return string.Format("�����: {0:00}:{1:00}", minutes, seconds);
    }


}
