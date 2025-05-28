using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarMove : MonoBehaviour
{
    public float MaxZ;
    public float MinZ;
    public float speed;

    private bool forward = true;

    void Update()
    {
        if (forward)
        {
            
            if (transform.position.z > MaxZ)
            {
                forward = false;
            }
            else
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
        }
        else
        {
            
            if (transform.position.z < MinZ)
            {
                forward = true;
            }
            else
            {
                transform.Translate(0, 0, -speed * Time.deltaTime);
            }
        }
    }
}

