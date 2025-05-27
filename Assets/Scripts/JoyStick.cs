using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject Handle;
    //�����

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 inputPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        //������� ������ �� ������ � ������� ������

        Handle.transform.position = new Vector3(inputPosition.x, inputPosition.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Handle.transform.localPosition = Vector3.zero;
        //�� ��������� �������������� �����, ������ � � ��������� �������.
    }
}