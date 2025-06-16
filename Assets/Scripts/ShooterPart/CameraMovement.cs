using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference Joystick;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Camera _camera;
    [SerializeField] private Button shootButton;
    [SerializeField] private AudioManager audio;

    public float XRorate = 30f;
    public float YRorate = 30f;
    public float sensitivity = 1f;

    private Animator animator;
    private ContrManager contrManager;

    private float moveInput, rotateInput;
    private bool CanIMove = false;
    private bool CanIShoot = false;
    private float pitch = 0f;
    private float yaw = 0f;
    private float _time = 0f;

    private void Awake()
    {
        GetComponent<PlayerInput>().ActivateInput();
    }

    void Start()
    {
        contrManager = GameObject.Find("ControllerManager").GetComponent<ContrManager>();
        audio = GameObject.Find("audio").transform.GetComponent<AudioManager>();

        if (!contrManager.gameplayPC && shootButton != null)
        {
            shootButton.onClick.AddListener(ShootButtonPressed);
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
                moveInput = normalizedInput.y > 0.2f ? 1f : joystickInput.y;
            }
            else
            {
                moveInput = 0f;
            }
        }
        else
        {
            moveInput = Input.GetAxis("Vertical");
            rotateInput = Input.GetAxis("Horizontal");
        }

        // Вращение камеры
        if (CanIMove)
        {
            pitch -= moveInput * sensitivity * Time.deltaTime;
            yaw += rotateInput * sensitivity * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, -XRorate, XRorate);
            yaw = Mathf.Clamp(yaw, 180 - XRorate, 180 + XRorate);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }

        // Стрельба для ПК
        if (CanIShoot && contrManager.gameplayPC && Input.GetMouseButton(0))
        {
            Shoot();
        }
        else if (!CanIShoot && CanIMove)
        {
            CoolDown();
        }
    }

    public void ShootButtonPressed()
    {
        if (CanIShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        audio.PlayShot();
        animator.Play("weaponShoot", 0, 0f);

        Vector3 point = new(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray ray = _camera.ScreenPointToRay(point);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("enemy"))
            {
                hit.transform.root.GetComponent<EnemyMovement>().Atacked();
            }
        }

        CanIShoot = false;
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
