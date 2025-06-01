using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContrManager : MonoBehaviour
{
    public bool gameplayPC = true;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
