using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    //[SerializeField] private Transform vfxAnimatedHit3;
    //[SerializeField] private Transform vfxAnimatedHit1;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 40f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.GetComponent<BulletTarget>() != null)
        //{
        //    Debug.Log("Hit target");
        //} else
        //{
        //    Debug.Log("Hit something else");
        //}
        Destroy(gameObject);
    }
}