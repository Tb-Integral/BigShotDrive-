using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
    //shooter
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private TextMeshProUGUI deathsText;

    int coinsPoints = 0;
    int cool = 0;
    int nitro = 0;
    int time = 0;
    int timeScorePoints = 0;
    int totalScorePoints = 0;
    string totalMark = "";
    //shooter
    int deaths = 0;
    int deathsScore = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            GameObject.Find("audio").transform.GetComponent<AudioSource>().priority = 1;
            Time.timeScale = 0;
            finish.SetActive(true);

            if (coinManager)
            {
                coinsPoints = coinManager.CoinEnd();
                cool = (int)coinManager.CoolScoreEnd();
                nitro = (int)coinManager.NitroScoreEnd();
                time = ((int)coinManager.TimeScoreEnd());
                timeScorePoints = time < 31 ? 800 : time < 41 ? 600 : time > 120 ? -2000 : time > 90 ? -500 : time > 60 ? -200 : 0;
                totalScorePoints = cool + nitro + timeScorePoints;
                totalMark = totalScorePoints > 1800 ? "S" : totalScorePoints > 1700 ? "A" : totalScorePoints > 1600 ? "B" : totalScorePoints > 1400 ? "C" : totalScorePoints > 1200 ? "D" : "F";

                coins.text = "Заработано: $" + coinsPoints.ToString();
                coolScore.text = "Крутой счет: " + cool.ToString();
                nitroScore.text = "Счет за нитро: " + nitro.ToString();
                timeScore.text = "Счет за время: " + timeScorePoints.ToString();
                totalScore.text = "Полный счет: " + totalScorePoints.ToString();
                mark.text = "Оценка: " + totalMark;
                timer.text = coinManager.TimerEnd(coinManager.timeElapsed).ToString();
            }
            else if (enemySystem)
            {
                deaths = enemySystem.deaths;
                deathsScore = deaths * 100;

                deathsText.text = "Побеждено врагов: " + deaths.ToString();
                totalScore.text = "Полный счет: " + deathsScore.ToString();

                totalMark = deathsScore >= 1000 ? "S" : deathsScore >= 900 ? "A" : deathsScore >= 700 ? "B" : deathsScore >= 500 ? "C" : deathsScore >= 400 ? "D" : "F";
                mark.text = "Оценка: " + totalMark;
            }

        }
    }

    public void EndLvlAndSaveResults()
    {
        PlayerPrefs.SetInt("balance", PlayerPrefs.GetInt("balance")+ coinsPoints);
        if (SceneManager.GetActiveScene().name == "Lvl1" && (totalScorePoints > PlayerPrefs.GetInt("lvl1Score")))
        {
            PlayerPrefs.SetInt("lvl1Score", totalScorePoints);
            PlayerPrefs.SetString("lvl1Mark", totalMark);
        }
        else if (SceneManager.GetActiveScene().name == "Lvl2" && (deathsScore > PlayerPrefs.GetInt("lvl2Score")))
        {
            PlayerPrefs.SetInt("lvl2Score", deathsScore);
            PlayerPrefs.SetString("lvl2Mark", totalMark);
        }
        else if (SceneManager.GetActiveScene().name == "Lvl3" && (totalScorePoints > PlayerPrefs.GetInt("lvl3Score")))
        {
            PlayerPrefs.SetInt("lvl3Score", totalScorePoints);
            PlayerPrefs.SetString("lvl3Mark", totalMark);
        }
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLvl2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Lvl2");
    }

    public void NextLvl3()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Lvl3");
    }
}
