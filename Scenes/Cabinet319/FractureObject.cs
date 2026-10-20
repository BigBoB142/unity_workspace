    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureObject : MonoBehaviour
{
    public GameObject fracturedObject;

    public float forceThreshold = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "molotCollision")
        {
            var rb = other.GetComponentInParent<Rigidbody>();
            Debug.Log(rb.velocity.sqrMagnitude);

            if (rb.velocity.sqrMagnitude > forceThreshold)
            {
                Instantiate(fracturedObject, transform.position, transform.localRotation);
                Destroy(gameObject);
            }

        }
    }
}
