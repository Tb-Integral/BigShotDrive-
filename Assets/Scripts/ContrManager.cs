using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ContrManager : MonoBehaviour
{
    private Material bikeColor1;
    private Material bikeColor2;
    public bool gameplayPC = true;
    public Color currentColor1;
    public Color currentColor2;

    public static ContrManager instance;

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (GameObject.Find("ControllerManager") != null)

        if (!PlayerPrefs.HasKey("balance")) PlayerPrefs.SetInt("balance", 0);

        if (!PlayerPrefs.HasKey("lvl1Score")) PlayerPrefs.SetInt("lvl1Score", 0);
        if (!PlayerPrefs.HasKey("lvl1Mark")) PlayerPrefs.SetString("lvl1Mark", "--");
        if (!PlayerPrefs.HasKey("lvl2Score")) PlayerPrefs.SetInt("lvl2Score", 0);
        if (!PlayerPrefs.HasKey("lvl2Mark")) PlayerPrefs.SetString("lvl2Mark", "--");
        if (!PlayerPrefs.HasKey("lvl3Score")) PlayerPrefs.SetInt("lvl3Score", 0);
        if (!PlayerPrefs.HasKey("lvl3Mark")) PlayerPrefs.SetString("lvl3Mark", "--");

        if (!PlayerPrefs.HasKey("redBikeSkin")) PlayerPrefs.SetInt("redBikeSkin", 1);
        if (!PlayerPrefs.HasKey("greenBikeSkin")) PlayerPrefs.SetInt("greenBikeSkin", 0);
        if (!PlayerPrefs.HasKey("blueBikeSkin")) PlayerPrefs.SetInt("blueBikeSkin", 0);
        if (!PlayerPrefs.HasKey("pinkBikeSkin")) PlayerPrefs.SetInt("pinkBikeSkin", 0);
        if (!PlayerPrefs.HasKey("goldBikeSkin")) PlayerPrefs.SetInt("goldBikeSkin", 0);
        if (!PlayerPrefs.HasKey("whiteBikeSkin")) PlayerPrefs.SetInt("whiteBikeSkin", 0);

        if (!PlayerPrefs.HasKey("equipColor")) PlayerPrefs.SetString("equipColor", "red");

        GameObject bikeForColor = GameObject.Find("rootBike").transform.Find("Motorcycle (1)").Find("Bike").Find("Bike").gameObject;
        bikeColor1 = bikeForColor.GetComponent<Renderer>().materials[4];
        currentColor1 = bikeForColor.GetComponent<Renderer>().materials[4].color;
        bikeColor2 = bikeForColor.GetComponent<Renderer>().materials[3];
        currentColor2 = bikeForColor.GetComponent<Renderer>().materials[3].color;

        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        GameObject bikeForColor = GameObject.Find("rootBike").transform.Find("Motorcycle (1)").Find("Bike").Find("Bike").gameObject;
        bikeColor1 = bikeForColor.GetComponent<Renderer>().materials[4];
        bikeColor1.SetColor("_BaseColor", currentColor1);
        bikeColor2 = bikeForColor.GetComponent<Renderer>().materials[3];
        bikeColor2.SetColor("_BaseColor", currentColor2);
    }

    public void ChangeColor(Color color)
    {
        currentColor1 = color;
        bikeColor1.SetColor("_BaseColor", color);
        currentColor2 = color;
        bikeColor2.SetColor("_BaseColor", color);
    }
}
