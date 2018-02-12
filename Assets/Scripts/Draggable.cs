using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public GameObject attachedBlock;
    Vector3 startPosition;

    public GameObject canvas;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Main.Instance.simulating)
        {
            if (attachedBlock)
            {
                Main.Instance.destroyObject(attachedBlock);
            }
            startPosition = transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Main.Instance.simulating)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Main.Instance.simulating)
        {
            transform.position = startPosition;
            Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 temp2 = new Vector3((int)temp1.x, (int)temp1.y, (int)temp1.z);

            Main.Instance.spawnBlockSprite(temp2);
            if (gameObject.name.Contains("(Clone)"))
            {
                Destroy(gameObject);
            }
        }
    }
}
