using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContrManager : MonoBehaviour
{
    public bool gameplayPC = true;

    void Start()
    {
        if (!PlayerPrefs.HasKey("balance")) PlayerPrefs.SetInt("balance", 0);
        if (!PlayerPrefs.HasKey("lvl1Score")) PlayerPrefs.SetInt("lvl1Score", 0);
        if (!PlayerPrefs.HasKey("lvl1Mark")) PlayerPrefs.SetString("lvl1Mark", "--");
        if (!PlayerPrefs.HasKey("lvl2Score")) PlayerPrefs.SetInt("lvl2Score", 0);
        if (!PlayerPrefs.HasKey("lvl2Mark")) PlayerPrefs.SetString("lvl2Mark", "--");
        if (!PlayerPrefs.HasKey("lvl3Score")) PlayerPrefs.SetInt("lvl3Score", 0);
        if (!PlayerPrefs.HasKey("lvl3Mark")) PlayerPrefs.SetString("lvl3Mark", "--");

        DontDestroyOnLoad(gameObject);
    }
}
