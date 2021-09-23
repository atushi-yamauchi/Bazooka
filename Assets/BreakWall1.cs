using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall1 : MonoBehaviour
{
    bool color = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (color)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.name == "Sphere(Clone)")
        {
            GetComponent<Renderer>().material.color = Color.blue;
            color = true;
        }
    }
}
