using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    public int coins = 0;

    public void RefreshCoinText()
    {
        coinText.text = "$" + coins;
    }

    public int CoinEnd()
    {
        return coins;
    }
}
