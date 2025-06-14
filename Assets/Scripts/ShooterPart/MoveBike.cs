using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBike : MonoBehaviour
{
    public float speed = 2f;

    [SerializeField] private Rigidbody CircleRb;
    [SerializeField] private Rigidbody BikeRb;
    [HideInInspector] public Vector3 velocity;
    private void Start()
    {
        CircleRb.transform.parent = null;
        BikeRb.transform.parent = null;
    }

    private void Update()
    {
        transform.position = CircleRb.transform.position;
        velocity = BikeRb.transform.InverseTransformDirection(BikeRb.velocity);
    }

    private void FixedUpdate()
    {
        CircleRb.velocity = Vector3.forward * speed;
    }
}
