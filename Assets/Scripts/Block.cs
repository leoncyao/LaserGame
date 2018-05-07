using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public GameObject attachedSprite;
    

    // Collision Timers
    protected float lastColx = 0;
    protected float lastColy = 0;

    public Material Editable, nonEditable;


    public void Awake()
    {
        if (attachedSprite) {
            this.GetComponent<Renderer>().material = Editable;
        }
        else
        {
            this.GetComponent<Renderer>().material = nonEditable;
        }
    }


    public virtual Vector3 calcCol(Vector3 hitImpulse, Vector3 startVelocity)
    {
        Vector3 temp = startVelocity;
        //print(string.Format("Time.time is {0} lastColx is {1} lastColy is {2}", Time.time, lastColx, lastColy));
        if (Time.time - lastColx > 0.01)
        {
            //print(string.Format("Time.time is {0} lastCol is {1}", Time.time, lastColx));
            lastColx = Time.time;
            if (Mathf.Abs((hitImpulse.x)) > 0.00001)
            {
                // print("horizontal hit");
                //print(startVelocity.x);
                temp.x *= -1;
                // test.velocity =  new Vector3(test.velocity.x*-1, test.velocity.y, test.velocity.z);
                //print(startVelocity.x);
                temp *= 1.01f;
            }
        }
        if (Time.time - lastColy > 0.01)
            {
                lastColy = Time.time;
                if (Mathf.Abs((hitImpulse.y)) > 0.00001)
                {
                    temp.y *= -1;
                    temp *= 1.01f;
                }
            }
        
        return temp;
    }
}
