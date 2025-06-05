using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLvl : MonoBehaviour
{
    [SerializeField] private GameObject finish;
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI mark;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            Time.timeScale = 0;
            finish.SetActive(true);
            coins.text = "����������: $" + coinManager.CoinEnd().ToString();
        }
    }
}
