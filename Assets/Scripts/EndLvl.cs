using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLvl : MonoBehaviour
{
    [SerializeField] private GameObject finish;
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI coolScore;
    [SerializeField] private TextMeshProUGUI nitroScore;
    [SerializeField] private TextMeshProUGUI timeScore;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI mark;

    int cool = 0;
    int nitro = 0;
    int time = 0;
    int timeScorePoints = 0;
    int totalScorePoints = 0;
    string totalMark = "";
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            Time.timeScale = 0;
            finish.SetActive(true);

            cool = (int)coinManager.CoolScoreEnd();
            nitro = (int)coinManager.NitroScoreEnd();
            time = ((int)coinManager.TimeScoreEnd());
            timeScorePoints = time < 31 ? 800 : time < 41 ? 600 : 0;
            totalScorePoints = cool + nitro + timeScorePoints;
            totalMark = totalScorePoints > 1800 ? "S" : totalScorePoints > 1700 ? "A" : totalScorePoints > 1600 ? "B" : totalScorePoints > 1400 ? "C" : totalScorePoints > 1200 ? "D" : "F";

            coins.text = "����������: $" + coinManager.CoinEnd().ToString();
            coolScore.text = "������ ����: " + cool.ToString();
            nitroScore.text = "���� �� �����: " + nitro.ToString();
            timeScore.text = "���� �� �����: " + timeScorePoints.ToString();
            totalScore.text = "����� ����: " + totalScorePoints.ToString();
            mark.text = "������: " + totalMark;
            timer.text = coinManager.TimerEnd(coinManager.timeElapsed).ToString();
        }
    }
}
