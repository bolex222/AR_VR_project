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
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<UserManager>())
        {
            //it's a player
            Debug.Log("Bullet hit player: " + collision.gameObject.name);
            Health health = collision.transform.GetComponent<Health>();
            health.TakeDamage(bulletDamage);
            //health.photonView.RPC("TakeDamage", RpcTarget.AllViaServer, bulletDamage);
            
        }
    }
}