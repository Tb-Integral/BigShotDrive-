using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference Joystick;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Camera _camera;
    [SerializeField] private Button shootButton;

    public float XRorate = 30f;
    public float YRorate = 30f;
    public float sensitivity = 1f;

    private Animator animator;

    private ContrManager contrManager;
    private float moveInput, rotateInput;
    private bool CanIMove = false;
    private bool stopRequested = false;

    private float pitch = 0f;
    private float yaw = 0f;
    private bool CanIShoot = false;
    float _time = 0f;

    private void Awake()
    {
        GetComponent<PlayerInput>().ActivateInput();
    }

    void Start()
    {
        if (contrManager == null)
        {
            contrManager = GameObject.Find("ControllerManager").GetComponent<ContrManager>();
        }

        if (!contrManager.gameplayPC)
        {
            // Добавляем триггеры для кнопки
            var eventTrigger = shootButton.gameObject.GetComponent<EventTrigger>() ??
                              shootButton.gameObject.AddComponent<EventTrigger>();

            // Нажатие
            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener((data) => { stopRequested = true; });
            eventTrigger.triggers.Add(pointerDown);

            // Отпускание
            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener((data) => { stopRequested = false; });
            eventTrigger.triggers.Add(pointerUp);
        }

        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;

        canvas.SetActive(false);
        StartCoroutine(Anim());
    }

    void Update()
    {
        if (!contrManager.gameplayPC)
        {
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
            moveInput = Input.GetAxis("Vertical");    // Вверх / вниз
            rotateInput = Input.GetAxis("Horizontal");  // Влево / вправо
        }

        //moveCam

        if (CanIMove)
        {
            pitch -= moveInput * sensitivity * Time.deltaTime;
            yaw += rotateInput * sensitivity * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, -XRorate, XRorate);
            yaw = Mathf.Clamp(yaw, 180-XRorate, 180+XRorate);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }

        //shooting
        if (CanIShoot)
        {
            if (Input.GetMouseButton(0) || stopRequested)
            {
                animator.Play("weaponShoot", 0, 0f);
                Vector3 point = new(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
                Ray ray = _camera.ScreenPointToRay(point);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "enemy")
                    {
                        hit.transform.root.GetComponent<EnemyMovement>().Atacked();
                        CanIShoot = false;
                    }
                    CanIShoot = false;
                }
            }
        }
        else{
            if (CanIMove)
            {
                CoolDown();
            }
        }

    }

    private IEnumerator Anim()
    {
        yield return new WaitForSeconds(6.5f);
        weapon.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<CinemachineVirtualCamera>().Priority += 1;
        canvas.SetActive(true);
        CanIMove = true;
        animator = transform.GetComponentInChildren<Animator>();
        CanIShoot = true;
    }

    private void CoolDown()
    {
        CanIShoot = false;
        if (_time > 0.3f)
        {
            _time = 0;
            CanIShoot = true;
        }
        else
        {
            _time += Time.deltaTime;
        }
    }
}
