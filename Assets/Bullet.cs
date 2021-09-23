using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float explosionForce;
    public float explosionR;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player")
        {
            Debug.Log(explosionForce);
            Debug.Log(explosionR);
            GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionR);
            Destroy(gameObject);
        }
    }

}
