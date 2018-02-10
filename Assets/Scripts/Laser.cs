using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    Rigidbody test;
    public Vector3 startVelocity;
    float lastColx = 0;
    float lastColy = 0;
    Vector3 hitImpulse;
    void Start()
    {
        //startVelocity = Main.Instance.ballVelocity;
        //print("Start velocity is " + startVelocity);
        //test = GetComponent<Rigidbody>();
        //hitImpulse = Vector3.zero;
    }
    private void Awake()
    {
        //print("Start velocity is " + startVelocity);
        test = GetComponent<Rigidbody>();
        hitImpulse = Vector3.zero;
    }

    void Update()
    {
        test.velocity = startVelocity;
        //print("test.velocity is " + test.velocity);
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.GetComponent<Laser>())
        {
            Physics.IgnoreCollision(col.collider, GetComponent<Collider>());
        }
        if (col.gameObject.GetComponent<Block>())
        {
            //print(string.Format("Time.time is {0} lastColx is {1} lastColy is {2}", Time.time, lastColx, lastColy));
            hitImpulse = col.impulse;
            if (Time.time - lastColx > 0.01)
            {
                //print(string.Format("Time.time is {0} lastCol is {1}", Time.time, lastColx));
                lastColx = Time.time;
                if (Mathf.Abs((hitImpulse.x)) > 0.00001)
                {
                    // print("horizontal hit");
                    //print(startVelocity.x);
                    startVelocity.x *= -1;
                    // test.velocity =  new Vector3(test.velocity.x*-1, test.velocity.y, test.velocity.z);
                    //print(startVelocity.x);
                    startVelocity *= 1.01f;
                }
            }
            if (Time.time - lastColy > 0.01)
            {
                lastColy = Time.time;
                if (Mathf.Abs((hitImpulse.y)) > 0.00001)
                {
                    //print(Mathf.Abs((int)(hitImpulse.y)));
                    //print(Mathf.Abs((int)(hitImpulse.y)) > 0.01);
                    //  print("Vertical hit");
                    //print(startVelocity.y);
                    startVelocity.y *= -1;

                    //test.velocity = new Vector3(test.velocity.x , test.velocity.y * -1, test.velocity.z);
                    //print(startVelocity.y);
                    startVelocity *= 1.01f;
                }
            }
            //else if (Mathf.Abs((int)(hitImpulse.z)) > 0.0000000001)
            //{
            //    //  print("Vertical hit");
            //    //print(startVelocity.y);
            //    startVelocity.z *= -1;
            //    //test.velocity = new Vector3(test.velocity.x, test.velocity.y , test.velocity.z * -1);
            //    //print(startVelocity.y);
            //}
            //startVelocity *= 1.01f;
        }
    }
}


