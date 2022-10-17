using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletProjectile : MonoBehaviourPunCallbacks
{
    public float bulletSpeed = 40f;
    public float bulletDamage = 1f;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidbody.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            Debug.Log("Bullet hit: " + collision.gameObject.name);

            Health health = collision.transform.GetComponent<Health>();
            //health.TakeDamage(bulletDamage);
            health.photonView.RPC("TakeDamage", RpcTarget.All, bulletDamage);
        }
    }
}