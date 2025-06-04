using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path : MonoBehaviour
{
    public Color gizmColor;

    private List<Transform> nodes = new List<Transform>();
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmColor;
        nodes = transform.GetComponentsInChildren<Transform>()
            .Where(nodes => nodes.transform != transform)
            .ToList();

        for (int i = 0; i < nodes.Count(); i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;
            if (i > 0)
            {
                previousNode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count() > 1)
            {
                previousNode = nodes[nodes.Count() - 1].position;
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawSphere(currentNode, 2f);
        }
    }
}
