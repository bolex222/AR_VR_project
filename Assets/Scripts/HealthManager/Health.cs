using Interfaces;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxHealth;
    public float currentHealth;
    public GameObject AudioManager;
    public AudioSource PlayerDeath;
    public AudioClip playerDeathSound;

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
        if (Input.GetKeyDown("t"))
        {
            TakeDamage(1);


        }

    }

    //[PunRPC]
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;
        
        //healthBar.SetHealth(currentHealth);
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
            Play(playerDeathSound);
            currentHealth = maxHealth;
            
            healthBar.SetMaxHealth(maxHealth);
        }
    }
    public void Play(AudioClip clip)
    {
        PlayerDeath.clip = clip;
        PlayerDeath.Play();
    }

    public void Die()
    {
        photonView.RPC("PlayerVisibility", RpcTarget.AllViaServer, false);
        _timerOn = true;

        deathScreen.gameObject.SetActive(true);
        if (photonView.IsMine)
        {
            GetSpawn();
        }
    }

    
    private void GetSpawn()
    {
        AllGenericTypes.Team team = MatchMakingNetworkManager.playersTeamA.Contains(PhotonNetwork.LocalPlayer)
                   ? AllGenericTypes.Team.TeamA
                   : AllGenericTypes.Team.TeamB;

        Transform spawn =
        team == AllGenericTypes.Team.TeamA ? SpawnerManager.instance.GetTeamSpawn(0) : SpawnerManager.instance.GetTeamSpawn(1);

        player.transform.position = spawn.position;
        player.transform.rotation = spawn.rotation;
        Physics.SyncTransforms();

        Debug.Log("Respawned to: " + spawn.position);
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
                _timerOn = false;

                photonView.RPC("ResetHealth", RpcTarget.AllViaServer);
                photonView.RPC("PlayerVisibility", RpcTarget.AllViaServer, true);

                deathScreen.gameObject.SetActive(false);
                _timeLeft = respawnTime;
                
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            healthBar.SetHealth(currentHealth);
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            healthBar.SetHealth(currentHealth);

        }
    }

}
