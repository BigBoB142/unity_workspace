using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float explosionRadius = 100f;
    public float explosionForce = 2000f;
    public GameObject explosionEffect;

    public XRGrabInteractable Pin;

    private bool hasExploded = false;
    private bool isPined = false;

    public void Start()
    {

    }

    public void GrenadeExplode()
    {
        StartCoroutine(Countdown());
    }
    
    public void PinDeattach()
    {
        if (isPined) return;
        isPined = true;
        Rigidbody PinRB = Pin.GetComponent<Rigidbody>();
        PinRB.isKinematic = false;
        Pin.transform.SetParent(null);

        Destroy(Pin);

    }
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }
    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(gameObject);
    }
}
