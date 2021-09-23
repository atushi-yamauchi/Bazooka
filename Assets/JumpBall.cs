using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBall : MonoBehaviour
{
    private Rigidbody rb;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(0, 10, 0, ForceMode.Impulse);
    }
}
