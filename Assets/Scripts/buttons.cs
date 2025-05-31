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

    private string chooseLvl = "";
    public void StartGame()
    {
        ChooseLvlWindow.SetActive(true);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Lvl1()
    {
        chooseLvl = "Lvl1";
        Score.SetActive(true);
        LvlName.text = "Уровень 1";
    }

    public void Lvl2()
    {
        chooseLvl = "Lvl2";
        Score.SetActive(true);
        LvlName.text = "Уровень 2";
    }
    public void Lvl3()
    {
        chooseLvl = "Lvl3";
        Score.SetActive(true);
        LvlName.text = "Уровень 3";
    }

    public void LoadLvl()
    {
        SceneManager.LoadScene(chooseLvl);
    }
}
