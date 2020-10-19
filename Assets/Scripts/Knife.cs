using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Rigidbody knifeRigidbody;
    public bool bHasHitGround = false;
    // Start is called before the first frame update
    void Start()
    {
        knifeRigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bHasHitGround)
        {
            this.transform.LookAt(this.transform.position + knifeRigidbody.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            bHasHitGround = true;
            knifeRigidbody.isKinematic = true;
        }
    }
}
