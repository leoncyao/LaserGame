using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    Rigidbody test;
    public Vector3 startVelocity;
    //float lastColx = 0;
    //float lastColy = 0;
    //Vector3 hitImpulse;
    void Start()
    {
    }
    private void Awake()
    {
        test = GetComponent<Rigidbody>();
        //hitImpulse = Vector3.zero;
    }

    void Update()
    {
        test.velocity = startVelocity;
    }

    private void OnCollisionStay(Collision col)
    {
        
        if (col.gameObject.GetComponent<Laser>())
        {
            Physics.IgnoreCollision(col.collider, GetComponent<Collider>());
        }
        if (col.gameObject.GetComponent<Block>())
        {
            startVelocity = col.gameObject.GetComponent<Block>().calcCol(col.impulse, startVelocity);
        }
        if (col.gameObject.GetComponent<TriangularBlock>())
        {
            startVelocity = col.gameObject.GetComponent<TriangularBlock>().calcCol(col.impulse, startVelocity);
        }
    }
}


