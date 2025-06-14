using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public ContrManager contrManager;

    private void Start()
    {
        contrManager = GameObject.Find("ControllerManager").GetComponent<ContrManager>();
    }

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
        LvlName.text = "”ровень 1";
        Score.transform.Find("score").transform.GetComponentInChildren<TextMeshProUGUI>().text = "—чет: " + PlayerPrefs.GetInt("lvl1Score").ToString();
        Score.transform.Find("mark").transform.GetComponentInChildren<TextMeshProUGUI>().text = "ќценка: " + PlayerPrefs.GetString("lvl1Mark");
    }

    public void Lvl1Shooter()
    {
        SceneManager.LoadScene("Lvl1Shooter");
    }

    public void Lvl2()
    {
        Time.timeScale = 1;
        chooseLvl = "Lvl2";
        Score.SetActive(true);
        LvlName.text = "”ровень 2";
        Score.transform.Find("score").transform.GetComponentInChildren<TextMeshProUGUI>().text = "—чет: " + PlayerPrefs.GetInt("lvl2Score").ToString();
        Score.transform.Find("mark").transform.GetComponentInChildren<TextMeshProUGUI>().text = "ќценка: " + PlayerPrefs.GetString("lvl2Mark");
    }
    public void Lvl3()
    {
        Time.timeScale = 1;
        chooseLvl = "Lvl3";
        Score.SetActive(true);
        LvlName.text = "”ровень 3";
        Score.transform.Find("score").transform.GetComponentInChildren<TextMeshProUGUI>().text = "—чет: " + PlayerPrefs.GetInt("lvl3Score").ToString();
        Score.transform.Find("mark").transform.GetComponentInChildren<TextMeshProUGUI>().text = "ќценка: " + PlayerPrefs.GetString("lvl3Mark");
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
        contrManager.gameplayPC = true;

    }

    public void GameplayMobile()
    {

        contrManager.gameplayPC = false;

    }

    public void BuyColor()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        ShopItem shopItem = clickedButton.GetComponent<ShopItem>();

        if (!shopItem.IsBuyed)
        {
            if (PlayerPrefs.GetInt("balance") >= 200)
            {
                PlayerPrefs.SetInt("balance", PlayerPrefs.GetInt("balance") - 200);

                Transform shop = GameObject.Find("Canvas").transform.Find("Shop").Find("balance");
                shop.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetInt("balance").ToString() + "$";

                if (clickedButton.transform.name == "ShopItemGreen") PlayerPrefs.SetInt("greenBikeSkin", 1);
                if (clickedButton.transform.name == "ShopItemBlue") PlayerPrefs.SetInt("blueBikeSkin", 1);
                if (clickedButton.transform.name == "ShopItemPink") PlayerPrefs.SetInt("pinkBikeSkin", 1);
                if (clickedButton.transform.name == "ShopItemGold") PlayerPrefs.SetInt("goldBikeSkin", 1);
                if (clickedButton.transform.name == "ShopItemWhite") PlayerPrefs.SetInt("whiteBikeSkin", 1);
            }
        }
        else if (!shopItem.IsEquiped)
        {
            if (clickedButton.transform.name == "ShopItemRed") PlayerPrefs.SetString("equipColor", "red");
            if (clickedButton.transform.name == "ShopItemGreen") PlayerPrefs.SetString("equipColor", "green");
            if (clickedButton.transform.name == "ShopItemBlue") PlayerPrefs.SetString("equipColor", "blue");
            if (clickedButton.transform.name == "ShopItemPink") PlayerPrefs.SetString("equipColor", "pink");
            if (clickedButton.transform.name == "ShopItemGold") PlayerPrefs.SetString("equipColor", "gold");
            if (clickedButton.transform.name == "ShopItemWhite") PlayerPrefs.SetString("equipColor", "white");
        }
        shopItem.SwitchColors();
    }
}
