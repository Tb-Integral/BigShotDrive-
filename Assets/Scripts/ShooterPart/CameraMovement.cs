using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference Joystick;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject weapon;

    public float XRorate = 30f;
    public float YRorate = 30f;
    public float sensitivity = 1f;

    private ContrManager contrManager;
    private float moveInput, rotateInput;
    private bool CanIMove = false;

    private float pitch = 0f;
    private float yaw = 0f;

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
            moveInput = Input.GetAxis("Vertical");    // ¬верх / вниз
            rotateInput = Input.GetAxis("Horizontal");  // ¬лево / вправо
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
    }

    private IEnumerator Anim()
    {
        yield return new WaitForSeconds(6.5f);
        weapon.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<CinemachineVirtualCamera>().Priority += 1;
        canvas.SetActive(true);
        CanIMove = true;
    }
}
