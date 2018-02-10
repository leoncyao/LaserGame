using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour {
    /*
     * Class for useful functions
    */

    public static double findAngle(double obj1_x, double obj1_y, double obj2_x, double obj2_y)
    {

        double dx = (obj2_x - obj1_x);
        double dy = (obj2_y - obj1_y);

        double angle = 0;

        if (dx == 0)
        {
            if (dy > 0)
            {
                angle = Mathf.PI / 2;
            }
            else if (dy <= 0)
            {
                angle = 3 * Mathf.PI / 2;
            }
        }
        else
        {
            angle = Mathf.Atan((float)dy / (float)dx);
        }

        angle = Mathf.Abs((float)angle);
        //print("angle is " + Mathf.Rad2Deg*angle);
        if (dx > 0 && dy >= 0)
        {
            angle = angle;
            //print("1rst quad");
        }
        else if (dx < 0 && dy > 0)
        {
            //print("2nd quad");
            angle = Mathf.PI - angle;
        }
        else if (dx < 0 && dy <= 0)
        {
            angle = Mathf.PI + angle;
            //print("3rth quad");
        }
        else if (dx > 0 && dy < 0)
        {
            angle = 2 * Mathf.PI - angle;
            //print("4rth quad");
        }
        //        System.out.println("angle is " + String.valueOf(180/Math.PI*angle));
        return Mathf.Rad2Deg * angle;
    }


}
