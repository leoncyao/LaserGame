using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangularBlock : Block {
    float lastColDiag;
    float lastCol;
    public float rotationAngle;
    //public GameObject attachedSprite;
    void Start()
    {
        lastColDiag = 0;
        lastColx = 0;
        lastColy = 0;
        lastCol = 0;
        //print("rotation angle is " + rotationAngle);
    }

    public override Vector3 calcCol(Vector3 hitImpulse, Vector3 startVelocity)
    {

        Vector3 temp = startVelocity;
        //print("check for hitimpulse is " + hitImpulse);
        //print("what is going on " + Mathf.Abs(Mathf.Abs(hitImpulse.y) - Mathf.Abs(hitImpulse.x)));
        //print("check for first cond is " + (Mathf.Abs(Mathf.Abs(hitImpulse.y) - Mathf.Abs(hitImpulse.x)) < 0.001));
        if (Mathf.Abs(Mathf.Abs(hitImpulse.y) - Mathf.Abs(hitImpulse.x)) < 0.001)
        {
            //print("last col is " + lastCol);
            if (Time.time - lastColDiag > 0.1f)
            {
  
                if (rotationAngle == 0 || rotationAngle == 180)
                {
                    //print("branch1");
                    float tempVal = temp.y;
                    temp.y = temp.x;
                    temp.x = tempVal;
                    temp.x *= -1;
                    temp.y *= -1;
                }
                else
                {
                    //print("branch2");
                    float tempVal = temp.y;
                    temp.y = temp.x;
                    temp.x = tempVal;
                }
                lastColDiag = Time.time;
                temp *= 1.01f;       
            }
        }
        if (Mathf.Abs((hitImpulse.x)) > 0.00001)
        {
            if (Time.time - lastColx > 0.01)
            {
                //print("branchx");
                lastCol = Time.time;
                lastColx = Time.time;
                temp.x *= -1;
                temp *= 1.01f;
            }
        }
        if (Mathf.Abs((hitImpulse.y)) > 0.00001){
            if (Time.time - lastColy > 0.01)
            {
                print("branchy");
                lastColy = Time.time;
                temp.y *= -1;
                temp *= 1.01f;
                lastCol = Time.time;
            }
        }
        
        return temp;
    }

    private void OnMouseDown()
    {
        if (attachedSprite.GetComponent<DraggableTriangularBlock>().notDragging)
        {

            Invoke("rotateTriangle", 0.2f);
        }
    }

    void rotateTriangle()
    {
        rotationAngle += 90;
        if (rotationAngle == 360)
        {
            rotationAngle = 0;
        }
        Quaternion newRot = Quaternion.Euler(rotationAngle, 90, 90);
        this.transform.localRotation = newRot;
        attachedSprite.transform.localRotation = Quaternion.Euler(0, 0, -rotationAngle); 
    }
}
