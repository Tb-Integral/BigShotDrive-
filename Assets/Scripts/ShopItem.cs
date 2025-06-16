using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private GameObject ShopItemsConteiner;

    public Color color;
    public bool IsBuyed;
    public bool IsEquiped;

    private ShopItem[] shopItemsList;
    private ContrManager contrManager;

    private void Start()
    {
        shopItemsList = transform.parent.gameObject.GetComponentsInChildren<ShopItem>();

        if (gameObject.name == "ShopItemRed") IsBuyed = PlayerPrefs.GetInt("redBikeSkin") == 1;
        if (gameObject.name == "ShopItemGreen") IsBuyed = PlayerPrefs.GetInt("greenBikeSkin") == 1;
        if (gameObject.name == "ShopItemBlue") IsBuyed = PlayerPrefs.GetInt("blueBikeSkin") == 1;
        if (gameObject.name == "ShopItemPink") IsBuyed = PlayerPrefs.GetInt("pinkBikeSkin") == 1;
        if (gameObject.name == "ShopItemGold") IsBuyed = PlayerPrefs.GetInt("goldBikeSkin") == 1;
        if (gameObject.name == "ShopItemWhite") IsBuyed = PlayerPrefs.GetInt("whiteBikeSkin") == 1;

        string currentColorEquip = PlayerPrefs.GetString("equipColor");
        if (gameObject.name == "ShopItemRed" && currentColorEquip == "red") IsEquiped = true;
        if (gameObject.name == "ShopItemGreen" && currentColorEquip == "green") IsEquiped = true;
        if (gameObject.name == "ShopItemBlue" && currentColorEquip == "blue") IsEquiped = true;
        if (gameObject.name == "ShopItemPink" && currentColorEquip == "pink") IsEquiped = true;
        if (gameObject.name == "ShopItemGold" && currentColorEquip == "gold") IsEquiped = true;
        if (gameObject.name == "ShopItemWhite" && currentColorEquip == "white") IsEquiped = true;

        if (IsBuyed)
        {
            if (IsEquiped)
            {
                transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "Покрашено";
                Color myColor;
                ColorUtility.TryParseHtmlString("#49984E", out myColor);
                transform.Find("Price").GetComponent<TextMeshProUGUI>().color = myColor;
            }
            else
            {
                transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "Покрасить";
                Color myColor;
                ColorUtility.TryParseHtmlString("#9A9A9A", out myColor);
                transform.Find("Price").GetComponent<TextMeshProUGUI>().color = myColor;
            }
        }
    }

    public void SwitchColors()
    {
        if (gameObject.name == "ShopItemRed") IsBuyed = PlayerPrefs.GetInt("redBikeSkin") == 1;
        if (gameObject.name == "ShopItemGreen") IsBuyed = PlayerPrefs.GetInt("greenBikeSkin") == 1;
        if (gameObject.name == "ShopItemBlue") IsBuyed = PlayerPrefs.GetInt("blueBikeSkin") == 1;
        if (gameObject.name == "ShopItemPink") IsBuyed = PlayerPrefs.GetInt("pinkBikeSkin") == 1;
        if (gameObject.name == "ShopItemGold") IsBuyed = PlayerPrefs.GetInt("goldBikeSkin") == 1;
        if (gameObject.name == "ShopItemWhite") IsBuyed = PlayerPrefs.GetInt("whiteBikeSkin") == 1;

        string currentColorEquip = PlayerPrefs.GetString("equipColor");
        if (gameObject.name == "ShopItemRed" && currentColorEquip == "red") IsEquiped = true;
        if (gameObject.name == "ShopItemGreen" && currentColorEquip == "green") IsEquiped = true;
        if (gameObject.name == "ShopItemBlue" && currentColorEquip == "blue") IsEquiped = true;
        if (gameObject.name == "ShopItemPink" && currentColorEquip == "pink") IsEquiped = true;
        if (gameObject.name == "ShopItemGold" && currentColorEquip == "gold") IsEquiped = true;
        if (gameObject.name == "ShopItemWhite" && currentColorEquip == "white") IsEquiped = true;

        if (IsBuyed)
        {
            if (IsEquiped)
            {
                contrManager = GameObject.Find("ControllerManager")?.GetComponent<ContrManager>();
                contrManager.ChangeColor(color);

                transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "Покрашено";
                Color myColor;
                ColorUtility.TryParseHtmlString("#49984E", out myColor);
                transform.Find("Price").GetComponent<TextMeshProUGUI>().color = myColor;

                foreach (ShopItem item in shopItemsList)
                {
                    if (item.transform.name != transform.name && item.IsBuyed)
                    {
                        item.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "Покрасить";
                        ColorUtility.TryParseHtmlString("#9A9A9A", out myColor);
                        item.transform.Find("Price").GetComponent<TextMeshProUGUI>().color = myColor;
                        item.IsEquiped = false;
                    }
                }
            }
            else
            {
                transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "Покрасить";
                Color myColor;
                ColorUtility.TryParseHtmlString("#9A9A9A", out myColor);
                transform.Find("Price").GetComponent<TextMeshProUGUI>().color = myColor;
            }
        }
    }
}
