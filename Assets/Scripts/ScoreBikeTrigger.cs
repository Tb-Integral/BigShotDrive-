using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreBikeTrigger : MonoBehaviour
{
    [SerializeField] private CoinManager scoreManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Car")
        {
            scoreManager.score += 100;
            scoreManager.RefreshScoreText();
        }
    }
}
