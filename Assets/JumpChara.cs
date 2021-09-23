using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChara : MonoBehaviour
{
    private Rigidbody rb;
    int count = 0;

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
        count++;
        if(count == 3)
        {
            Destroy(gameObject);
        }
    }
}
