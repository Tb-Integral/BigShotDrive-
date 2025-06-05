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
    public TrailRenderer skidMarks1;
    public TrailRenderer skidMarks2;

    public float maxSpeed, acceleration, rotateStrength, gravity, bikeTiltInc, zTiltAngle = 45f, skidWildth = 0.062f, minSkidVelocity = 10f, norDrag = 2f, driftDrag = 0.5f;
    [Range(1, 10)]
    public float brakingFactor;
    public LayerMask derivableSurface;

    private bool stopRequested = false;
    private float moveInput, rotateInput, currentVelocityOffset;
    private float currentZTilt = 0f;
    public ContrManager contrManager;

    float rayLength;
    RaycastHit hit;

    private void Awake()
    {
        GetComponent<PlayerInput>().ActivateInput();
    }

    void Start()
    {
        if (contrManager == null)
        {
            contrManager = GameObject.Find("ControllerManager").GetComponent<ContrManager>();

            if (!contrManager.gameplayPC && stopButton != null)
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
        }

        CircleRb.transform.parent = null;
        BikeRb.transform.parent = null;

        

        rayLength = CircleRb.transform.GetComponent<SphereCollider>().radius * CircleRb.transform.localScale.y + 0.7f;

        skidMarks1.startWidth = skidWildth;
        skidMarks1.emitting = false;
        skidMarks2.startWidth = skidWildth;
        skidMarks2.emitting = false;
    }

    void Update()
    {
        if (!contrManager.gameplayPC)
        {
            Vector2 joystickInput = Joystick.action.ReadValue<Vector2>();
            moveInput = joystickInput.y;    // Вперёд / назад
            rotateInput = joystickInput.x;  // Влево / вправо
        }
        else if (contrManager.gameplayPC)
        {
            moveInput = Input.GetAxis("Vertical");    // Вперёд / назад
            rotateInput = Input.GetAxis("Horizontal");  // Влево / вправо
        }
        transform.position = CircleRb.transform.position;

        velocity = BikeRb.transform.InverseTransformDirection(BikeRb.velocity);
        currentVelocityOffset = velocity.z / maxSpeed;
    }

    private void FixedUpdate()
    {
        Movement_();
        SkidMarks();
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
        bool isBraking = (Input.GetKey(KeyCode.Space) || stopRequested);

        if (isBraking && Mathf.Abs(rotateInput) > 0.1f)
        {
            float sharpTurnStrength = rotateStrength * 1.5f;
            transform.Rotate(
                0,
                rotateInput * sharpTurnStrength * Time.fixedDeltaTime,
                0,
                Space.World
            );
        }
        else
        {
            transform.Rotate(
                0,
                rotateInput * currentVelocityOffset * rotateStrength * Time.fixedDeltaTime,
                0,
                Space.World
            );
        }
    }


    private void Acceleration()
    {
        CircleRb.velocity = Vector3.Lerp(CircleRb.velocity, maxSpeed * moveInput * transform.forward, Time.fixedDeltaTime * acceleration);
    }
    void BikeTilt()
    {
        Ray ray = new Ray(CircleRb.position, Vector3.down);
        Physics.Raycast(ray, out hit, rayLength + 3f, derivableSurface);
        // 1. Наклон по X (вперёд/назад) по поверхности
        float xRot = (Quaternion.FromToRotation(BikeRb.transform.up, hit.normal) * BikeRb.transform.rotation).eulerAngles.x;

        // 2. Целевой наклон по Z (вбок) — не задаём напрямую!
        float targetZTilt = 0f;
        if (currentVelocityOffset > 0)
        {
            targetZTilt = -zTiltAngle * rotateInput * currentVelocityOffset;
        }

        // 3. Плавно интерполируем Z-наклон
        currentZTilt = Mathf.Lerp(currentZTilt, targetZTilt, Time.fixedDeltaTime * 4f); // 5 — скорость сглаживания

        // 4. Собираем итоговый поворот
        Quaternion targetRot = Quaternion.Euler(xRot, transform.eulerAngles.y, currentZTilt);

        // 5. Плавно вращаем RigidBody
        BikeRb.MoveRotation(Quaternion.Slerp(BikeRb.rotation, targetRot, bikeTiltInc));
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || stopRequested)
        {
            CircleRb.velocity *= brakingFactor / 10;
            CircleRb.drag = driftDrag;
        }
        else
        {
            CircleRb.drag = norDrag;
        }
    }

    bool Grounded()
    {
        float radius = rayLength - 0.2f;
        Vector3 origin = CircleRb.transform.position + radius * Vector3.up;
        if (Physics.SphereCast(origin,radius + 0.02f, -transform.up, out hit, rayLength, derivableSurface))
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

    void SkidMarks()
    {
        skidMarks1.emitting = (Input.GetKey(KeyCode.Space) || stopRequested) && Grounded() && (Mathf.Abs(velocity.z) <= minSkidVelocity);
        skidMarks2.emitting = (Input.GetKey(KeyCode.Space) || stopRequested) && Grounded() && (Mathf.Abs(velocity.z) <= minSkidVelocity);
    }
}