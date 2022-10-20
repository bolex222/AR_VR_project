using Interfaces;
using Photon.Pun;
using UnityEngine;

namespace Shooter
{
    public class BulletProjectile : MonoBehaviourPunCallbacks
    {
        public float bulletSpeed = 40f;
        public float bulletDamage = 1f;
        public AllGenericTypes.Team teamToAvoid;

        private bool hasAlreadyHit;

        private Rigidbody bulletRigidbody;

        private void Awake()
        {
            bulletRigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Debug.Log($"bullet team start: {teamToAvoid}");
            bulletRigidbody.velocity = transform.forward * bulletSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"bullet team at trigger enter: {teamToAvoid} + trigger name : {other.gameObject.name}");
            // photonView.RPC("EnterCollistion", RpcTarget.AllViaServer, other);
            EnterCollistion(other);
        }


        private void OnCollisionEnter(Collision collision)
        {
            // EnterCollistion();
        }

        // [PunRPC]
        private void EnterCollistion(Collider collision)
        {
            Debug.Log("just collision");

            PlayerTeam playerTeam = collision.gameObject.GetComponentInParent<PlayerTeam>();
            if (playerTeam is not null && playerTeam.team == teamToAvoid)
            {
                Debug.Log($"bullet team to avoid and team hitted id {playerTeam.team}");
                Debug.Log("============= END OF BULLET PATH =========================");
                // Destroy(gameObject);
                return;
            }

            if (playerTeam)
            {
                if (!hasAlreadyHit)
                {
                    //it's a player
                    Health health = playerTeam.GetComponent<Health>();
                    health.TakeDamage(bulletDamage);
                    //health.photonView.RPC("TakeDamage", RpcTarget.AllViaServer, bulletDamage);
                    hasAlreadyHit = true;
                    Debug.Log($"bullet hitted the player {playerTeam.gameObject.name}");
                }
            }

            Destroy(gameObject);
            Debug.Log("Bullet destroyed");
            Debug.Log("============= END OF BULLET PATH =========================");
        }
    }
}