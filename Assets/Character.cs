using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody rb;
    bool sleep;
    bool hit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(rb.IsSleeping() == false)
        {
            sleep = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(sleep)
        {
            Destroy(gameObject);
        }
    }
}
