using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class buttons : MonoBehaviour
{
    [SerializeField] private GameObject ChooseLvlWindow;
    [SerializeField] private OnButtonClick LvlButton;
    [SerializeField] private GameObject Score;
    [SerializeField] private TextMeshProUGUI LvlName;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject lvlChoice;

    private string chooseLvl = "";
    private ContrManager contrManager;
    public void StartGame()
    {
        ChooseLvlWindow.SetActive(true);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Lvl1()
    {
        chooseLvl = "Lvl1";
        Score.SetActive(true);
        LvlName.text = "������� 1";
        Score.transform.Find("score").transform.GetComponentInChildren<TextMeshProUGUI>().text = "����: " + PlayerPrefs.GetInt("lvl1Score").ToString();
        Score.transform.Find("mark").transform.GetComponentInChildren<TextMeshProUGUI>().text = "������: " + PlayerPrefs.GetString("lvl1Mark");
    }

    public void Lvl2()
    {
        Time.timeScale = 1;
        chooseLvl = "Lvl2";
        Score.SetActive(true);
        LvlName.text = "������� 2";
        Score.transform.Find("score").transform.GetComponentInChildren<TextMeshProUGUI>().text = "����: " + PlayerPrefs.GetInt("lvl2Score").ToString();
        Score.transform.Find("mark").transform.GetComponentInChildren<TextMeshProUGUI>().text = "������: " + PlayerPrefs.GetString("lvl2Mark");
    }
    public void Lvl3()
    {
        Time.timeScale = 1;
        chooseLvl = "Lvl3";
        Score.SetActive(true);
        LvlName.text = "������� 3";
        Score.transform.Find("score").transform.GetComponentInChildren<TextMeshProUGUI>().text = "����: " + PlayerPrefs.GetInt("lvl3Score").ToString();
        Score.transform.Find("mark").transform.GetComponentInChildren<TextMeshProUGUI>().text = "������: " + PlayerPrefs.GetString("lvl3Mark");
    }

    public void LoadLvl()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(chooseLvl);
    }

    public void Settings()
    {
        settings.SetActive(true);
        Time.timeScale = 0;
    }

    public void Shop()
    {
        shop.SetActive(true);
        shop.transform.Find("balance").transform.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetInt("balance").ToString() + "$";        
    }

    public void CloseSettings()
    {
        Time.timeScale = 1;
        settings.SetActive(false);   
    }

    public void CloseShop()
    {
        shop.SetActive(false);
    }

    public void CloseLvlChoice()
    {
        lvlChoice.SetActive(false);
    }

    public void RestartLvl1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Lvl1");
    }

    public void GameplayPC()
    {
        if (contrManager == null)
        {
            contrManager = GameObject.Find("ControllerManager").GetComponent<ContrManager>();
        }
        else
        {
            contrManager.gameplayPC = true;
        }
    }

    public void GameplayMobile()
    {
        if (contrManager == null)
        {
            contrManager = GameObject.Find("ControllerManager").GetComponent<ContrManager>();
        }
        else
        {
            contrManager.gameplayPC = false;
        }
    }
}
