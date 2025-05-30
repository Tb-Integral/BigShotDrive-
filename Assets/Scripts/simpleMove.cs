using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class simpleMove : MonoBehaviour
{
    [SerializeField] private Rigidbody CircleRb, BikeRb;
    [SerializeField] private InputActionReference Joystick;
    [SerializeField] private Slider slider;
    [SerializeField] private Button stopButton;

    public float maxSpeed, acceleration, rotateStrength, rayLength, gravity;
    [Range(1, 10)]
    public float breakingFactor;
    public LayerMask derivableSurface;

    private float moveInput, rotateInput;
    private bool IsGrounded;
    private bool stopRequested = false;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        CircleRb.transform.parent = null;
        BikeRb.transform.parent = null;

        if (stopButton != null)
        {
            // Добавляем триггеры для кнопки
            var eventTrigger = stopButton.gameObject.GetComponent<EventTrigger>() ??
                              stopButton.gameObject.AddComponent<EventTrigger>();

            // Нажатие
            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener((data) => { stopRequested = true; });
            eventTrigger.triggers.Add(pointerDown);

            // Отпускание
            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener((data) => { stopRequested = false; });
            eventTrigger.triggers.Add(pointerUp);
        }

        float scaledRadius = CircleRb.GetComponent<SphereCollider>().radius * CircleRb.transform.lossyScale.y;
        rayLength = scaledRadius + 0.2f;
    }

    private void Update()
    {
        if (slider.value == 0)
        {
            Vector2 joystickInput = Joystick.action.ReadValue<Vector2>();
            moveInput = joystickInput.y;    // Вперёд / назад
            rotateInput = joystickInput.x;  // Влево / вправо
        }
        else if (slider.value == 1)
        {
            moveInput = Input.GetAxis("Vertical");    // Вперёд / назад
            rotateInput = Input.GetAxis("Horizontal");  // Влево / вправо
        }
        transform.position = CircleRb.transform.position;

        BikeRb.MoveRotation(transform.rotation);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (Grounded())
        {
            if (Input.GetKey(KeyCode.Space) || stopRequested)
            {
                Stop(); // если торможение — только тормозим
            }
            else
            {
                Acceleration(); // иначе — едем
            }
            Rotation();
        }
        else
        {
            Gravity();
        }
    }

    private void Acceleration()
    {
        CircleRb.velocity = Vector3.Lerp(CircleRb.velocity, maxSpeed * moveInput * transform.forward, Time.fixedDeltaTime * acceleration);
    }

    private void Rotation()
    {
        transform.Rotate(0, rotateInput * rotateStrength * Time.fixedDeltaTime, 0, Space.World);
    }

    public void Stop()
    {
        if (Input.GetKey(KeyCode.Space) || stopRequested)
        {
            CircleRb.velocity *= breakingFactor / 10;
        }
    }

    bool Grounded()
    {
        if (Physics.Raycast(CircleRb.position, Vector3.down, out hit, rayLength, derivableSurface))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Gravity()
    {
        CircleRb.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }
}