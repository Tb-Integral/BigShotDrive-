using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public Transform path;
    public float maxAngle = 45f;
    public float maxMotorTorque;
    public float currentSpeed;
    public float maxSpeed = 100f;
    public float NextCarStop = 10f;

    [SerializeField] private WheelCollider WheelFL;
    [SerializeField] private WheelCollider WheelFR;

    private List<Transform> nodes;
    private int currentNode;

    void Start()
    {
        nodes = path.GetComponentsInChildren<Transform>()
            .Where(node => node.transform != path.transform)
            .ToList();

        Vector3 minVectorToTarget = nodes[currentNode].position - transform.position;
        float minAngle = 10f;
        foreach (Transform node in nodes)
        {
            Vector3 currentTargetVector = node.position - transform.position;

            if ((Vector3.Angle(currentTargetVector, transform.forward) < minAngle))
            {
                minAngle = Vector3.Angle(currentTargetVector, transform.forward);
                if ((currentTargetVector).magnitude < minVectorToTarget.magnitude)
                {
                    minVectorToTarget = node.position - transform.position;
                    currentNode = nodes.IndexOf(node);
                }
            }
        }

        if (currentNode == nodes.Count - 1)
        {
            if (((Vector3.Angle(nodes[currentNode].position - transform.position, transform.forward))) > (Vector3.Angle(nodes[0].position - transform.position, transform.forward)))
            {
                currentNode = 0;
            }
        }
        else
        {
            if (((Vector3.Angle(nodes[currentNode].position - transform.position, transform.forward))) > (Vector3.Angle(nodes[currentNode + 1].position - transform.position, transform.forward)))
            {
                currentNode++;
            }
        }

        transform.GetComponent<Rigidbody>().velocity = maxSpeed/2 * transform.forward;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 8f, out hit, NextCarStop))
        {
            if (hit.transform.tag == "Car" || hit.transform.tag == "Player")
            {
                transform.GetComponent<Rigidbody>().velocity /= 10f;
            }
        }
    }

    private void FixedUpdate()
    {
        WheelRotate();
        CarMovement();
        CheckNextTarget();
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
            if (currentNode != nodes.Count - 1)
            {
                currentNode += 1;
            }
            else
            {
                currentNode = 0;
            }
            transform.GetComponent<Rigidbody>().velocity = maxSpeed / 7 * transform.forward;
        }
    }
}
