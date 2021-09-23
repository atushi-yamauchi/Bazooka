using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider col;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        col = GetComponent<CapsuleCollider>();
        col.enabled = true;
    }

    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        col.enabled = false;
    }
}
