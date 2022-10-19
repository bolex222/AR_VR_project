using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

public class BulletProjectile : MonoBehaviourPunCallbacks
{
    public float bulletSpeed = 40f;
    public float bulletDamage = 1f;
    public AllGenericTypes.Team teamToAvoid;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidbody.velocity = transform.forward * bulletSpeed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet hit: " + collision.gameObject.name);

        PlayerTeam playerTeam = collision.gameObject.GetComponentInParent<PlayerTeam>();
        if (playerTeam is not null && playerTeam.team == teamToAvoid)
        {
            Debug.Log("hit same team");
            Destroy(gameObject);
            return;
        }

        if (playerTeam)
        {
            //it's a player
            Debug.Log("Bullet hit player: " + collision.gameObject.name);
            Health health = playerTeam.GetComponent<Health>();
            health.TakeDamage(bulletDamage);
            //health.photonView.RPC("TakeDamage", RpcTarget.AllViaServer, bulletDamage);
        }

        Destroy(gameObject);
    }
}