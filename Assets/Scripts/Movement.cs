using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody CircleRb, BikeRb;
    [SerializeField] private InputActionReference Joystick;
    [SerializeField] private Slider slider;
    [SerializeField] private Button stopButton;
    [HideInInspector] public Vector3 velocity;

    public float maxSpeed, acceleration, rotateStrength, gravity, bikeTiltInc, zTiltAngle = 45f;
    [Range(1, 10)]
    public float brakingFactor;
    public LayerMask derivableSurface;

    private bool stopRequested = false;
    private float moveInput, rotateInput, currentVelocityOffset;
    private float currentZTilt = 0f;

    float rayLength;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        CircleRb.transform.parent = null;
        BikeRb.transform.parent = null;

        if (stopButton != null)
        {
            // ��������� �������� ��� ������
            var eventTrigger = stopButton.gameObject.GetComponent<EventTrigger>() ??
                              stopButton.gameObject.AddComponent<EventTrigger>();

            // �������
            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener((data) => { stopRequested = true; });
            eventTrigger.triggers.Add(pointerDown);

            // ����������
            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener((data) => { stopRequested = false; });
            eventTrigger.triggers.Add(pointerUp);
        }

        rayLength = CircleRb.transform.GetComponent<SphereCollider>().radius * CircleRb.transform.localScale.y + 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value == 0)
        {
            Vector2 joystickInput = Joystick.action.ReadValue<Vector2>();
            moveInput = joystickInput.y;    // ����� / �����
            rotateInput = joystickInput.x;  // ����� / ������
        }
        else if (slider.value == 1)
        {
            moveInput = Input.GetAxis("Vertical");    // ����� / �����
            rotateInput = Input.GetAxis("Horizontal");  // ����� / ������
        }
        transform.position = CircleRb.transform.position;

        velocity = BikeRb.transform.InverseTransformDirection(BikeRb.velocity);
        currentVelocityOffset = velocity.z / maxSpeed;
    }

    private void FixedUpdate()
    {
        Movement_();
    }

    void Movement_()
    {
        if (Grounded())
        {
            if (!Input.GetKey(KeyCode.Space) || !stopRequested)
            {

                Acceleration();
            }
            Rotation();
            Brake();
        }
        else
        {
            Gravity();
        }
        BikeTilt();
    }

    void Rotation()
    {
        transform.Rotate(0, rotateInput * currentVelocityOffset * rotateStrength * Time.fixedDeltaTime, 0, Space.World);
    }

    private void Acceleration()
    {
        CircleRb.velocity = Vector3.Lerp(CircleRb.velocity, maxSpeed * moveInput * transform.forward, Time.fixedDeltaTime * acceleration);
    }
    void BikeTilt()
    {
        // 1. ������ �� X (�����/�����) �� �����������
        float xRot = (Quaternion.FromToRotation(BikeRb.transform.up, hit.normal) * BikeRb.transform.rotation).eulerAngles.x;

        // 2. ������� ������ �� Z (����) � �� ����� ��������!
        float targetZTilt = 0f;
        if (currentVelocityOffset > 0)
        {
            targetZTilt = -zTiltAngle * rotateInput * currentVelocityOffset;
        }

        // 3. ������ ������������� Z-������
        currentZTilt = Mathf.Lerp(currentZTilt, targetZTilt, Time.fixedDeltaTime * 5f); // 5 � �������� �����������

        // 4. �������� �������� �������
        Quaternion targetRot = Quaternion.Euler(xRot, transform.eulerAngles.y, currentZTilt);

        // 5. ������ ������� RigidBody
        BikeRb.MoveRotation(Quaternion.Slerp(BikeRb.rotation, targetRot, bikeTiltInc));
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || stopRequested)
        {
            CircleRb.velocity *= brakingFactor / 10;
        }
    }

    bool Grounded()
    {
        Ray ray = new Ray(CircleRb.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, rayLength, derivableSurface))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Gravity()
    {
        CircleRb.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }
}