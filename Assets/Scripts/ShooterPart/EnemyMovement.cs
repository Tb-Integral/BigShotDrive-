using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Image healthBar;
    [SerializeField] private EnemySystem system;

    public float speed;
    public float maxHealth = 100;
    public float currentHealth;

    private Rigidbody rb;
    private bool IsShootable = false;
    private bool IsDead = false;
    private bool IsCourStart = false;
    private float currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        character = GameObject.Find("Motorcycle (1)").transform;
        currentHealth = maxHealth;
        currentSpeed = speed;
        StartCoroutine(StartAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsShootable)
        {
            if (IsDead && IsCourStart)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 0.5f);
                rb.velocity = Vector3.forward * currentSpeed;
            }
            else if (!IsDead)
            {
                currentSpeed = speed;
                rb.velocity = Vector3.forward * currentSpeed;
            }
        }
        else
        {
            rb.velocity = Vector3.forward * speed * 1.4f;
        }
    }

    public void Atacked()
    {
        currentHealth -= 10;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth == 0)
        {
            IsDead = true;
            system = GameObject.Find("EnemySystem").transform.GetComponent<EnemySystem>();
            system.DelEnemy((int)transform.position.x == -84 ? 2 : (int)transform.position.x == -75 ? 1 : 0);
            StartCoroutine(Death());
        }
    }

    private IEnumerator StartAnim()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            character.position.z - 300f
        );
        yield return new WaitForSeconds(6.8f);
        IsShootable= true;
    }

    private IEnumerator Death()
    {
        gameObject.layer = 13;
        IsCourStart= true;
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
