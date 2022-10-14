using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float speedBullet;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidbody.velocity = transform.forward * speedBullet;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.GetComponent<BulletTarget>() != null)
        {
            Debug.Log("Hit target");
        } else
        {
            Debug.Log("Hit something else");
        }*/
        Destroy(gameObject);
    }
}