using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idn : MonoBehaviour
{
    void Start()
    {
        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.allowOcclusionWhenDynamic = false;
        }
    }
}
