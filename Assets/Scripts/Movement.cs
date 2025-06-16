using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float nitroDuration = 2f; // Длительность нитро
    [SerializeField] private float nitroRechargeRate = 0.5f; // Скорость восстановления
    [SerializeField] private float nitroDrainRate = 1f; // Скорость расхода
    [SerializeField] private InputActionReference nitroButton;
    [SerializeField] private RectTransform nitroEffects;
    [SerializeField] private AudioManager audio;
    [HideInInspector] public Vector3 velocity;
    public TrailRenderer skidMarks1;
    public TrailRenderer skidMarks2;
    public float NirtoSpeed;
    public bool IsNitroActive = false;
    public float maxSpeed, acceleration, rotateStrength, gravity, bikeTiltInc, zTiltAngle = 45f, skidWildth = 0.062f, minSkidVelocity = 10f, norDrag = 2f, driftDrag = 0.5f;
    [Range(1, 10)]
    public float brakingFactor;
    public LayerMask derivableSurface;

    private bool stopRequested = false; 
    private float moveInput, rotateInput, currentVelocityOffset;
    private float currentZTilt = 0f;
    private ContrManager contrManager;
    private float nitroCharge = 1f; // от 0 до 1
    private float originalMaxSpeed;
    private bool nitroReleased = true;
    private bool CanRidingAudio = false;
    private bool IsAudioPlaying = false;
    private bool IsAudioDrift = false;
    private bool IsDrift = false;

    bool nitroKeyPressed = false;
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

        originalMaxSpeed = maxSpeed;

        rayLength = CircleRb.transform.GetComponent<SphereCollider>().radius * CircleRb.transform.localScale.y + 0.7f;

        skidMarks1.startWidth = skidWildth;
        skidMarks1.emitting = false;
        skidMarks2.startWidth = skidWildth;
        skidMarks2.emitting = false;

        if (audio == null)
        {
            audio = GameObject.Find("audio").transform.GetComponent<AudioManager>();
        }
    }

    void Update()
    {
        transform.position = CircleRb.transform.position;

        
        if (!contrManager.gameplayPC)
        {
            nitroKeyPressed = nitroButton.action.IsPressed();
            Vector2 joystickInput = Joystick.action.ReadValue<Vector2>();

            rotateInput = joystickInput.x;

            if (joystickInput.magnitude > 0.05f)
            {
                Vector2 normalizedInput = joystickInput.normalized;

                if (normalizedInput.y > 0.2f)
                {
                    moveInput = 1f;
                }
                else
                {
                    moveInput = joystickInput.y;
                }
            }
            else
            {
                moveInput = 0f;
            }
        }
        else if (contrManager.gameplayPC)
        {
            nitroKeyPressed = Input.GetKey(KeyCode.LeftShift);
            moveInput = Input.GetAxis("Vertical");    // Вперёд / назад
            rotateInput = Input.GetAxis("Horizontal");  // Влево / вправо
        }

        velocity = BikeRb.transform.InverseTransformDirection(BikeRb.velocity);
        currentVelocityOffset = velocity.z / maxSpeed;

        if (velocity.magnitude > 4f)
        {
            CanRidingAudio = true;
        }
        else
        {
            CanRidingAudio = false;
            IsAudioPlaying = false;
            audio.StopRiding();
        }

        if (CanRidingAudio && !IsAudioPlaying)
        {
            IsAudioPlaying = true;
            audio.PlayRiding();
        }
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
            Nitro();
            Brake();
        }
        else
        {
            Gravity();
        }
        BikeTilt();
    }

    void Nitro()
    {
        if (nitroCharge == 0f)
        {
            nitroReleased = true;
        }
        else if (nitroCharge == 1f)
        {
            nitroReleased = false;
        }

        if (nitroKeyPressed && nitroCharge > 0f && !nitroReleased && moveInput != 0)
        {
            if (!IsNitroActive)
            {
                IsNitroActive = true;
                maxSpeed = NirtoSpeed;
            }

            audio.transform.root.GetComponent<AudioSource>().pitch = 1.5f;

            nitroCharge -= nitroDrainRate * Time.deltaTime;
            nitroCharge = Mathf.Clamp01(nitroCharge);
        }
        else
        {
            if (nitroReleased || !nitroKeyPressed || moveInput == 0)
            {
                if (IsNitroActive)
                {
                    IsNitroActive = false;
                    maxSpeed = originalMaxSpeed;
                }

                audio.transform.root.GetComponent<AudioSource>().pitch = 1f;

                nitroCharge += nitroRechargeRate * Time.deltaTime;
                nitroCharge = Mathf.Clamp01(nitroCharge);
            }
        }

        float targetScale = IsNitroActive ? 1f : 2f;
        Vector3 currentScale = nitroEffects.localScale;
        nitroEffects.localScale = Vector3.Lerp(currentScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime * 10f);

        if (slider != null)
        {
            slider.value = nitroCharge;
        }
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
            IsDrift = true;
        }
        else
        {
            CircleRb.drag = norDrag;
            IsDrift = false;
        }
        if (IsDrift)
        {
            if (!IsAudioDrift)
            {
                IsAudioDrift = true;
                audio.PlayDrift();
            }
        }
        else
        {
            IsAudioDrift = false;
            audio.StopDrift();
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