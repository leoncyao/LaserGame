using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public GameObject attachedBlock;
    protected Vector3 startPosition;
    public GameObject canvas;



    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        //float scale = Main.getScreenSideLength();
        //transform.localScale = new Vector3(scale, scale, scale);
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

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!Main.Instance.simulating)
        {
            if (gameObject.name.Contains("(Clone)"))
            {
                //float test = attachedBlock.GetComponent<TriangularBlock>().rotationAngle;
                Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 temp2 = new Vector3((int)temp1.x, (int)temp1.y, (int)temp1.z);
                Main.Instance.spawnBlockSprite(temp2);
                Destroy(gameObject);
            }
            else
            {
                if (UI.Instance.blockQuantity > 0)
                {
                    Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 temp2 = new Vector3((int)temp1.x, (int)temp1.y, (int)temp1.z);
                    Main.Instance.spawnBlockSprite(temp2);
                    UI.Instance.blockQuantity -= 1;
                }
            }
            transform.position = startPosition;
        }
        //}
        //if (!Main.Instance.simulating)
        //{
        //    transform.position = startPosition;
        //    if (UI.Instance.blockQuantity > 0)
        //    {
        //        Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        Vector3 temp2 = new Vector3((int)temp1.x, (int)temp1.y, (int)temp1.z);
        //        Main.Instance.spawnBlockSprite(temp2);
        //        if (gameObject.name.Contains("(Clone)"))
        //        {
        //            Destroy(gameObject);
        //        }
        //        else
        //        {
        //            UI.Instance.blockQuantity -= 1;
        //        }
        //    }
        //}
    }
}
