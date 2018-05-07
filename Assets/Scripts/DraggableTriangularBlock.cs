using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class DraggableTriangularBlock : Draggable {
    public GameObject attachedSprite;
    public float lastRot;
    public bool notDragging;
    private void Start()
    {
        notDragging = true;
    }

    private void Awake()
    {

    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        //float scale = Main.getScreenSideLength();
        //transform.localScale = new Vector3(scale, scale, scale);
        if (!Main.Instance.simulating)
        {
            notDragging = false;
            if (gameObject.name.Contains("(Clone)"))
            {

                lastRot = attachedBlock.GetComponent<TriangularBlock>().rotationAngle;
                //this.transform.localRotation = Quaternion.Euler(0, 0, lastRot - 90);
            }
            if (attachedBlock)
            {
                Main.Instance.destroyObject(attachedBlock);
            }
            startPosition = transform.position;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!Main.Instance.simulating)
        {
            notDragging = true;
            if (gameObject.name.Contains("(Clone)"))
            {
                //float test = attachedBlock.GetComponent<TriangularBlock>().rotationAngle;
                Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 temp2 = new Vector3((int)temp1.x, (int)temp1.y, (int)temp1.z);
                Quaternion check = transform.localRotation;
                Main.Instance.spawnTriangularBlockSprite(temp2, lastRot, check);
                Destroy(gameObject);


            }
            else
            {
                if (UI.Instance.blockQuantity > 0)
                {
                    Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 temp2 = new Vector3((int)temp1.x, (int)temp1.y, (int)temp1.z);
                    Quaternion check = transform.localRotation;
                    Vector3 check2 = check.eulerAngles;

                    Main.Instance.spawnTriangularBlockSprite(temp2, 0, check);
                    UI.Instance.blockQuantity -= 1;
                }
            }
            transform.position = startPosition;
        }
    }
}
