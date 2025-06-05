using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSelect : MonoBehaviour
{
    private CoinManager coinManager;
    private void Start()
    {
        GetComponentInChildren<ParticleSystem>().simulationSpace = ParticleSystemSimulationSpace.World;
        coinManager = transform.parent.GetComponent<CoinManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            coinManager.coins += 25;
            coinManager.RefreshCoinText();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(0, 2f, 0);
    }
}
