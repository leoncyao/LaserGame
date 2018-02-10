using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour {

    public int score = 0;
    public int required;
    public bool completed;

	void Start () {
        score = 0;
        completed = false;
        required = 2;
	}

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Laser>())
        {
            score += 1;
            if (score > required)
            {
                completed = true;
            }

            Main.Instance.destroyObject(col.gameObject);
        }


    }
}
