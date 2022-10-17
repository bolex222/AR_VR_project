using Interfaces;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxHealth;
    public float currentHealth;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject deathScreen;
    
    public HealthBar healthBar;

    private float respawnTime;
    private bool _timerOn = false;
    private float _timeLeft;

    private void Start()
    {
        respawnTime = GameDataManager.Instance.data.DelayRespawn;

        maxHealth = GameDataManager.Instance.data.LifeNumber;
        _timeLeft = respawnTime;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        RespawnTimer();
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        
        healthBar.SetHealth(currentHealth);
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
        
        
    }

    [PunRPC]
    private void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    [PunRPC]
    private void PlayerVisibility(bool state)
    {
        transform.GetChild(1).gameObject.SetActive(state); // hide skeleton
        transform.GetChild(2).gameObject.SetActive(state);// hide gun 
        gameObject.GetComponent<BasicRigidBodyPush>().enabled = state;//collider rigibody
        gameObject.GetComponent<StarterAssets.ThirdPersonController>().enabled = state;//input
        gameObject.GetComponent<ThirdPersonShooterController>().enabled = state;
        healthBar.gameObject.SetActive(state);
    }

    public void Die()
    {
        
        photonView.RPC("PlayerVisibility", RpcTarget.AllViaServer, false);
        _timerOn = true;
        deathScreen.gameObject.SetActive(true);
        
    }

    private void RespawnTimer()
    {
        if (_timerOn)
        {
            

            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
            }
            else
            {
                AllGenericTypes.Team team = MatchMakingNetworkManager.playersTeamA.Contains(PhotonNetwork.LocalPlayer)
                   ? AllGenericTypes.Team.TeamA
                   : AllGenericTypes.Team.TeamB;

                Transform spawn =
                team == AllGenericTypes.Team.TeamA ? SpawnerManager.instance.GetTeamSpawn(0) : SpawnerManager.instance.GetTeamSpawn(1);

                _timerOn = false;

                photonView.RPC("ResetHealth", RpcTarget.All);
                photonView.RPC("PlayerVisibility", RpcTarget.All, true);
               

                player.transform.position = spawn.position;
                player.transform.rotation = spawn.rotation;
                Physics.SyncTransforms();

                //GameManager.Instance.Spawn();

                deathScreen.gameObject.SetActive(false);
                _timeLeft = respawnTime;
                
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }
    }
}
