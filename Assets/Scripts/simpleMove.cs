using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class simpleMove : MonoBehaviour
{
    [SerializeField] private Rigidbody CircleRb, BikeRb;
    [SerializeField] private InputActionReference Joystick;
    public float maxSpeed, acceleration, rotateStrength;

    private float moveInput, rotateInput;
    // Start is called before the first frame update
    void Start()
    {
        CircleRb.transform.parent = null;
        BikeRb.transform.parent = null;
    }

    private void Update()
    {
        Vector2 joystickInput = Joystick.action.ReadValue<Vector2>();
        transform.position = CircleRb.transform.position;
        moveInput = joystickInput.y;    // Вперёд / назад
        rotateInput = joystickInput.x;  // Влево / вправо
        BikeRb.MoveRotation(transform.rotation);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }   

    private void Movement()
    {
        CircleRb.velocity = Vector3.Lerp(CircleRb.velocity, maxSpeed * moveInput * transform.forward, Time.fixedDeltaTime * acceleration);
    }

    private void Rotation()
    {
        transform.Rotate(0, rotateInput * moveInput * rotateStrength * Time.fixedDeltaTime, 0, Space.World);
    }
}
