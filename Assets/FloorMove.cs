using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMove : MonoBehaviour
{
    int counter = 0;
    float move = 0.03f;

    void Update()
    {
        Vector3 p = new Vector3(0, move, 0);
        transform.Translate(p);
        counter++;

        if (counter == 700)
        {
            counter = 0;
            move *= -1;
        }
    }
}