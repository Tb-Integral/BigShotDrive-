using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public string nextTarget;
    public Transform path;
    public float maxAngle = 45f;
    public float maxMotorTorque;
    public float currentSpeed;
    public float maxSpeed = 100f;
    public bool inversion = false;

    [SerializeField] private WheelCollider WheelFL;
    [SerializeField] private WheelCollider WheelFR;

    private List<Transform> nodes;
    private int currentNode;
    // Start is called before the first frame update
    void Start()
    {
        nodes = path.GetComponentsInChildren<Transform>()
            .Where(node => node.transform != path.transform)
            .ToList();

        //Transform minDirection = nodes[currentNode];
        Vector3 minVectorToTarget = nodes[currentNode].position - transform.position;

        foreach (Transform node in nodes)
        {
            
            Vector3 currentTargetVector = node.position - transform.position;

            if ( (Vector3.Angle(currentTargetVector, transform.forward) < 5f) && ((currentTargetVector).magnitude < minVectorToTarget.magnitude))
            {
                minVectorToTarget = node.position - transform.position;

                currentNode = nodes.IndexOf(node);
            }
        }
        transform.GetComponent<Rigidbody>().velocity = maxSpeed/2 * transform.forward;

        nextTarget = nodes[currentNode].name;
    }

    private void FixedUpdate()
    {
        WheelRotate();
        CarMovement();
        CheckNextTarget();
        Debug.DrawLine(transform.position, transform.position + 5 * transform.forward, Color.red);

    }

    private void WheelRotate()
    {
        Vector3 nextTargetNode = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (nextTargetNode.x / nextTargetNode.magnitude) * maxAngle;

        float speedReduction = 1f - Mathf.Abs(newSteer) / maxAngle * 0.7f;
        maxSpeed = Mathf.Lerp(30f, 140f, speedReduction);

        WheelFL.steerAngle = newSteer;
        WheelFR.steerAngle = newSteer;
    }

    private void CarMovement()
    {
        //currentSpeed = 2 * Mathf.PI * WheelFL.radius * WheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed)
        {
            WheelFL.motorTorque = maxMotorTorque;
            WheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            WheelFL.motorTorque = 0;
            WheelFR.motorTorque = 0;
        }
    }

    private void CheckNextTarget()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 4f)
        {
            if (inversion)
            {
                if (currentNode != 0)
                {
                    currentNode -= 1;
                }
                else
                {
                    currentNode = nodes.Count - 1;
                }
            }
            else
            {
                if (currentNode != nodes.Count - 1)
                {
                    currentNode += 1;
                }
                else
                {
                    currentNode = 0;
                }
            }
            nextTarget = nodes[currentNode].name;
        }
    }
}
