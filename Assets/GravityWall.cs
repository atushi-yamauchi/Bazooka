using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWall : MonoBehaviour
{
    public GameObject legLess;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(legLess == null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
